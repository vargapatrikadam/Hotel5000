using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class LoggingDBRepository<TEntity> : ARepository<TEntity, LoggingDBContext> 
        where TEntity : class
    {
        public LoggingDBRepository(LoggingDBContext context) : base(context)
        {

        }
    }
}
