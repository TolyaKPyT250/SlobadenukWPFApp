using projectGamesPlatformers.Models;
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
    /// Логика взаимодействия для GameDetailsWindow.xaml
    /// </summary>
    public partial class GameDetailsWindow : Window
    {
        public GameDetailsWindow(Game game)
        {
            InitializeComponent();

            // Заполняем данные из переданного объекта
            tbTitle.Text = game.Title;
            tbDeveloper.Text = game.Developer?.Name ?? "Не указан";
            tbPublisher.Text = game.Publisher?.Name ?? "Не указан";
            tbPlatform.Text = game.Platform?.Name ?? "Не указана";
            tbMechanics.Text = game.Mechanics;
            tbProtagonist.Text = game.Protagonist;
            tbDescription.Text = string.IsNullOrEmpty(game.Description) ? "Описание отсутствует." : game.Description;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
