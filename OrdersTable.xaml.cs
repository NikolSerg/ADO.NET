using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace ShopKindaThing
{
    /// <summary>
    /// Логика взаимодействия для OrdersTable.xaml
    /// </summary>
    public partial class OrdersTable : Window
    {
        public OrdersTable(string eMail)
        {
            
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SQLProvider.FillDataGrid(ordersDataGrid, eMail);
            this.eMail = eMail;
        }

        string eMail;

        private void addOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Topmost = true;
            window.ResizeMode = ResizeMode.NoResize;
            window.Width = 200;
            window.Height = 250;
            window.Title = "Add Order";

            Grid grid = new Grid();
            

            StackPanel sp = new StackPanel();
            sp.VerticalAlignment = VerticalAlignment.Top;


            Label codeLabel = new Label();
            codeLabel.Content = "Product Code";
            TextBox codeField = new TextBox();
            codeField.Margin = new Thickness(0, 0, 0, 20);
            codeLabel.Target = codeField;
            codeLabel.MouseLeftButtonDown += Label_MouseLeftButtonDown;
            codeField.LostFocus += CodeField_LostFocus;

            Label nameLabel = new Label();
            nameLabel.Content = "Product Name";
            TextBox nameField = new TextBox();
            nameLabel.Target = nameField;
            nameLabel.MouseLeftButtonDown += Label_MouseLeftButtonDown;

            Button acceptBtn = new Button();
            Button cancelBtn = new Button();
            acceptBtn.Content = "Add new order";
            cancelBtn.Content = "Cancel";
            acceptBtn.Height = cancelBtn.Height = 20;
            acceptBtn.Width = cancelBtn.Width = 150;
            acceptBtn.Margin = cancelBtn.Margin = new Thickness(0, 20, 0, 0);
            cancelBtn.IsCancel = true;
            acceptBtn.Click += new RoutedEventHandler((a, b) => AcceptBtn_Click(a, b, window, codeField.Text, nameField.Text));

            sp.Children.Add(codeLabel);
            sp.Children.Add(codeField);

            sp.Children.Add(nameLabel);
            sp.Children.Add(nameField);

            sp.Children.Add(acceptBtn);
            sp.Children.Add(cancelBtn);

            grid.Children.Add(sp);

            window.Content = grid;

            codeField.Width = 180;
            nameField.Width = 180;

            window.ShowDialog();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e, Window window, string code, string name)
        {
            if (valid)
            {
                SQLProvider.AddOrder(new string[] { eMail, code, name });
                window.Close();
            }
            else
            {
                MessageBox.Show("Invalid input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CodeField_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32((sender as TextBox).Text);
                (sender as TextBox).BorderBrush = Brushes.Gray;
                valid = true;
            }
            catch
            {
                (sender as TextBox).BorderBrush = Brushes.Red;
                valid = false;
            }
        }

        bool valid;

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Target.Focus();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            SQLProvider.OrdersDeleteRow(drv);
            deleteButton.IsEnabled = false;
        }

        private void ordersDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                drv = (DataRowView)ordersDataGrid.CurrentItem;
                deleteButton.IsEnabled = true;
            }
            catch
            {
                return;
            }
        }

        DataRowView drv;

    }
}
