using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSlate.Models
{
    public class ResourceSkill
    {
        [ForeignKey("Resource")]
        public int ResourceId { get; set; }

        public Resource Resource { get; set; }

        [ForeignKey("Skill")]
        public int SkillId { get; set; }

        public SkillMaster Skill { get; set; }
    }

}
