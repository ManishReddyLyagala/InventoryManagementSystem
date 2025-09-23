namespace InventoryManagement_Backend.Settings
{
    public class StockAlertSettings
    {
        public int Threshold { get; set; } = 11;
        public int DailyScheduleHour { get; set; } = 9;
        public int DailyScheduleMinute { get; set; } = 0;
        public List<string> Recipients { get; set; } = new();
    }
}
