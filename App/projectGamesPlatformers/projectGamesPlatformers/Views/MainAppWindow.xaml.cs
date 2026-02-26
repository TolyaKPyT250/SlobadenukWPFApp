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
using System.Data.Entity;

namespace projectGamesPlatformers.Views
{
    /// <summary>
    /// Логика взаимодействия для MainAppWindow.xaml
    /// </summary>
    public partial class MainAppWindow : Window
    {
        user15Entities db = new user15Entities();

        public MainAppWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // метод загрузки данных в таблицу и выпадающие списки
        private void LoadData()
        {
            // загружаем игры вместе со связанными данными разработчиков и издателей
            dgGames.ItemsSource = db.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Platform)
                .ToList();

            // Заполняем ComboBox-ы данными из соответствующих таблиц
            cmbDeveloper.ItemsSource = db.Developers.ToList();
            cmbPublisher.ItemsSource = db.Publishers.ToList();
            cmbPlatform.ItemsSource = db.Platforms.ToList();
        }

        // фильтрация (поиск по назв игры)
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtSearch.Text.ToLower();
            dgGames.ItemsSource = db.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Platform)
                .Where(g => g.Title.ToLower().Contains(search) || g.Mechanics.ToLower().Contains(search))
                .ToList();
        }

        // добавление новой игры
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inpTitle.Text)) return;

                Game newGame = new Game
                {
                    Title = inpTitle.Text,
                    Description = inpDescription.Text,
                    Mechanics = inpMechanics.Text,
                    Protagonist = inpProtagonist.Text,
                    DeveloperID = (int)cmbDeveloper.SelectedValue,
                    PublisherID = (int)cmbPublisher.SelectedValue,
                    PlatformID = (int)cmbPlatform.SelectedValue
                };

                db.Games.Add(newGame);
                db.SaveChanges();
                LoadData(); // обновляем таблицу
                MessageBox.Show("Игра добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении. Проверьте заполнение всех полей.");
            }
        }

        // Удаление выбранной игры
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgGames.SelectedItem is Game selectedGame)
            {
                var result = MessageBox.Show($"Удалить игру {selectedGame.Title}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    db.Games.Remove(selectedGame);
                    db.SaveChanges();
                    LoadData();
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            LoadData();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            inpTitle.Clear();
            inpMechanics.Clear();
            inpProtagonist.Clear();
        }

        // при выборе строки в таблице можно подтягивать данные в поля (необязательно)
        private void DgGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGames.SelectedItem is Game g)
            {
                inpTitle.Text = g.Title;
                inpMechanics.Text = g.Mechanics;
                inpProtagonist.Text = g.Protagonist;
                inpDescription.Text = g.Description;

                cmbDeveloper.SelectedValue = g.DeveloperID;
                cmbPublisher.SelectedValue = g.PublisherID;
                cmbPlatform.SelectedValue = g.PlatformID;
            }
        }

        // добавление
        private void BtnAddDev_Click(object sender, RoutedEventArgs e)
        {
            var win = new Views.AllEditLookupWindow("разработчика");
            if (win.ShowDialog() == true)
            {
                var newDev = new Developer { Name = win.EnteredName };
                db.Developers.Add(newDev);
                db.SaveChanges();
                RefreshCombos(); // обновка
            }
        }

        // удаление
        private void BtnDelDev_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDeveloper.SelectedItem is Developer selectedDev)
            {
                var res = MessageBox.Show($"Удалить {selectedDev.Name}? (Это может удалить связанные игры!)",
                                          "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    db.Developers.Remove(selectedDev);
                    db.SaveChanges();
                    RefreshCombos();
                }
            }
        }

        private void RefreshCombos()
        {
            cmbDeveloper.ItemsSource = db.Developers.ToList();
            cmbPublisher.ItemsSource = db.Publishers.ToList();
            cmbPlatform.ItemsSource = db.Platforms.ToList();
        }
        private void BtnAddPub_Click(object sender, RoutedEventArgs e)
        {
            var win = new projectGamesPlatformers.Views.AllEditLookupWindow("издателя");
            if (win.ShowDialog() == true)
            {
                var newPub = new Publisher { Name = win.EnteredName };
                db.Publishers.Add(newPub);
                db.SaveChanges();
                RefreshCombos(); // обновляем список
            }
        }

        private void BtnDelPub_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPublisher.SelectedItem is Publisher selectedPub)
            {
                var res = MessageBox.Show($"Удалить издателя {selectedPub.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Publishers.Remove(selectedPub);
                        db.SaveChanges();
                        RefreshCombos();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Нельзя удалить издателя, так как с ним связаны игры!");
                    }
                }
            }
        }

        // обработка платформ

        private void BtnAddPlat_Click(object sender, RoutedEventArgs e)
        {
            var win = new projectGamesPlatformers.Views.AllEditLookupWindow("платформу");
            if (win.ShowDialog() == true)
            {
                var newPlat = new Platform { Name = win.EnteredName };
                db.Platforms.Add(newPlat);
                db.SaveChanges();
                RefreshCombos();
            }
        }

        private void BtnDelPlat_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPlatform.SelectedItem is Platform selectedPlat)
            {
                var res = MessageBox.Show($"Удалить платформу {selectedPlat.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Platforms.Remove(selectedPlat);
                        db.SaveChanges();
                        RefreshCombos();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Эта платформа используется в записях об играх!");
                    }
                }
            }
        }
        private void DgGames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgGames.SelectedItem is Game selectedGame)
            {
                // Создаем окно и передаем в него объект игры
                var detailsWin = new GameDetailsWindow(selectedGame);
                detailsWin.ShowDialog();
            }
        }
    }
}
