namespace Keddy_Rest_Reminder.Core
{
    public class AppSettings
    {
        public static int DefaultDuration { get; private set; }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static void Save(int defaultDuration)
        {
            DefaultDuration = defaultDuration;
            Properties.Settings.Default.DefaultDuration = defaultDuration;
            Properties.Settings.Default.Save();
        }

        public static void Load()
        {
            DefaultDuration = Properties.Settings.Default.DefaultDuration;
        }

        public static void Reset()
        {
            Properties.Settings.Default.DefaultDuration = 300;
            Save();
            Load();
        }

        public static void Reset(Action? callBack)
        {
            Properties.Settings.Default.DefaultDuration = 300;
            Save();
            Load();
            callBack?.Invoke();
        }
    }
}
