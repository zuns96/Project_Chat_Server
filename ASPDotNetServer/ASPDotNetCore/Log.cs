using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ASPDotNetCore
{
    public class Log
    {
        const int c_MAX_SIZE = 1024 * 1024 * 500;  // 500 MB

        static Log s_instance = null;

        StreamWriter m_textWriter = null;
        object m_objLock = null;

        static public void Create()
        {
            if (s_instance == null)
                s_instance = new Log();
        }

        static public void Write(string format, params object[] param)
        {
            Write(string.Format(format, param));
        }

        static public void Write(string msg)
        {
            if (s_instance != null)
                s_instance.write(msg);
        }

        public Log()
        {
            m_objLock = new object();
            createLogFile();
        }

        void createLogFile()
        {
            if(m_textWriter != null)
            {
                m_textWriter.Dispose();
                m_textWriter = null;
            }

            string fileName = string.Format("{0}_{1}.txt", AppDomain.CurrentDomain.FriendlyName, DateTime.Now.ToString("yyyy/MM/dd_HH;mm;ss"));
            string dir = @".\Log";
            string filePath = string.Format(@"{0}\{1}", dir, fileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            FileStream stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            m_textWriter = new StreamWriter(stream, Encoding.UTF8);
            m_textWriter.AutoFlush = true;
        }

        void write(string msg)
        {
            lock (m_objLock)
            {
                string log = string.Format("[{0}] : ({1}){2}", DateTime.Now.ToString("yyyy/MM/dd_HH;mm;ss.ffff"), Thread.CurrentThread.ManagedThreadId, msg);

                Console.WriteLine(log);

                lock (m_objLock)
                {
                    m_textWriter.WriteLine(log);

                    if(m_textWriter.BaseStream.Length >= c_MAX_SIZE)
                    {
                        createLogFile();
                    }
                }
            }
        }
    }
}
