using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CreatePatientPage.xaml
    /// </summary>
    public partial class CreatePatientPage : Page
    {
        private ObservableCollection<Patient> _patients;
        public Patient CurrentPatient { get; } = new Patient();

        public CreatePatientPage(ObservableCollection<Patient> patients)
        {
            InitializeComponent();
            _patients = patients;
            DataContext = this;
            CurrentPatient.AppointmentStories = new ObservableCollection<Appointment>();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentPatient.Name) ||
                string.IsNullOrWhiteSpace(CurrentPatient.LastName) ||
                string.IsNullOrWhiteSpace(CurrentPatient.MiddleName) ||
                string.IsNullOrWhiteSpace(CurrentPatient.Birthday) ||
                string.IsNullOrWhiteSpace(CurrentPatient.PhoneNumber))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            Random random = new Random();
            int patientId;
            bool idExists;

            do
            {
                patientId = random.Next(1000000, 10000000);
                idExists = File.Exists($"P_{patientId}.json");
            } while (idExists);

            CurrentPatient.Id = patientId;


            string fileName = $"P_{CurrentPatient.Id}.json";
            string json = JsonSerializer.Serialize(CurrentPatient, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(fileName, json);
            _patients.Add(CurrentPatient);

            MessageBox.Show($"Пациент создан! ID: {CurrentPatient.Id}");
            NavigationService.GoBack();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BirthdayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayDatePicker.SelectedDate.HasValue)
            {
                CurrentPatient.Birthday = BirthdayDatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");
            }
        }
    }
}
