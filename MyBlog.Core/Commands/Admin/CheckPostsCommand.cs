namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 检查博文命令
    /// 若存在异常博文将清理
    /// </summary>
    public class CheckPostsCommand
    {
        public CheckPostsCommand()
        {
            this.Take = 20;
            this.PageNum = 1;
        }

        /// <summary>
        /// 每页取得数量
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 所有页
        /// </summary>
        public int AllPageNum { get; set; }

        /// <summary>
        /// 下一页
        /// </summary>
        public bool HasNext
        {
            get
            {
                return PageNum < AllPageNum;
            }
        }

        /// <summary>
        /// 当前Web应用程序静态文件根目录
        /// </summary>
        public string WebRootPath { get; set; }
    }
}
