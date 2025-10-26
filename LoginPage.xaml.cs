using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using System.IO;

namespace PacientApp1
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(IDTextBox.Text) || !int.TryParse(IDTextBox.Text, out int userId))
            {
                MessageBox.Show("Введите корректный идентификатор");
                return;
            }

            string fileName = $"D_{userId}.json";
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            string json = File.ReadAllText(fileName);
            var user = JsonSerializer.Deserialize<Doctor>(json);

            if (user.Password != PassBox.Password)
            {
                MessageBox.Show("Неверный пароль");
                return;
            }

            App.CurrentDoctor = user;

            App.LoadPatients();

            NavigationService.Navigate(new MainUserPage());
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}

