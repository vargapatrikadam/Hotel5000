﻿using Core.Enums.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Authentication
{
    public class BaseRole : BaseEntity
    {
        public RoleType Name { get; set; }
        public ICollection<Rule> Rules { get; set; }
    }
}