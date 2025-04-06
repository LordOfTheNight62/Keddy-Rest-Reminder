using System.Windows;
using Keddy_Rest_Reminder.Core;

namespace Keddy_Rest_Reminder
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private List<string>? durations;

        public SettingsWindow()
        {
            InitializeComponent();
            WindowTitle.Content = this.Title;
            durations = Enumerable.Range(1, 12).Select(x => (x * 5).ToString()).ToList();
            DurationComboBox.ItemsSource = durations;
            SyncComponentSettings();
        }

        private void SyncComponentSettings()
        {
            DurationComboBox.SelectedItem = (AppSettings.DefaultDuration / 60).ToString();
        }

        private void Toolbar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.Save(Convert.ToInt32(DurationComboBox.SelectedItem) * 60);
            this.Close();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.Reset(SyncComponentSettings);
        }
    }
}
