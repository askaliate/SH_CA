using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Core.Entities
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 707) ^ Id.GetHashCode();
        }
    }
}
