using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Web
{
    public class Global
    {
        /// <summary>
        /// 全局的错误队列
        /// </summary>
        public static Queue<Exception> ExceptionQueue { get; set; } = new Queue<Exception>();

        
    }
}
