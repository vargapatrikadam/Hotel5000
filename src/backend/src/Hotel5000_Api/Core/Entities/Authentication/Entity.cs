using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Authentication
{
    public class Entity : BaseEntity
    {
        public Entity()
        {

        }
        public Entity(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public string Name { get; set; }
        public ICollection<Rule> Rules { get; set; }

    }
}
