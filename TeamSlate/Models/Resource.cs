using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamSlate.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Designation")]
        public int DesignationId { get; set; }

        public DesignationMaster Designation { get; set; }

        [Required]
        [ForeignKey("Billable")]
        public int BillableId { get; set; }

        public BillableMaster Billable { get; set; }

        [Required]
        public string Availability { get; set; }
        public ICollection<ResourceSkill> ResourceSkills { get; set; } = new List<ResourceSkill>();
        public ICollection<WeeklyHour> WeeklyHours { get; set; } = new List<WeeklyHour>();
    }

}
