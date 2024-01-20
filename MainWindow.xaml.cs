using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace ShopKindaThing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Customers.UpdateConnStatus += MySql_UpdateConnStatus;
            Orders.UpdateConnStatus += MySql_UpdateConnStatus;



            string[] connstring = Properties.Resources.ConnectionString.Split('\n');
            connstring[0] = connstring[0].Remove(connstring[0].Count() - 1);
            SQLProvider.Start(connstring[0], connstring[1]);

            if (!SQLProvider.CustomersIsOpen()) addCustomer.IsEnabled = false;
        }

        private void MySql_UpdateConnStatus()
        {
            if (SQLProvider.CustomersIsOpen()) addCustomer.Dispatcher.Invoke(() => addCustomer.IsEnabled = true);
            connectionTextBox.Dispatcher.Invoke(() => { connectionTextBox.Text = SQLProvider.GetConnectionStatus(); });
            SQLProvider.FillDataGrid(customersDataGrid);
        }

        private void customersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView row = (DataRowView)customersDataGrid.SelectedItem;
            currentRow = row;

            backupRow = new object[] { row.Row.ItemArray[0], row.Row.ItemArray[1], row.Row.ItemArray[2],
            row.Row.ItemArray[3], row.Row.ItemArray[4], row.Row.ItemArray[5]};
            SQLProvider.CustomersBeginEdit(row);

            isEditing = true;
        }

        DataRowView currentRow;
        object[] backupRow;
        bool isEditing;

        private bool Validate(DataRowView row)
        {
            bool valid = true;
            if (row != null)
            {
                for (int i = 0; i < row.Row.ItemArray.Length; i++)
                {
                    if (i == 1 || i == 2 || i == 3)
                    {
                        if (!Validator.ValidateNames(row.Row.ItemArray[i].ToString()))
                            valid = false;
                    }
                    else if (i == 4)
                    {
                        if (!Validator.ValidatePhoneNumber(row.Row.ItemArray[i].ToString()))
                            valid = false;
                    }
                    else if (i == 5)
                    {
                        if (!Validator.ValidateEMail(row.Row.ItemArray[i].ToString()))
                            valid = false;
                    }
                }
            }
            return valid;
        }



        private void customersDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (isEditing)
            {
                if (!Validate(currentRow) || !SQLProvider.CustomersEndEdit() )
                {
                    MessageBox.Show("Incorrect input, all changes will be canceled", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    currentRow.Row.ItemArray = backupRow;
                    SQLProvider.CustomersEndEdit();
                }
                isEditing = false;
            }
        }

        private void deleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            SQLProvider.CustomersDeleteRow(drv);
            deleteCustomer.IsEnabled = false;
            showOrdersButton.IsEnabled = false;
        }

        DataRowView drv;
        private void customersDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                drv = (DataRowView)customersDataGrid.CurrentItem;
                deleteCustomer.IsEnabled = true;
                showOrdersButton.IsEnabled = true;
            }
            catch
            {
                return;
            }
        }

        private void addCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow window = new AddCustomerWindow();
            window.Topmost = true;
            
            if (window.ShowDialog() == true)
                SQLProvider.AddCustomer(window.GetCustomerStringArray());
        }

        private void showOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersTable orders = new OrdersTable(drv[5].ToString());
            orders.Show();
        }
    }
}
