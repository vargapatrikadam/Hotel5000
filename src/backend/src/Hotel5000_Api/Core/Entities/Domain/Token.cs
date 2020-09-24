using System;

namespace Core.Entities.Domain
{
    public class Token : BaseEntity
    {
        public string RefreshToken { get; set; }
        public DateTime UsableFrom { get; set; }
        public DateTime ExpiresAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}