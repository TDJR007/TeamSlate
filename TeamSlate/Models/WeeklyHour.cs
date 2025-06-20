using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSlate.Models
{ 
    public class WeeklyHour
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime WeekStartDate { get; set; }

        [Required]
        public double Hours { get; set; }

        [Required]
        [ForeignKey("Resource")]
        public int ResourceId { get; set; }

        public Resource Resource { get; set; }
    }

}
