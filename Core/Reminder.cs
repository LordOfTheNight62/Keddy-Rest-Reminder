using System.Windows;
using System.Windows.Threading;

namespace Keddy_Rest_Reminder.Core
{
    internal class Reminder
    {
        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                if(!Active)
                    StartingDuration = value;
            }
        }
        private int StartingDuration { get; set; }
        public int LastDuration { get; private set; }
        public bool Completed { get; private set; }
        public bool Active { get; private set; }
        public bool Paused { get; set; }
        public Action? CallBacks;
        private DispatcherTimer? timer;

        public Reminder()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Process;
        }

        public void Start()
        {
            timer?.Start();
            Active = true;
            Paused = false;
        }

        public void Stop()
        {
            timer?.Stop();
            Paused = true;
        }

        public void Reset()
        {
            timer?.Stop();
            Active = false;
            Paused = false;
            Completed = false;
        }

        private void Process(object sender, EventArgs e)
        {
            if (Duration > 0)
            {
                Duration--;
            }
            else
            {
                Completed = true;
                Stop();
                UserData.Save(UserData.DailyDuration + StartingDuration);
                MessageWindow messageWindow = new MessageWindow("Mola Vakti", "Süreniz bitti, biraz mola vermeye ne dersiniz?", MessageWindowState.OK);
                messageWindow.ShowDialog();
            }
            CallBacks?.Invoke();
        }
    }
}
