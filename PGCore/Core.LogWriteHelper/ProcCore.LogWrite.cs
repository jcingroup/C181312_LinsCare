using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using ProcCore.NetExtension;

namespace ProcCore //Log記錄除錯 放在名命空間第一層 以利除錯方便使用
{
    public static class Log
    {
        public static String SetupBasePath { get; set; }
        public static Boolean Enabled { get; set; }
        private static Queue<String> QueueMessage = new Queue<String>();

        #region Write Method Function
        public static void WriteToFile()
        {
            if (SetupBasePath == null) SetupBasePath = "D:\\";
            String FileName = "Log." + DateTime.Now.ToString("yyyyMMddHH") + ".txt";

            try
            {
                FileStream fs = File.Open(SetupBasePath + FileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                while (QueueMessage.Count > 0)
                {
                    String dq = QueueMessage.Dequeue();
                    sw.WriteLine(dq);
                }
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (IOException) {}
        }
        public static void Write(String message)
        {
            if (Enabled)
                QueueMessage.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "=>" + message);
        }
        public static void Write(LogPlamInfo pi, LogMessageType mType, String className, String methodName, String message)
        {
            String strTpl = "[{0}][I:{1},{2},{3}][{4}.{5}][M:{6}]";
            Write(String.Format(strTpl, mType,
                pi.UserId,
                pi.IP,
                pi.BroswerInfo,
                className, methodName, message));
        }
        public static void Write(LogPlamInfo pi,String className, String methodName, String message)
        {
            Write(pi, LogMessageType.Info, className, methodName, message);
        }
        public static void Write(LogPlamInfo pi, String className, String methodName, params String[] message)
        {
            String messages = message.JoinArray(",");
            Write(pi, LogMessageType.Info, className, methodName, messages);
        }

        public static void Write(LogPlamInfo pi,String className, String methodName, Exception ex)
        {
            String message = String.Format("<[{0}][{1}]>", ex.Message, ex.StackTrace);
            Write(pi, LogMessageType.LogicError, className, methodName, message);
        }
        public static void Write(LogPlamInfo pi,String className, String methodName, LogicError ex)
        {
            String message = String.Format("<[{0}][{1}]>", ex.Message,ex.StackTrace);
            Write(pi, LogMessageType.LogicError, className, methodName, message);
        }
        public static void Write(LogPlamInfo pi,String className, String methodName, LogicRoll ex)
        {
            String message = String.Format("<[{0}][{1}]>", ex.Message, ex.StackTrace);
            Write(pi, LogMessageType.SystemError, className, methodName, message);
        } 
        #endregion

        public class LogPlamInfo
        {
            public Boolean AllowWrite { get; set; }
            public Int32 UserId { get; set; }
            public Int32 UnitId { get; set; }
            public String IP { get; set; }
            public String BroswerInfo { get; set; }
        }
        public enum LogMessageType
        {
            Info, Success, LogicError, SystemError
        }
    }
}
