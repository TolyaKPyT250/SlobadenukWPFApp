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

namespace projectGamesPlatformers.Views
{
    /// <summary>
    /// Логика взаимодействия для AllEditLookupWindow.xaml
    /// </summary>
    public partial class AllEditLookupWindow : Window
    {
        public string EnteredName { get; private set; }

        public AllEditLookupWindow(string title, string existingValue = "")
        {
            InitializeComponent();
            lblTitle.Text = $"Введите {title}:";
            txtName.Text = existingValue;
            this.Title = title;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Поле не может быть пустым!");
                return;
            }
            EnteredName = txtName.Text;
            DialogResult = true; // Закрывает окно и возвращает успех
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
