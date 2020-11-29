using Laba2NYSS.Models;
using System.ComponentModel;
using System.Windows;

namespace Laba2NYSS
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        internal void Show(BindingList<ThreatUpdate> changedThreats, BindingList<Threat> newThreats, BindingList<Threat> deletedThreats)
        {
            if (changedThreats.Count == 0 && newThreats.Count == 0 && deletedThreats.Count == 0)
            {
                ChangesDataGrid.Visibility = Visibility.Collapsed;
                NewThreatsDataGrid.Visibility = Visibility.Collapsed;
                DeletedThreatsDataGrid.Visibility = Visibility.Collapsed;
                ChangedThreatsLabel.Visibility = Visibility.Collapsed;
                NewThreatsLabel.Visibility = Visibility.Collapsed;
                DeletedThreatsLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoChangesDetectedLabel.Visibility = Visibility.Collapsed;
                if (changedThreats.Count == 0)
                {
                    ChangesDataGrid.Visibility = Visibility.Collapsed;
                    ChangedThreatsLabel.Visibility = Visibility.Collapsed;
                }
                if (newThreats.Count == 0)
                {
                    NewThreatsDataGrid.Visibility = Visibility.Collapsed;
                    NewThreatsLabel.Visibility = Visibility.Collapsed;
                }
                if (deletedThreats.Count == 0)
                {
                    DeletedThreatsDataGrid.Visibility = Visibility.Collapsed;
                    DeletedThreatsLabel.Visibility = Visibility.Collapsed;
                }
            }
            ChangesDataGrid.ItemsSource = changedThreats;
            NewThreatsDataGrid.ItemsSource = newThreats;
            DeletedThreatsDataGrid.ItemsSource = deletedThreats;

            Show();
        }
    }
}
