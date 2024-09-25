namespace TechnicalTest.Shared.Models.Contacts
{
    public class ContactEditRequest
    {
        public int Id { get; set; }
        public string ContactType { get; set; } = null!;
    }
}
