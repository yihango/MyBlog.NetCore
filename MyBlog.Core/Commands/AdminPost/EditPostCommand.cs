namespace MyBlog.Core.Commands.AdminPost
{
    /// <summary>
    /// 编辑博文命令
    /// </summary>
    public class EditPostCommand
    {
        /// <summary>
        /// 博文编号
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 博文标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 旧的标题
        /// </summary>
        public string OldTitle { get; set; }

        /// <summary>
        /// 博文标签
        /// </summary>
        public string Tags { get; set; } = string.Empty;

        /// <summary>
        /// 博文内容
        /// </summary>
        public string PostContent { get; set; }

        private string _pubState;

        /// <summary>
        /// 是否发布（两个值，true=1，false=0）
        /// </summary>
        public string PubState
        {
            get
            {
                return _pubState;
            }
            set
            {
                switch (value)
                {
                    case "true":
                        this._pubState = "1";
                        break;
                    default:
                        this._pubState = "0";
                        break;
                }
            }
        }

        /// <summary>
        /// 之前的页码
        /// </summary>
        public int ReturnPageNum { get; set; }

        /// <summary>
        /// 当前应用静态文件所在目录
        /// </summary>
        public string WebRootPath { get; set; }
    }
}
