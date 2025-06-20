using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamSlate.Models
{
    

    public class SkillMaster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ResourceSkill> ResourceSkills { get; set; } = new List<ResourceSkill>();
    
    }

}
