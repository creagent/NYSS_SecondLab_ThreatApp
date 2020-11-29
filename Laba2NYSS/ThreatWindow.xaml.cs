using Laba2NYSS.Models;
using System.Windows;

namespace Laba2NYSS
{
    /// <summary>
    /// Логика взаимодействия для ThreatWindow.xaml
    /// </summary>
    public partial class ThreatWindow : Window
    {
        public ThreatWindow()
        {
            InitializeComponent();
        }

        internal void Show(Threat threat)
        {
            IdTextBox.Text = threat.Id.ToString();
            NameTextBox.Text = threat.Name;
            DescriptionTextBox.Text = threat.Description;
            SourceTextBox.Text = threat.Source;
            ObjectTextBox.Text = threat.Object;
            PrivacyTextBox.Text = threat.PrivacyViolation;
            IntegrityTextBox.Text = threat.IntegrityViolation;
            AccessTextBox.Text = threat.IntegrityViolation;

            Show();
        }
    }
}
