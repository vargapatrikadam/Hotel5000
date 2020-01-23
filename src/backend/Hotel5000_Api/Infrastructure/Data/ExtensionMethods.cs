using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public static class ExtensionMethods
    {
        public static void UpdateSoftDeleteStatuses(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}
