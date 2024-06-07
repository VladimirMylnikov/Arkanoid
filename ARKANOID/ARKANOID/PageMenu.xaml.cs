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

namespace ARKANOID
{
    /// <summary>
    /// Логика взаимодействия для PAGE_MENU.xaml
    /// </summary>
    public partial class PAGE_MENU : Page
    {
        public PAGE_MENU()
        {
            InitializeComponent();
        }

        private void START_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GAME());
        }

        private void EXIT_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
