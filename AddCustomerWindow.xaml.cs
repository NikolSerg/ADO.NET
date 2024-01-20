using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            this.ResizeMode = ResizeMode.NoResize;
            isValid = new bool[] { false, false, false, false, false };
            customer = new string[5];
        }

        bool[] isValid;

        string[] customer;

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Target.Focus();
        }

        private void NamesField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Validator.ValidateNames((sender as TextBox).Text))
            {
                (sender as TextBox).BorderBrush = Brushes.Red;
            }
            else
            {
                (sender as TextBox).BorderBrush = Brushes.Gray;
                switch ((sender as TextBox).Tag.ToString())
                {
                    case "Surname":
                        isValid[0] = true;
                        customer[0] = (sender as TextBox).Text;
                        break;
                    case "First Name":
                        isValid[1] = true;
                        customer[1] = (sender as TextBox).Text;
                        break;
                    case "Patronymic":
                        isValid[2] = true;
                        customer[2] = (sender as TextBox).Text;
                        break;
                }
            }
        }

        private void phoneField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Validator.ValidatePhoneNumber((sender as TextBox).Text))
            {
                (sender as TextBox).BorderBrush = Brushes.Red;
            }
            else
            {
                (sender as TextBox).BorderBrush = Brushes.Gray;
                isValid[3] = true;
                customer[3] = (sender as TextBox).Text;
            }
        }

        private void eMailField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Validator.ValidateEMail((sender as TextBox).Text))
            {
                (sender as TextBox).BorderBrush = Brushes.Red;
            }
            else
            {
                (sender as TextBox).BorderBrush = Brushes.Gray;
                isValid[4] = true;
                customer[4] = (sender as TextBox).Text;
            }
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (isValid.All(p => p == true))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect customer data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string[] GetCustomerStringArray()
        {
            return customer;
        }
    }
}
