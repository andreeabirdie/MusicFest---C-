using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace MusicFest.repository
{
    public class DBUtils
    {
        private static SQLiteConnection instance = null;
        public static SQLiteConnection getConnection()
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                instance = getNewConnection();
                instance.Open();
            }
            return instance;
        }

        private static SQLiteConnection getNewConnection()
        {
            string connectionString = null;
            ConnectionStringSettings settings =ConfigurationManager.ConnectionStrings["festivalDB"];
            if (settings != null)
                connectionString = settings.ConnectionString;
            return new SQLiteConnection(connectionString);
        }

        
        public static void closeConnection()
        {
            if (instance != null && instance.State != ConnectionState.Closed)
            {
                instance.Close();
            }
        }
    
    }
}