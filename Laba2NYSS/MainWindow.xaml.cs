using LABA_2.Controller;
using Laba2NYSS.Controllers;
using Laba2NYSS.Models;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Laba2NYSS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow .xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _dispatcherTimer;
        private int _updatedThreatsCount = 0;
        private Paginator<Threat> _pager;
        private static bool _timerIsActive;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeUIComponents()
        {
            PrevPageButton.Click += (sender, args) =>
            {
                ThreatDataGrid.ItemsSource = _pager.PageBack();
                PaginatorLabel.Content = _pager.CurrentPageId + $"/{_pager.PagesCount}";
            };

            NextPageButton.Click += (sender, args) =>
            {
                ThreatDataGrid.ItemsSource = _pager.PageForward();
                PaginatorLabel.Content = _pager.CurrentPageId + $"/{_pager.PagesCount}";
            };

            InitializeNewPaginator();

            ThreatDataGrid.ItemsSource = _pager.PageForward();

            PaginatorLabel.Content = $"1/{_pager.PagesCount}";

            if (Threat.LatestUpdateDate == DateTime.MinValue)
            {
                LatestUpdateLabel.Content = "";
            }
            else
            {
                LatestUpdateLabel.Content = Threat.LatestUpdateDate.ToString();
            }

            if ((ThreatDataGrid.ItemsSource as BindingList<Threat>).Count == 0)
            {
                ThreatDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ResetAutoUpdateTimer();
                EmptyDataGridLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void InitializeNewPaginator()
        {
            _pager = new Paginator<Threat>(ThreatController.GetData(), 15);

            _pager.IsFirstPage += () => PrevPageButton.IsEnabled = false;
            _pager.IsLastPage += () => NextPageButton.IsEnabled = false;
            _pager.IsNotFirstPage += () => PrevPageButton.IsEnabled = true;
            _pager.IsNotLastPage += () => NextPageButton.IsEnabled = true;
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            _dispatcherTimer.Stop();
        }

        protected void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            try
            {
                UpdateAndRefresh();
                MessageBox.Show($"Успешно!\n\n{_updatedThreatsCount} обновленных записей!");
                _updatedThreatsCount = 0;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            ResetAutoUpdateTimer();
        }

        private void ResetAutoUpdateTimer()
        {
            try
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
                if (Threat.LatestUpdateDate.AddHours(1) < DateTime.Now)
                {
                    _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                }
                else
                {
                    _dispatcherTimer.Interval = Threat.LatestUpdateDate.AddHours(1).Subtract(DateTime.Now);
                }
                _dispatcherTimer.Start();
                _timerIsActive = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowThreatInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ThreatDataGrid.SelectedItem != null)
                {
                    var threatWindow = new ThreatWindow();
                    threatWindow.Show(ThreatDataGrid.SelectedItem as Threat);
                }
            }
            catch
            {
                MessageBox.Show("Похоже, ты ошибся строчкой");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (_timerIsActive)
            {
                _dispatcherTimer.Stop();
            }
            try
            {
                UpdateAndRefresh();
                MessageBox.Show($"Успешно!\n\n{_updatedThreatsCount} обновленных записей!");
                _updatedThreatsCount = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка!");
            }
            ResetAutoUpdateTimer();
        }

        private void UpdateAndRefresh()
        {
            BindingList<ThreatUpdate> changedThreats;
            BindingList<Threat> newThreats;
            BindingList<Threat> deletedThreats;

            ThreatController.UpdateInfo(out changedThreats, out newThreats, out deletedThreats);
            Refresh();

            _updatedThreatsCount = changedThreats.Count + newThreats.Count + deletedThreats.Count;

            if (_updatedThreatsCount > 0)
            {
                var updateWindow = new UpdateWindow();
                updateWindow.Show(changedThreats, newThreats, deletedThreats);
            }
        }

        private void Refresh()
        {
            InitializeNewPaginator();

            ThreatDataGrid.ItemsSource = _pager.PageForward();
            ThreatDataGrid.Items.Refresh();

            PaginatorLabel.Content = _pager.CurrentPageId + $"/{_pager.PagesCount}";
            LatestUpdateLabel.Content = Threat.LatestUpdateDate.ToString();
            ThreatDataGrid.Visibility = Visibility.Visible;
            EmptyDataGridLabel.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeUIComponents();
            Application.Current.Exit += Current_Exit;
        }
    }
}
