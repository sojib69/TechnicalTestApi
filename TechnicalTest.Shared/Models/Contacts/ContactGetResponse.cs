namespace TechnicalTest.Shared.Models.Contacts
{
    public class ContactGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ContactType { get; set; } = null!;
        public int ContactGroupId { get; set; }
        public string ContactGroupName { get; set; } = null!;
    }
}
