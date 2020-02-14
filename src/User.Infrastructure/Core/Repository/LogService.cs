using EInfrastructure.Core.Configuration.Ioc;
using NLog;

namespace User.Infrastructure.Core.Repository
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public class LogService : ILogService
    {
        private static readonly Logger Log = LogManager.GetLogger("WolfLog");

        #region 得到唯一标识

        /// <summary>
        /// 得到唯一标识
        /// </summary>
        /// <returns></returns>
        public string GetIdentify()
        {
            return "wolf_infrastructure_logservice";
        }

        #endregion

        #region 增加error文件日志

        /// <summary>
        /// 增加error文件日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        public void Error(string title, object msg = null)
        {
            Log?.Error(FormatMsg(title, msg));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        public void Error<T>(T t) where T : class
        {
            if (t is string)
            {
                Error(t.ToString(), null);
                return;
            }
        }

        #endregion

        #region 增加debug文件日志

        public void Debug(string title, object msg = null)
        {
            Log?.Debug(FormatMsg(title, msg));
        }

        public void Debug<T>(T t) where T : class
        {
            if (t is string)
            {
                Debug(t.ToString(), null);
            }
        }

        #endregion

        #region 增加info文件日志

        /// <summary>
        /// 增加info文件日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void Info(string title, object msg = null)
        {
            Log?.Info(FormatMsg(title, msg));
        }

        public void Info<T>(T t) where T : class
        {
            if (t is string)
            {
                Info(t.ToString(), null);
            }
        }

        #endregion

        #region 增加warn文件日志

        /// <summary>
        /// 增加warn文件日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        public void Warn(string title, object msg = null)
        {
            Log?.Warn(FormatMsg(title, msg));
        }

        public void Warn<T>(T t) where T : class
        {
            if (t is string)
            {
                Warn(t.ToString(), null);
            }
        }

        #endregion

        #region 增加trace文件日志

        /// <summary>
        /// 增加trace文件日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void Trace(string title, object msg = null)
        {
            Log?.Trace(FormatMsg(title, msg));
        }

        public void Trace<T>(T t) where T : class
        {
            if (t is string)
            {
                Trace(t.ToString(), null);
            }
        }

        #endregion

        #region private methods

        #region Format Content

        /// <summary>
        /// Format Content
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static string FormatMsg(string title, object msg)
        {
            if (msg == null)
            {
                return title;
            }

            return title + "\r\n" + msg;
        }

        #endregion

        #endregion
    }
}