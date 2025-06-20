using System.Collections.Generic;

namespace TeamSlate.Models
{
    public class DesignationMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    }
}
