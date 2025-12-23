namespace AirportTool.Application.Options
{
    public class ScheduleImportOptions
    {
        public int MaxRows { get; set; } = 1000;
        public long MaxFileSizeBytes { get; set; } = 2 * 1024 * 1024;
    }
}
