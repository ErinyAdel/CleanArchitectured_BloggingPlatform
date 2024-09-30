using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
    }
}
