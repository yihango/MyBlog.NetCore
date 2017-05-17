using System.Linq;
using System.Collections.Generic;

namespace MyBlog.Core
{
    /// <summary>
    /// 命令执行结果类
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// 命令结果中存储错误数据的集合
        /// </summary>
        private readonly List<CommandError> _errors = new List<CommandError>();

        /// <summary>
        /// 初始化命令处理结果对象(待确认成功的命令结果)
        /// </summary>
        public CommandResult() { }


        /// <summary>
        /// 初始化命令处理结果对象(发生错误的命令结果)
        /// </summary>
        /// <param name="errorMsg"></param>
        public CommandResult(string errorMsg)
        {
            this.AddError(errorMsg);
        }

        /// <summary>
        /// 成功的命令结果
        /// </summary>
        public static CommandResult SuccessResult
        {
            get
            {
                return new CommandResult();
            }
        }


        /// <summary>
        /// 添加一个错误信息到错误信息集合
        /// </summary>
        /// <param name="error"></param>
        public void AddError(string error)
        {
            _errors.Add(new CommandError(error));
        }


        /// <summary>
        /// 获取所有的命令错误信息(数组)
        /// </summary>
        /// <returns>错误信息数组</returns>
        public string[] GetErrors()
        {
            return _errors.Select(t => t.Message).ToArray();
        }

        /// <summary>
        /// 判断是否执行成功，执行成功返回true
        /// </summary>
        public bool IsSuccess
        {
            get { return !_errors.Any(); }
        }

    }
}
