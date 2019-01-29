using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MicwayTechTest.Log
{
    public class LogUtil
    {
        private static readonly string LOGPATH = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

        private static readonly ILog log = LogManager.GetLogger("appender");
        private static bool isSet = false;

        private static void Configure()
        {
            if (isSet == false)
            {
                XmlConfigurator.Configure(new FileInfo(string.Format("{0}\\{1}", LOGPATH, "log4net.config")));
                isSet = true;
            }
        }

        public static void Debug(string msg)
        {
            Configure();
            log.Debug(msg);
        }

        public static void Warn(string msg)
        {
            Configure();
            log.Warn(msg);
        }

        public static void Info(string msg)
        {
            Configure();
            log.Info(msg);
        }

        public static void Error(string msg)
        {
            Configure();
            log.Error(msg);
        }

        public static void Error(Exception e)
        {
            Configure();
            log.Error(e);
        }
    }
}