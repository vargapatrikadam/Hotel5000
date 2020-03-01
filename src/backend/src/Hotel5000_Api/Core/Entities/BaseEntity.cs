using System;

namespace Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}