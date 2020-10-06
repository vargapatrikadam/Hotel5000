namespace Core.Entities.Authentication
{
    public class Rule : BaseEntity
    {
        public BaseRole Role { get; set; }
        public Operation Operation { get; set; }
        public Entity Entity { get; set; }
        public bool IsAllowed { get; set; }
        public int RoleId { get; set; }
        public int OperationId { get; set; }
        public int EntityId { get; set; }
    }
}
