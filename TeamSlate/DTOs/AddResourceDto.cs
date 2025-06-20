namespace TeamSlate.DTOs
{
    public class AddResourceDto
    {
        public string Name { get; set; }
        public int DesignationId { get; set; }
        public int BillableId { get; set; }
        public string Availability { get; set; }
        public List<SkillDto> ResourceSkills { get; set; }
        public List<WeeklyHourDto> WeeklyHours { get; set; }
    }
}
