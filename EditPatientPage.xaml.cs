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
    /// Логика взаимодействия для EditPatientPage.xaml
    /// </summary>
    public partial class EditPatientPage : Page
    {
        private ObservableCollection<Patient> _patients;
        private Patient _originalPatient;
        public Patient CurrentPatient { get; }

        public EditPatientPage(ObservableCollection<Patient> patients, Patient patient)
        {
            InitializeComponent();
            _patients = patients;
            _originalPatient = patient;

            CurrentPatient = new Patient
            {
                Id = patient.Id,
                Name = patient.Name,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                Birthday = patient.Birthday,
                PhoneNumber = patient.PhoneNumber,
                AppointmentStories = patient.AppointmentStories
            };

            DataContext = this;

            if (!string.IsNullOrEmpty(CurrentPatient.Birthday) &&
                DateTime.TryParse(CurrentPatient.Birthday, out DateTime birthday))
            {
                BirthdayDatePicker.SelectedDate = birthday;
            }
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

            _originalPatient.Name = CurrentPatient.Name;
            _originalPatient.LastName = CurrentPatient.LastName;
            _originalPatient.MiddleName = CurrentPatient.MiddleName;
            _originalPatient.Birthday = CurrentPatient.Birthday;
            _originalPatient.PhoneNumber = CurrentPatient.PhoneNumber;
            string fileName = $"P_{_originalPatient.Id}.json";
            string json = JsonSerializer.Serialize(_originalPatient, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(fileName, json);

            MessageBox.Show("Информация о пациенте обновлена");
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
