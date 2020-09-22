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
        private readonly User _user;
        private readonly bool _isAnonymous;
        public AuthorizeAction(Entity entity, Operation operation, User user)
        {
            _entity = entity;
            _operation = operation;
            _user = user;
            _isAnonymous = false;
        }
        public AuthorizeAction(Entity entity, Operation operation, User user, bool isAnonymous)
        {
            _entity = entity;
            _operation = operation;
            _user = user;
            _isAnonymous = isAnonymous;
        }
        public Operation.Type Operation { get => _operation.Action; }
        public string EntityName { get => _entity.Name; }
        public int EntityAccessorId { get => _entity.Id; }
        public string UserName { get => _user.Username; }
        public int UserId { get => _user.Id; }
        public bool IsAnonymous { get => _isAnonymous; }
    }
}
