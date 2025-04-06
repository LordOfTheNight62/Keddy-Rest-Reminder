using System.Windows;
using System.Windows.Input;
using Keddy_Rest_Reminder.Core;

namespace Keddy_Rest_Reminder
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindowState MessageWindowState { get; set; }
        public bool? Result;

        public MessageWindow(string title, string message, MessageWindowState messageWindowState)
        {
            InitializeComponent();
            WindowTitle.Content = title;
            MessageLabel.Content = message;
            this.Topmost = true;
            MessageWindowState = messageWindowState;
            InitializeWindow();
        }

        public void InitializeWindow()
        {
            switch(MessageWindowState)
            {
                case MessageWindowState.OK:
                    OkBtn.Visibility = Visibility.Visible;
                    break;
                case MessageWindowState.YesNo:
                    YesBtn.Visibility = Visibility.Visible;
                    NoBtn.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Toolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            this.Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();
        }
    }
}
