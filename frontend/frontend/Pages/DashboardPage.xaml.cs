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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace frontend
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            WelcomeLabel.Content = "Welcome, " + ((MainWindow)Application.Current.MainWindow).Username +"!";
            BalanceLabel.Content = ((MainWindow)Application.Current.MainWindow).ClientBalance.ToString();
            ExpenseLabel.Content = ((MainWindow)Application.Current.MainWindow).ClientExpense.ToString();
            UserIdLabel.Content = "UserID: "+((MainWindow)Application.Current.MainWindow).UserId.ToString();
        }

        private void NavigateToEditBalance(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Navigate("EditPemasukan");
        }

        private void NavigateToEditExpense(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Navigate("EditPengeluaran");
        }

        private void ConfirmDeletePopUp(object sender, RoutedEventArgs e)
        {
            ConfirmDelete popupWindow = new ConfirmDelete();
            popupWindow.Show();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Logout();
            ((MainWindow)Application.Current.MainWindow).Navigate("LandingPage");
        }
    }
}
