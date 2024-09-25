namespace TechnicalTest.Shared.Models.Contacts
{
    public class ContactAddRequest
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ContactType { get; set; } = null!;
        public int ContactGroupId { get; set; }
    }
}
