using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Exceptions
{
    public class EntityNotFoundException : System.Exception
    {
        public EntityNotFoundException(Type type, object id)
            : base($"No entity with {id} of {type.FullName} was found")
        {
        }
    }
}
