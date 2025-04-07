namespace Keddy_Rest_Reminder.Core
{
    internal class UserData
    {
        public static DateTime? DailyDurationDate { get; private set; }
        public static int DailyDuration { get; private set; }


        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static void Save(int dailyDuration)
        {
            DailyDuration = dailyDuration;
            Properties.Settings.Default.DailyDuration = dailyDuration;
            Properties.Settings.Default.Save();
        }

        public static void Load()
        {
            DailyDurationDate = Properties.Settings.Default.DailyDurationDate;
            DailyDuration = Properties.Settings.Default.DailyDuration;

            if(Properties.Settings.Default.DailyDurationDate.Date != DateTime.Today)
            {
                Save(0);
                Properties.Settings.Default.DailyDurationDate = DateTime.Today;
                DailyDurationDate = DateTime.Today;
                Save();
            }
        }
    }
}
