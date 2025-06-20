using System.Collections.Generic;

namespace TeamSlate.Models
{
    public class BillableMaster
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    }
}
