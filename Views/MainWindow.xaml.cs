using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Keddy_Rest_Reminder.Core;

namespace Keddy_Rest_Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string>? durations;
        private Reminder reminder;

        public MainWindow()
        {
            InitializeComponent();
            App();
        }

        #region Toolbar

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Toolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion

        private void App()
        {
            AppSettings.Load();
            UserData.Load();
            SyncUserData();
            durations = Enumerable.Range(1, 12).Select(x => (x * 5).ToString()).ToList();
            DurationComboBox.ItemsSource = durations;
            reminder = new Reminder();
            reminder.Duration = Convert.ToInt32(DurationComboBox.SelectedItem) * 60;
            reminder.CallBacks += UpdateUI;

            WindowTitle.Content = $"{this.Title} - {DurationComboBox.SelectedItem} dakika";
            SyncComponentSettings();
            UpdateUI();
        }

        private void SyncComponentSettings()
        {
            DurationComboBox.SelectedItem = (AppSettings.DefaultDuration / 60).ToString();
        }

        private void SyncUserData()
        {
            DailyDurationLabel.Content = $"Bugün Toplam {UserData.DailyDuration / 60 / 60} sa {UserData.DailyDuration / 60} dk";
        }

        private void UpdateUI()
        {
            int minutes = reminder.Duration / 60;
            int seconds = reminder.Duration % 60;
            TimeLabel.Content = $"{minutes:D2}:{seconds:D2}";
            if (reminder.Paused)
                TimeLabel.Foreground = new SolidColorBrush(Colors.Red);
            else
                TimeLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            if (reminder.Completed)
            {
                ResetBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                SyncUserData();
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            bool? result = settingsWindow.ShowDialog();
            if (result == false && !reminder.Active)
            {
                SyncComponentSettings();
                UpdateUI();
            }
        }

        private void DurationComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            WindowTitle.Content = $"{this.Title} - {DurationComboBox.SelectedItem} dakika";
            if (reminder is not null)
            {
                reminder.Duration = Convert.ToInt32(DurationComboBox.SelectedItem) * 60;
                UpdateUI();
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            reminder.Start();
            StartBtn.Visibility = Visibility.Hidden;
            PauseResumeBtn.Visibility = Visibility.Visible;
            ResetBtn.Visibility = Visibility.Visible;
            DurationComboBox.Visibility = Visibility.Hidden;
        }

        private void PauseResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            reminder.Paused = !reminder.Paused;
            if (reminder.Paused)
                reminder.Stop();
            else
                reminder.Start();
            UpdateUI();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            reminder.Reset();
            reminder.Duration = Convert.ToInt32(DurationComboBox.SelectedItem) * 60;
            UpdateUI();
            StartBtn.Visibility = Visibility.Visible;
            PauseResumeBtn.Visibility = Visibility.Hidden;
            ResetBtn.Visibility = Visibility.Hidden;
            DurationComboBox.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (reminder.Active)
            {
                MessageWindow messageWindow = new MessageWindow("Uygulamayı kapatmak istiyor musunuz?", "Şu an sayacınız aktif, kapatırsanız sayaç sıfırlanacak.", MessageWindowState.YesNo);
                messageWindow.ShowDialog();

                if (messageWindow.Result.HasValue && !messageWindow.Result.Value)
                    e.Cancel = true;
            }
        }
    }
}