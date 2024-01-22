using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShopKindaThing
{
    public static class SQLProvider
    {
        static Customers Customers;
        static Orders Orders;

        static Thread firstThread;
        static Thread secondThread;

        public static void Start(string connstring1, string connstring2)
        {
            firstThread = new Thread(() => { Customers = new Customers(connstring1); });
            firstThread.Start();
            secondThread = new Thread(() => { Orders = new Orders(connstring2); });
            secondThread.Start();
            //Customers = new MySql("localhost", "localdb", "user", "user", 0);
            //Orders = new MySql("localhost", "local", "user", "user", 1);
        }

        public static void AddCustomer(string[] customer)
        {
            if (!Customers.IsExist(customer[4], "customers"))
            {
                DataRow row = Customers.Dt.NewRow();
                row[1] = customer[0];
                row[2] = customer[1];
                row[3] = customer[2];
                row[4] = customer[3];
                row[5] = customer[4];
                Customers.Dt.Rows.Add(row);
                Customers.Update();
            }
            else MessageBox.Show("Customer is already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void AddOrder(string[] order)
        {
            DataRow row = Orders.Dt.NewRow();
            row[1] = order[0];
            row[2] = Convert.ToInt32(order[1]);
            row[3] = order[2];
            Orders.Dt.Rows.Add(row);
            Orders.Update();
        }

        public static bool CustomersIsOpen()
        {
            return Customers != null && Customers.IsOpen;
        }

        public static bool OrdersIsOpen()
        {
            return Orders != null && Orders.IsOpen;
        }
        public static string GetConnectionStatus()
        {
            if (Orders != null && Customers != null)
            {
                string connStatus = "";
                connStatus += $"CustomersDB Connection String: {Customers.Connection.ConnectionString}\n" +
                    $"CustomersDB Connection Status: {Customers.Connection.State.ToString()}\n\n" +
                    $"OrdersDB Connection String: {Orders.Connection.ConnectionString}\n" +
                    $"OrdersDB Connection Status: {Orders.Connection.State.ToString()}";
                return connStatus;
            }
            else return String.Empty;
        }

        public static async void FillDataGrid(DataGrid grid)
        {
            await grid.Dispatcher.InvokeAsync(() =>
            {
                if (Customers != null && Orders != null)
                {
                    switch (grid.Name)
                    {
                        case "customersDataGrid":
                            if (Customers.Loader.IsCompleted)
                            {

                                grid.DataContext = Customers.Dt.DefaultView;

                            }
                            else
                            {
                                Customers.Loader.Wait();
                                grid.DataContext = Customers.Dt.DefaultView;
                            }
                            break;
                        case "ordersDataGrid":
                                grid.DataContext = Orders.Dt.DefaultView;
                            break;
                        default:
                            MessageBox.Show("Table doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            });

        }

        public static async void FillDataGrid(DataGrid grid, string eMail)
        {
            await grid.Dispatcher.InvokeAsync(() =>
            {
                Orders.SetAdapter(eMail);
                grid.DataContext = Orders.Dt.DefaultView;
            });
        }

        static DataTable GetDataView(string eMail)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("id"));
            data.Columns.Add(new DataColumn("eMail"));
            data.Columns.Add(new DataColumn("gCode"));
            data.Columns.Add(new DataColumn("gName"));
            foreach (DataRow row in Orders.Dt.Rows)
            {
                if (row[1].ToString() == eMail)
                {
                    data.Rows.Add(row.ItemArray);
                }
                
            }
            return data;
        }

        public static void CustomersBeginEdit(DataRowView row)
        {
            Customers.Drv = row;
            Customers.Drv.BeginEdit();
        }

        public static bool CustomersEndEdit()
        {
            if (Customers.Drv != null)
                Customers.Drv.EndEdit();

            return Customers.Update();
        }

        public static void CustomersDeleteRow(DataRowView row)
        {
            row.Row.Delete();
            Customers.Update();
        }

        public static void OrdersDeleteRow(DataRowView row)
        {
            row.Row.Delete();
            Orders.Update();
        }
        
    }
}
