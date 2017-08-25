using NLog;
using NLog.Config;
using NLog.Targets;
using System.Text;
using System.Configuration;

namespace MyBlog.Web.Common
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class NLogHelper
    {
        //先初始化Config
        private static LoggingConfiguration Config = new LoggingConfiguration();
        private static void Configuration(string loggerName)
        {
            var consoleTarget = new ConsoleTarget()
            {
                Layout = @"${date:format=HH\:mm\:ss} ${message}"
                //Encoding = Encoding.Default
            };
            //采用*号命名的LoggingRule，不用显示引用，启动其他任何name的LoggingRule都会自动带上它，默认起效。
            Config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));
            var fileTarget = new FileTarget()
            {
                FileName = @"${basedir}/output/" + loggerName + ".log",
                Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${message}",
                Encoding = Encoding.Default
            };
            Config.LoggingRules.Add(new LoggingRule(loggerName, LogLevel.Debug, fileTarget));
            LogManager.Configuration = Config;
        }
        public static Logger GetFileLogger(string loggerName)
        {
            //Config里面有实例，直接返回
            foreach (var item in Config.LoggingRules)
            {
                if (item.LoggerNamePattern == loggerName)
                    return LogManager.GetLogger(loggerName);
            }
            //Config里面没有实例，创建实例后返回
            Configuration(loggerName);
            return LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="logMsg">写入的信息</param>
        public static void Write(string logName,string logMsg)
        {
            GetFileLogger(logName).Info(logMsg);
        }
    }
}
