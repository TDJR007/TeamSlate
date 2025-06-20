namespace TeamSlate.DTOs
{
    public class ResourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public List<string> Skills { get; set; }
        public string Billable { get; set; }
        public string Availability { get; set; }
        public int? Hours { get; set; } // If you want to fetch current week’s hours
    }

}
