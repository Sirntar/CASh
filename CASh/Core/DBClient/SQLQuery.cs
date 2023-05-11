using System.Collections.Generic;

namespace CASh.Core.DBClient
{
    public struct Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class SQLQuery : DBClient
    {
        string? _query = null;
        string? _condition = null;
        string? _limit = null;

        public string? Query
        {
            get { return _query; }
        }

        public string? Condition
        {
            get { return _condition; }
        }

        public SQLQuery(DatabaseInterface databaseInterface, string connectionString)
            : base(databaseInterface, connectionString) {}

        private void addCols(string[]? cols = null)
        {
            if (cols != null)
            {
                _query += "(";
                for (int i = 0; i < cols.Length; i++)
                    _query += '`' + cols[i] + '`' + (i != cols.Length - 1 ? ',' : ") ");
            }
        }

        private void addConditionAndLimit()
        {
            if(_condition != null && _condition != null)
            {
                _query += " " + _condition + " ";

                if (_limit != null)
                {
                    _query += _limit + " ";
                }
            }

            _condition = null;
            _limit = null;
        }

        public SQLQuery Update(string table, string vals)
        {
            _query += "UPDATE " + table + " SET " + vals + " ";

            return this;
        }

        public SQLQuery Delete(string fromTable)
        {
            _query += "DELETE FROM " + fromTable + " ";
            return this;
        }

        public SQLQuery Where(string col, string op, string value)
        {
            if (op[0] == '>' || op[0] == '<' || op[0] == '=' || op == "like" || op == "LIKE")
            {
                if(op.Length > 1)
                    if(op[1] != '=') 
                        return this;

                _condition = "WHERE " + col + ' ' + op + ' ' + value + ' ';
            }
            return this;
        }

        public void CreateTable(string tableName, Column[] cols)
        {
            _limit = null;
            _condition = null;
            _query = "CREATE TABLE IF NOT EXISTS `" + tableName + "` (";

            for (int i = 0; i < cols.Length; i++)
                _query += cols[i].Name + " " + cols[i].Type + (i == cols.Length - 1 ? ")" : ",");
        }

        public void Insert(string intoTable, string[] cols, string[] insert)
        {
            _limit = null;
            _condition = null;
            _query = "INSERT INTO " + intoTable + " (";

            for (int i = 0; i < cols.Length; i++)
                _query += "" + cols[i] + "" + (i == cols.Length - 1 ? ")" : ",");

            _query += " VALUES (";

            for (int i = 0; i < insert.Length; i++)
                _query += "'" + insert[i] + "'" + (i == insert.Length - 1 ? ")" : ",");
        }

        public SQLQuery Limit(int limit)
        {
            _limit = "Limit=" + limit.ToString();
            return this;
        }

        public SQLQuery Select(string what, string fromTable)
        {
            if (_query != null)
            {
                addConditionAndLimit();
                _query += "; " + "SELECT " + what + " FROM `" + fromTable + "` ";
            } else
                _query = "SELECT " + what + " FROM `" + fromTable + "` ";
            return this;
        }

        public void RawSQL(string sql)
        {
            _query = sql;
        }

        public List<Dictionary<string, string>>? Fetch()
        {
            if(_query == null) return null;

            addConditionAndLimit();
            _query += ";";

            List<Dictionary<string, string>>? ret = base.ExecuteSelectQuery(_query);

            _query = null;
            _condition = null;

            return ret;
        }

        public int Execute()
        {
            if (_query == null) return -1;

            addConditionAndLimit();
            _query += ";";

            int ret = base.ExecuteQuery(_query);

            _query = null;
            _condition = null;

            return ret;
        }

        public object? ExecuteAsScalar()
        {
            if (_query == null) return null;

            addConditionAndLimit();
            _query += ";";

            object? ret = base.ExecuteQueryScalar(_query);

            _query = null;
            _condition = null;

            return ret;
        }
    }
}
