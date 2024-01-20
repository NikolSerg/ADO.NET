using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace ShopKindaThing
{
    abstract public class MySql
    {
        public MySqlConnection Connection { get; private set; }
        protected MySqlCommand sql;
        protected MySqlDataAdapter adapter;
        protected bool isOpen;
        public bool IsOpen { get { return isOpen; } }
        public DataTable Dt { get; private set; }
        public DataRowView Drv { get; set; }

        public MySql(string connstring)
        {
            string ConnectionString = connstring;
            Connection = new MySqlConnection(ConnectionString);
            adapter = new MySqlDataAdapter();
            Dt = new DataTable();
            
            sql = Connection.CreateCommand();
            try
            {
                Connection.Open();
                isOpen = true;
            }
            catch (Exception e)
            {
                WriteException(e);
                MessageBox.Show("Change ConnectionString.txt", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isOpen = false;
            }
        }




        
        public bool IsExist(string eMail, string tableName)
        {
            try
            {
                sql.CommandText = $"Select * from {tableName} where email = '{eMail}'";
                string str = null;
                using (MySqlDataReader reader = sql.ExecuteReader())
                {
                    while (reader.Read()) str += reader.GetString(0);
                    if (str != null) return true;
                }
            }
            catch (Exception e) { WriteException(e); return true; }
            return false;
        }

        public bool Update()
        {
            try
            {
                adapter.Update(Dt);
                return true;
            }
            catch (Exception e)
            {
                WriteException(e);
                return false;
            }
        }

        protected void WriteException(Exception e)
        {
            using (FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"\Update Exceptions.txt", FileMode.Append))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(e.Message + "\n");
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }

    public class Customers : MySql
    {
        public static event Action UpdateConnStatus;
        public Task Loader { get; private set; }


        public Customers(string connstring) : base(connstring)
        {
            if (isOpen)
            {
                if (!Exist()) CreateTable();

                Loader = SetAdapter();
                Loader.ContinueWith(delegate { UpdateConnStatus?.Invoke(); });
            }
        }

        bool Exist()
        {
            bool exist = false;
            sql.CommandText = $"show tables like 'Customers'";
            MySqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == "Customers".ToLower()) exist = true;
            }
            reader.Close();
            return exist;
        }

        void CreateTable()
        {
            try
            {
                sql.CommandText = @"Create table Customers 
            (
            id int auto_increment primary key,
            surname nvarchar(20) not null,
            firstName nvarChar(20) not null,
            patronymic nvarchar(20) not null,
            phoneNumber nvarChar(15),
            eMail nvarchar(50) not null
            );";
                sql.ExecuteReader().Close();

            }
            catch (Exception e)
            {
                WriteException(e);
            }
        }

        async Task SetAdapter()
        {
            //Select
            string sqlComm = $"Select * from Customers";
            string str = null;
            adapter.SelectCommand = new MySqlCommand(sqlComm, Connection);
            //Insert
            sqlComm = $"Insert Customers (surname, firstName, patronymic, phoneNumber, eMail)" +
                $"values (@surname, @firstName, @patronymic, @phoneNumber, @eMail); set @id = last_insert_id();";

            adapter.InsertCommand = new MySqlCommand(sqlComm, Connection);
            adapter.InsertCommand.Parameters.Add("@id", MySqlDbType.Int32, 5, "id").Direction = ParameterDirection.Output;
            adapter.InsertCommand.Parameters.Add("@surname", MySqlDbType.VarChar, 20, "surname");
            adapter.InsertCommand.Parameters.Add("@firstName", MySqlDbType.VarChar, 20, "firstName");
            adapter.InsertCommand.Parameters.Add("@patronymic", MySqlDbType.VarChar, 20, "patronymic");
            adapter.InsertCommand.Parameters.Add("@phoneNumber", MySqlDbType.VarChar, 15, "phoneNumber");
            adapter.InsertCommand.Parameters.Add("@eMail", MySqlDbType.VarChar, 50, "eMail");

            //Update
            sqlComm = @"Update Customers set surname = @surname, firstName = @firstName, patronymic = @patronymic,
phoneNumber = @phoneNumber, eMail = @eMail where id = @id";
            adapter.UpdateCommand = new MySqlCommand(sqlComm, Connection);
            adapter.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int32, 5, "id").SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand.Parameters.Add("@surname", MySqlDbType.VarChar, 20, "surname");
            adapter.UpdateCommand.Parameters.Add("@firstName", MySqlDbType.VarChar, 20, "firstName");
            adapter.UpdateCommand.Parameters.Add("@patronymic", MySqlDbType.VarChar, 20, "patronymic");
            adapter.UpdateCommand.Parameters.Add("@phoneNumber", MySqlDbType.VarChar, 15, "phoneNumber");
            adapter.UpdateCommand.Parameters.Add("@eMail", MySqlDbType.VarChar, 50, "eMail");

            //Delete
            sqlComm = @"Delete from Customers where id = @id";
            adapter.DeleteCommand = new MySqlCommand(sqlComm, Connection);
            adapter.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int32, 5, "id");
            adapter.DeleteCommand.Parameters.Add("@surname", MySqlDbType.VarChar, 20, "surname");
            adapter.DeleteCommand.Parameters.Add("@firstName", MySqlDbType.VarChar, 20, "firstName");
            adapter.DeleteCommand.Parameters.Add("@patronymic", MySqlDbType.VarChar, 20, "patronymic");
            adapter.DeleteCommand.Parameters.Add("@phoneNumber", MySqlDbType.VarChar, 15, "phoneNumber");
            adapter.DeleteCommand.Parameters.Add("@eMail", MySqlDbType.VarChar, 50, "eMail");


            sql.CommandText = @"Select * from Customers";

            using (MySqlDataReader reader = sql.ExecuteReader())
            {

                while (reader.Read())
                {
                    str = reader.GetString(0);

                }
            }
            if (str == null)
            {
                sql.CommandText = $"Insert Customers (surname, firstName, patronymic, phoneNumber, eMail) values " +
                $"('First-Surname', 'First-Name', 'First-Patronymic', '89000000000', 'firstmail@mail.ru'), " +
                $"('Second-Surname', 'Second-Name', 'Second-Patronymic', null, 'secondmail@mail.ru'), " +
                $"('Third-Surname', 'Third-Name', 'Third-Patronymic', 89000000002, 'thirdmail@mail.ru'), " +
                $"('Fourth-Surname', 'Fourth-Name', 'Fourth-Patronymic', 89000000003, 'fourthmail@mail.ru'), " +
                $"('Fifth-Surname', 'Fifth-Name', 'Fifth-Patronymic', null, 'fifthmail@mail.ru')";
            }

            try { sql.ExecuteReader().Close(); }
            catch(Exception e) { WriteException(e); }

            await adapter.FillAsync(Dt);
        }
    }

    public class Orders : MySql
    {
        public static event Action UpdateConnStatus;
        public Task Loader { get; private set; }
        public Orders(string connstring) : base(connstring)
        {
            if (isOpen)
            {
                if (!Exist()) CreateTable();

                Loader = SetAdapter();
                Loader.ContinueWith(delegate { UpdateConnStatus?.Invoke(); });
            }
        }

        bool Exist()
        {
            bool exist = false;
            sql.CommandText = $"show tables like 'Orders'";
            MySqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == "Orders".ToLower()) exist = true;
            }
            reader.Close();
            return exist;
        }

        void CreateTable()
        {
            try
            {
                sql.CommandText = @"Create table Orders
            (
            id int auto_increment primary key,
            eMail nvarchar(50) not null,
            gCode int not null,
            gName nvarchar(50) not null
            );";
                sql.ExecuteReader().Close();

            }
            catch (Exception e)
            {
                WriteException(e);
            }

        }

        async Task SetAdapter()
        {
            //Select
            string sqlComm = $"Select * from Orders";
            string str = null;
            adapter.SelectCommand = new MySqlCommand(sqlComm, Connection);

            //Insert
            sqlComm = @"Insert Orders (eMail, gCode, gName) values (@eMail, @gCode, @gName); set @id = last_insert_id()";
            adapter.InsertCommand = new MySqlCommand(sqlComm, Connection);
            adapter.InsertCommand.Parameters.Add("@id", MySqlDbType.Int32, 5, "id").Direction = ParameterDirection.Output;
            adapter.InsertCommand.Parameters.Add("@eMail", MySqlDbType.VarChar, 50, "eMail");
            adapter.InsertCommand.Parameters.Add("@gCode", MySqlDbType.UInt32, 6, "gCode");
            adapter.InsertCommand.Parameters.Add("@gName", MySqlDbType.VarChar, 50, "gName");

            //Update
            sqlComm = @"Update Orders set eMail = @eMail, gCode = @gCode, gName = @gName";
            adapter.UpdateCommand = new MySqlCommand(sqlComm, Connection);
            adapter.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int32, 5, "id").SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand.Parameters.Add("@eMail", MySqlDbType.VarChar, 50, "eMail");
            adapter.UpdateCommand.Parameters.Add("@gCode", MySqlDbType.UInt32, 6, "gCode");
            adapter.UpdateCommand.Parameters.Add("@gName", MySqlDbType.VarChar, 50, "gName");

            //Delete
            sqlComm = @"Delete from Orders where id = @id";
            adapter.DeleteCommand = new MySqlCommand(sqlComm, Connection);
            adapter.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int32, 5, "id");
            adapter.DeleteCommand.Parameters.Add("@eMail", MySqlDbType.VarChar, 50, "eMail");
            adapter.DeleteCommand.Parameters.Add("@gCode", MySqlDbType.UInt32, 6, "gCode");
            adapter.DeleteCommand.Parameters.Add("@gName", MySqlDbType.VarChar, 50, "gName");



            sql.CommandText = @"Select * from Orders";
            str = null;

            using (MySqlDataReader reader = sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    str = reader.GetString(0);

                }
            }
            if (str == null)
            {
                sql.CommandText = "Insert Orders (eMail, gCode, gName) values " +
                    "('firstmail@mail.ru', 1, 'gName1'), " +
                    "('firstmail@mail.ru', 2, 'gName2'), " +
                    "('firstmail@mail.ru', 3, 'gName3'), " +
                    "('firstmail@mail.ru', 4, 'gName4'), " +
                    "('firstmail@mail.ru', 5, 'gName5'), " +
                    "('secondmail@mail.ru', 1, 'gName1'), " +
                    "('secondmail@mail.ru', 3, 'gName3'), " +
                    "('secondmail@mail.ru', 10, 'gName10'), " +
                    "('thirdmail@mail.ru', 1, 'gName1'), " +
                    "('thirdmail@mail.ru', 5, 'gName5'), " +
                    "('fourthmail@mail.ru', 22, 'gName22'), " +
                    "('fourthmail@mail.ru', 1313, 'gName1313'), " +
                    "('fourthmail@mail.ru', 3, 'gName3'), " +
                    "('fifthmail@mail.ru', 7, 'gName7'), " +
                    "('fifthmail@mail.ru', 2, 'gName2'), " +
                    "('fifthmail@mail.ru', 10, 'gName10');";


                try { sql.ExecuteReader().Close(); }
                catch (Exception e) { WriteException(e); }
            }

            await adapter.FillAsync(Dt);
        }
    }


}
