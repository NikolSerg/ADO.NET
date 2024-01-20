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
    /// Логика взаимодействия для OrdersTable.xaml
    /// </summary>
    public partial class OrdersTable : Window
    {
        public OrdersTable(string eMail)
        {
            
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SQLProvider.FillDataGrid(ordersDataGrid, eMail);
        }
    }
}
