using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Settings
{
    public class Setting : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
}
