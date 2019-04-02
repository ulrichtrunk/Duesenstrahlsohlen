using NPoco;

namespace Shared.Entities
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }

        [Ignore]
        public bool IsTransient
        {
            get
            {
                if (Id == null)
                {
                    return true;
                }

                return Id.Equals(default(TId));
            }
        }
    }
}
