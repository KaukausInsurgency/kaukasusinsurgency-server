using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer
{
    public class UpdateMetaVersion
    {
        private IDbConnection Connection;
        private ILogger Logger;

        public UpdateMetaVersion(IDbConnection connection, ILogger logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public bool UpdateVersionInfo()
        {
            string Version = GlobalConfig.GetConfig().Version;
            string Guid = GlobalConfig.GetConfig().VersionKey;
            Logger.Log("Updating Version Information in database...");
            if (CheckDBConnection())
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE meta SET tcp_version = '{Version}', tcp_guid = '{Guid}'";

                try
                {
                    if (cmd.ExecuteNonQuery() <= 0)
                    {
                        Logger.Log("ERROR - Failed to update version information in database - the query did not effect any records");
                        return false;
                    }                 
                    else
                    {
                        Logger.Log($"Successfully updated verion information in database (tcp_version='{Version}', tcp_guid='{Guid}')");
                        return true;
                    }
                }
                catch(InvalidOperationException ex)
                {
                    Logger.Log($"ERROR - An error occurred while trying to update information in database - {ex.Message}");
                    return false;
                }              
            }
            else
            {
                Logger.Log("ERROR - could not update version information in database because the connection could not be established");
                return false;
            }
        }

        private bool CheckDBConnection()
        {
            if (Connection.State == ConnectionState.Open)
                return true;
            else
            {
                try
                {
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    Logger.Log("Error - Could not connect to MySql DB - " + ex.Message);
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();

                    return false;
                }
                return true;
            }
        }

        private void CloseConnection()
        {
            if (Connection != null)
                if (Connection.State == System.Data.ConnectionState.Open || Connection.State == System.Data.ConnectionState.Connecting)
                    Connection.Close();
        }
    }
}
