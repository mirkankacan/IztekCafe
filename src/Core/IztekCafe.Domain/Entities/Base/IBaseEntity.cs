namespace IztekCafe.Domain.Entities.Base
{
    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}