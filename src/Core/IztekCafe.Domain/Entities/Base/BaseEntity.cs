namespace IztekCafe.Domain.Entities.Base
{
    public abstract class BaseEntity<TId> : IBaseEntity where TId : struct
    {
        public TId Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}