using TechnicalTest.Domain.Contracts;

namespace TechnicalTest.Domain.Entities
{
    public class BaseEntity : IEntity
    {
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
