using Core.Entities;
using Core.Interfaces;
using Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging
{
    public class LoggingDBRepository<TEntity> : ARepository<TEntity, LoggingDBContext> 
        where TEntity : BaseEntity
    {
        public LoggingDBRepository(LoggingDBContext context) : base(context)
        {

        }
    }
}
