using Core.Entities.LodgingEntities;
using Core.Enums.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Authentication
{
    public class AuthorizeAction
    {
        private readonly Entity _entity;
        private readonly Operation _operation;
        private readonly EntityOwner _owner;
        public AuthorizeAction(Entity entity, Operation operation, EntityOwner owner)
        {
            _entity = entity;
            _operation = operation;
            _owner = owner;
        }
        public Operation.Type Operation { get => _operation.Action; }
        public string EntityName { get => _entity.Name; }
        public string UserName { get => _owner.Username; }
        public int ResourceOwnerId { get => _owner.UserId; }
    }
}
