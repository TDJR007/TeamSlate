namespace TeamSlate.DTOs
{
    public class ResourceWithWeeklyHoursDto
    {
        public string Name { get; set; }
        public string Billable { get; set; }
        public string Designation { get; set; }
        public string Skills { get; set; }
        public string Availability { get; set; }
        public List<WeeklyHourDto> WeeklyHours { get; set; }
    }
}