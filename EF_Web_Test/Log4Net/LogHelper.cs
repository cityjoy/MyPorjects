using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

//指定log4net使用的config文件来读取配置信息
namespace EF_Web_Test.Log4net
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        //通过配置文件的logerror 进行日志写入对象的实例化
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");   //选择<logger name="logerror">的配置 

        static LogHelper()
        {
            SetConfig();
        }

        /// <summary>
        /// 初始化web.config中的<log4net>节点中的配置
        /// </summary>
        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 负责将信息日志写入到日志文件
        /// </summary>
        public static void WriteInfo(string msg)
        {
            //检查信息日志是否允许
            if (logerror.IsInfoEnabled)
            {
                //表示写入信息级别的日志
                logerror.Info(msg);
            }
        }

        /// <summary>
        /// 负责将警告日志写入到日志文件
        /// </summary>
        public static void WriteWarn(string msg)
        {
            if (logerror.IsWarnEnabled)
            {
                //表示写入警告级别的日志
                logerror.Warn(msg);
            }
        }

        /// <summary>
        /// 负责将错误日志写入到日志文件
        /// </summary>
        public static void WriteError(string msg)
        {
            if (logerror.IsErrorEnabled)
            {
                //表示写入异常级别的日志
                logerror.Error(msg);
            }
        }
    }


}
