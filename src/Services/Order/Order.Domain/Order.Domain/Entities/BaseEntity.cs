

using Order.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; protected set; }
        public BaseEntity Clone()
        {
            return (BaseEntity)this.MemberwiseClone();
        }
    }
}
