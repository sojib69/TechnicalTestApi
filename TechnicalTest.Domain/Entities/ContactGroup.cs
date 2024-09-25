using System.ComponentModel.DataAnnotations;

namespace TechnicalTest.Domain.Entities
{
    public class ContactGroup : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GroupName { get; set; } = null!;
    }
}
