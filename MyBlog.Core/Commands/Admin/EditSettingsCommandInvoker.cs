using System;
using System.Linq;
using Microsoft.Extensions.Options;
using MyBlog.Extensions;
using MyBlog.EFCore;
using MyBlog.Settings;

namespace MyBlog.Commands.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class EditSettingsCommandInvoker : ICommandInvoker<EditSettingsCommand, CommandResult>
    {
        private readonly BlogDbContext _context;
        private readonly IOptions<AppConfig> _appConfig;

        public EditSettingsCommandInvoker(
            BlogDbContext db,
            IOptions<AppConfig> appConfig)
        {
            this._context = db;
            _appConfig = appConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public CommandResult Execute(EditSettingsCommand command)
        {
            try
            {
                //var entity = this._context.Settings.Where(o => o.Key == AppConsts.BasicSetting).FirstOrDefault();

                return new CommandResult();
            }
            catch (Exception e)
            {
                return new CommandResult(e.ToString());
            }
        }
    }
}
