using System.ComponentModel.DataAnnotations;

namespace TechnicalTest.Domain.Entities
{
    public class Contact : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string ContactType { get; set; } = null!;
        [Required]
        public int ContactGroupId { get; set; }
    }
}
