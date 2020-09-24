﻿using Core.Entities.Domain;
using Core.Enums.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Domain
{
    public class Role : BaseEntity
    {
        public RoleType Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
