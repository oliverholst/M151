using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics;

namespace Garage.Models
{
    /// <summary>
    /// Öffnet/Schliesst die vebindung zur MSSQL_Datenbank
    /// </summary>
    public class MSSqlBase
    {
        /// <summary>
        /// Handle zur Datenbank
        /// </summary>
        protected static SqlConnection dbHandle = null;

        /// <summary>
        /// Verbindung zur Datenbank aufbauen
        /// user=root, passwort=root, port=3306, database=M151.
        /// </summary>
        public SqlConnection Connect()
        {
            try
            {
                if (dbHandle == null)
                {
                    string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=M151;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    dbHandle = new SqlConnection(connStr);
                    dbHandle.Open();
                }
                return dbHandle;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Verbindung failed: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Schliesst eine offene Datenbankverbindung
        /// </summary>
        public void Close()
        {
            if (dbHandle != null)
            {
                dbHandle.Close();
                dbHandle = null;
            }
        }
    }
}