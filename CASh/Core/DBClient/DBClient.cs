using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace CASh.Core.DBClient
{
    public enum DatabaseInterface
    {
        SQLite
    }

    public class DBClient
    {
        private string _connectionString;
        protected object? _connection;

        public DatabaseInterface DatabaseInterface { get; }

        public DBClient(DatabaseInterface databaseInterface, string connectionString)
        {
            DatabaseInterface = databaseInterface;
            _connectionString = connectionString;

            switch (DatabaseInterface)
            {
                case DatabaseInterface.SQLite:
                    _connection = InitSQLite();
                    break;
                default:
                    break;
            }
        }

        private SqliteConnection? InitSQLite()
        {
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);

            dbConnection.Open();

            if (dbConnection.State != System.Data.ConnectionState.Open)
            {
                dbConnection.Close();
                return null;
            }

            dbConnection.Close();

            return dbConnection;
        }

        public List<Dictionary<string, string>>? ExecuteSelectQuery(string query)
        {
            if (_connection == null) return null;

            List<Dictionary<string, string>>? result = null;

            try
            {
                switch (DatabaseInterface)
                {
                    case DatabaseInterface.SQLite:
                        result = SQLiteExecuteSelectQuery(query);
                        break;
                    default:
                        break;
                }
            } 
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public int ExecuteQuery(string query)
        {
            if (_connection == null) return -1;

            int result = -1;

            try
            {
                switch (DatabaseInterface)
                {
                    case DatabaseInterface.SQLite:
                        result = SQLiteExecuteQuery(query);
                        break;
                    default:
                        break;
                }
            } 
            catch (Exception ex)
            {
                return -1;
            }

            return result;
        }

        public object? ExecuteQueryScalar(string query)
        {
            if (_connection == null) return null;

            object? result = null;

            try
            {
                switch (DatabaseInterface)
                {
                    case DatabaseInterface.SQLite:
                        result = SQLiteExecuteScalar(query);
                        break;
                    default:
                        break;
                }
            } 
            catch(Exception ex)
            {
                return null;
            }

            return result;
        }

        private List<Dictionary<string, string>>? SQLiteExecuteSelectQuery(string query)
        {
            if (DatabaseInterface != DatabaseInterface.SQLite) return null;

            SqliteConnection? connection = _connection as SqliteConnection;
            List<Dictionary<string, string>>? objs = null;

            connection?.Open();

            if (connection?.State == System.Data.ConnectionState.Open)
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                using (SqliteDataReader dbDataReader = command.ExecuteReader())
                {
                    objs = new List<Dictionary<string, string>>();

                    while (dbDataReader.Read())
                    {
                        Dictionary<string, string> row = new Dictionary<string, string>();

                        for (int i = 0; i < dbDataReader.FieldCount; i++)
                        {
                            row.Add(dbDataReader.GetName(i), new String(dbDataReader[i].ToString()));
                        }
                        objs.Add(row);
                    }
                }
            }

            connection?.Close();

            return objs;
        }

        private int SQLiteExecuteQuery(string query)
        {
            if (DatabaseInterface != DatabaseInterface.SQLite) return -1;

            SqliteConnection? connection = _connection as SqliteConnection;
            int res = -1;

            connection?.Open();

            if (connection?.State == System.Data.ConnectionState.Open)
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                res = command.ExecuteNonQuery();
            }

            connection?.Close();

            return res;
        }

        private object? SQLiteExecuteScalar(string query)
        {
            if (DatabaseInterface != DatabaseInterface.SQLite) return null;

            SqliteConnection? connection = _connection as SqliteConnection;
            object? res = null;

            connection?.Open();

            if (connection?.State == System.Data.ConnectionState.Open)
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                res = command.ExecuteScalar();
            }

            connection?.Close();

            return res;
        }
    }
}
