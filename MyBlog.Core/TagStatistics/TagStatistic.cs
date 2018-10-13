using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core.TagStatistics
{
    public class TagStatistic
    {
        public long Id { get; set; }

        public string Value { get; set; }

        public long Count { get; set; }
    }
}
