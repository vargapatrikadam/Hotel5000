namespace Core.Entities.Domain
{
    public class Contact : BaseEntity
    {
        public string MobileNumber { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}