using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Entities
{
    public abstract class Entity<TType>
    {
        public TType Id { get; set; }
    }

    public abstract class Entity : Entity<int>
    {

    }
}
