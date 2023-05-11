using System;
using System.Collections.Generic;
using System.Linq;
using CASh.Core.DBClient;

namespace CASh.MVVM.Model
{
    public class CustomerModel : CustomerAbstractModel, ModelInterface
    {
        public event EventHandler<EventArgs>? CustomerDeleted;
        public event EventHandler<EventArgs>? CustomerUpdated;

        protected static bool _hasInit = false;
        private void Init()
        {
            if (!_hasInit)
            {
                _hasInit = true;
                InitModelTables();
            }
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PESEL { get; set; }
        public string? IBAN { get; set; }

        public CustomerModel()
        {
            Init();
        }
        public CustomerModel(CustomerModel obj)
        {
            Init();

            Id = obj.Id;
            Email = obj.Email;
            PhoneNumber = obj.PhoneNumber;
            TotalMoneySpend = obj.TotalMoneySpend;
            OrderCounter = obj.OrderCounter;
            RmaCounter = obj.RmaCounter;
            CreatedAt = obj.CreatedAt;
            UpdatedAt = obj.UpdatedAt;

            FirstName = obj.FirstName;
            LastName = obj.LastName;
            PESEL = obj.PESEL;
            IBAN = obj.IBAN;
        }

        protected override void InitModelTables()
        {
            if (SQLQuery == null)
            {
                _hasInit = false;
                return;
            }

            SQLQuery.Select("Id", "b2c_customers").Limit(1);
            var id = SQLQuery.ExecuteAsScalar();

            if (id == null)
            {
                SQLQuery.CreateTable("b2c_customers", new[]{
                    new Column{ Name = "Id", Type = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT"},
                    new Column{ Name = "Email", Type = "varchar(320) NOT NULL"},
                    new Column{ Name = "PhoneNumber", Type = "varchar(24) NOT NULL"},
                    new Column{ Name = "TotalMoneySpend", Type = "number(8,2) DEFAULT 0"},
                    new Column{ Name = "OrderCounter", Type = "INTEGER DEFAULT 0"},
                    new Column{ Name = "RmaCounter", Type = "INTEGER DEFAULT 0"},
                    new Column{ Name = "UpdatedAt", Type = "datetime DEFAULT CURRENT_TIMESTAMP"},
                    new Column{ Name = "CreatedAt", Type = "datetime DEFAULT CURRENT_TIMESTAMP"},

                    new Column{ Name = "FirstName", Type = "varchar(128) NOT NULL"},
                    new Column{ Name = "LastName", Type = "varchar(128) NOT NULL"},
                    new Column{ Name = "PESEL", Type = "varchar(16)"},
                    new Column{ Name = "IBAN", Type = "varchar(32)"},
                });
                int i = SQLQuery.Execute(); // -1 = error, 0 = ok
            }
        }

        public static void Add(object obj)
        {
            CustomerModel? model = obj as CustomerModel;
            if (model == null) return;

            SQLQuery?.Insert("b2c_customers", new[]
            {
                "Email",
                "PhoneNumber",
                "TotalMoneySpend",
                "OrderCounter",
                "RmaCounter",

                "FirstName",
                "LastName",
                "PESEL",
                "IBAN"
            }, new[]
            {
                new String(model.Email),
                new String(model.PhoneNumber),
                model.TotalMoneySpend.ToString(),
                model.OrderCounter.ToString(),
                model.RmaCounter.ToString(),

                new string(model.FirstName),
                new string(model.LastName),
                new string(model.PESEL),
                new string(model.IBAN)
            });

            int? i = SQLQuery?.Execute(); // 1 = ok, -1 = error, null = not initialized
        }

        public static List<CustomerModel> FetchAll()
        {

            SQLQuery?.Select("Id, Email, PhoneNumber, TotalMoneySpend, OrderCounter, RmaCounter, UpdatedAt, CreatedAt, FirstName, LastName, PESEL, IBAN", "b2c_customers");

            List<Dictionary<string, string>>? rows = SQLQuery?.Fetch();

            if (rows == null)
                return new List<CustomerModel>();

            List<CustomerModel> list = new();

            foreach (Dictionary<string, string> row in rows)
            {
                list.Add(new CustomerModel()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Email = row["Email"],
                    PhoneNumber = row["PhoneNumber"],
                    OrderCounter = Convert.ToInt32(row["OrderCounter"]),
                    RmaCounter = Convert.ToInt32(row["RmaCounter"]),
                    UpdatedAt = Convert.ToDateTime(row["UpdatedAt"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),

                    FirstName = row["FirstName"],
                    LastName = row["LastName"],
                    PESEL = row["PESEL"],
                    IBAN = row["IBAN"],
                });
            }

            return list;
        }

        private void UpdateModel(Dictionary<string, string> changes)
        {
            foreach (var change in changes)
            {
                if (change.Key == "Id")
                    Id = Convert.ToInt32(change.Value);
                else if (change.Key == "email")
                    Email = change.Value;
                else if (change.Key == "PhoneNumber")
                    PhoneNumber = change.Value;
                else if (change.Key == "OrderCounter")
                    OrderCounter = Convert.ToInt32(change.Value);
                else if (change.Key == "RmaCounter")
                    RmaCounter = Convert.ToInt32(change.Value);
                else if (change.Key == "UpdatedAt")
                    UpdatedAt = Convert.ToDateTime(change.Value);
                else if (change.Key == "CreatedAt")
                    CreatedAt = Convert.ToDateTime(change.Value);
                else if (change.Key == "FirstName")
                    FirstName = change.Value;
                else if (change.Key == "LastName")
                    LastName = change.Value;
                else if (change.Key == "PESEL")
                    PESEL = change.Value;
                else if (change.Key == "IBAN")
                    IBAN = change.Value;
            }
        }

        public bool Update(Dictionary<string, string> changes)
        {
            int? res = null;
            int? oldId = Convert.ToInt32(changes["OldId"]);
            changes.Remove("OldId");

            string vals = "";

            for (int i = 0; i < changes.Count; i++)
            {
                // safe string for sql
                vals += '`' + changes.ElementAt(i).Key?.Replace("'", "''").Replace("\"", "\"\"") + "`="
                        + '\'' + changes.ElementAt(i).Value?.Replace("'", "''").Replace("\"", "\"\"") + '\'' + (i != changes.Count - 1 ? ',' : "");
            }

            SQLQuery?.Update("b2c_customers", vals).Where("Id", "=", new String(oldId.ToString()));

            res = SQLQuery?.Execute();

            if (res == -1 || res == null)
                return false;

            UpdateModel(changes);
            CustomerUpdated?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool Delete()
        {
            SQLQuery?.Delete("b2c_customers").Where("Id", "=", this.Id.ToString());

            int? res = SQLQuery?.Execute();

            if (res == -1 || res == null)
                return false;

            CustomerDeleted?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }
}
