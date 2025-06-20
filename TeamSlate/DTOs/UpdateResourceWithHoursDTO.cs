namespace TeamSlate.DTOs
{

    public class UpdateResourceWithHoursDto
    {
        public int ResourceId { get; set; }
        public string Name { get; set; }
        public int DesignationId { get; set; }
        public int BillableId { get; set; }
        public List<int> SkillIds { get; set; }
        public string Availability { get; set; }

        public List<WeeklyHourDto> WeeklyHours { get; set; }
    }
}

