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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(MiddleNameTextBox.Text) ||
                SpecComboBox.SelectedItem == null)
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            string specialisation = (SpecComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            Random random = new Random();
            int newId;
            bool idExists;

            do
            {
                newId = random.Next(10000, 100000);
                idExists = File.Exists($"D_{newId}.json");
            } while (idExists);

            var doctor = new Doctor
            {
                Id = newId,
                Name = NameTextBox.Text.Trim(),
                LastName = LastNameTextBox.Text.Trim(),
                MiddleName = MiddleNameTextBox.Text.Trim(),
                Specialisation = specialisation,
                Password = PasswordBox.Password
            };

            string fileName = $"D_{doctor.Id}.json";
            string json = JsonSerializer.Serialize(doctor, new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(fileName, json);

            App.CurrentDoctor = doctor;
            MessageBox.Show($"Регистрация успешна! Ваш ID: {doctor.Id}");

            App.LoadPatients();

            NavigationService.Navigate(new MainUserPage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
