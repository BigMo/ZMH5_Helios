using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.IO
{
    public class Logger
    {
        #region VARIABLES
        private object printLock = new object();
        #endregion

        #region PROPERTIES
        public bool PrintDate { get; set; }
        public bool PrintTime { get; set; }
        public List<TextWriter> Outputs { get; set; }
        public bool FlushInstantly { get; set; }
        #endregion

        public Logger()
        {
            Outputs = new List<TextWriter>();
            Outputs.Add(Console.Out);
            PrintDate = true;
            PrintTime = true;
            FlushInstantly = true;
        }

        ~Logger()
        {
            Kill();
        }

        #region METHODS
        /// <summary>
        /// Closes and disposes all outputs added to this instance
        /// </summary>
        public void Kill()
        {
            foreach (TextWriter output in Outputs)
            {
                try
                {
                    output.Close();
                    output.Dispose();
                } catch { }
            }
            Outputs.Clear();
        }
        private void PrintToOutputs(string text)
        {
            foreach (TextWriter output in Outputs)
            {
                output.Write(text);
                if (FlushInstantly)
                    output.Flush();
            }
        }

        private void PrintDateTime()
        {
            if (!PrintDate && !PrintTime)
                return;

            StringBuilder str = new StringBuilder();
            str.Append("[");
            if (PrintDate && PrintTime)
                str.Append(DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString());
            else if (PrintDate && !PrintTime)
                str.Append(DateTime.Now.ToShortDateString());
            else if (!PrintDate && PrintTime)
                str.Append(DateTime.Now.ToLongTimeString());
            str.Append("] ");

            PrintToOutputs(str.ToString());
        }

        public void PrintLine(string category, string message, object[] parameters, ConsoleColor color = ConsoleColor.White)
        {
            lock (printLock)
            {
                PrintDateTime();
                PrintToOutputs(string.Format("[{0}] ", category));

                ConsoleColor c = Console.ForegroundColor;
                Console.ForegroundColor = color;
                PrintToOutputs(string.Format(message + "\n", parameters));
                Console.ForegroundColor = c;
            }
        }

        public void Log(string message, params object[] parameters)
        {
            PrintLine("LOG", message, parameters, ConsoleColor.DarkGray);
        }

        public void Warning(string message, params object[] parameters)
        {
            PrintLine("WARNING", message, parameters, ConsoleColor.DarkYellow);
        }

        public void Info(string message, params object[] parameters)
        {
            PrintLine("INFO", message, parameters, ConsoleColor.Gray);
        }

        public void Error(string message, params object[] parameters)
        {
            PrintLine("ERROR", message, parameters, ConsoleColor.Red);
        }

        public void LogEx(object sender, string message, params object[] parameters)
        {
            PrintLine("LOG", "[" + sender.GetType().Name + "] " + message, parameters, ConsoleColor.DarkGray);
        }

        public void WarningEx(object sender, string message, params object[] parameters)
        {
            PrintLine("WARNING", "[" + sender.GetType().Name + "] " + message, parameters, ConsoleColor.DarkYellow);
        }

        public void InfoEx(object sender, string message, params object[] parameters)
        {
            PrintLine("INFO", "[" + sender.GetType().Name + "] " + message, parameters, ConsoleColor.Gray);
        }

        public void ErrorEx(object sender, string message, params object[] parameters)
        {
            PrintLine("ERROR", "[" + sender.GetType().Name + "] " + message, parameters, ConsoleColor.Red);
        }

        public void PrintMultipleLines(string category, string message, int indents)
        {
            string prefix = new string('\t', indents);
            lock (printLock)
            {
                using (MemoryStream str = new MemoryStream(Encoding.UTF8.GetBytes(message)))
                {
                    using (StreamReader reader = new StreamReader(str))
                    {
                        string line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            PrintLine(category, prefix + line, new object[0]);
                        }
                    }
                }
            }
        }

        public void PrintException(Exception ex, bool showMessage, bool showStackTrace)
        {
            lock (printLock)
            {
                PrintLine("EXCEPTION", "Exception \"{0}\" cought", new object[] { ex.GetType().Name });
                if (showMessage)
                    PrintMultipleLines("EXCEPTION", "Message:\n" + ex.Message, 1);
                if (showStackTrace)
                    PrintMultipleLines("EXCEPTION", "StackTrace:\n" + ex.StackTrace, 1);
                if (ex.InnerException != null)
                    PrintException(ex.InnerException, showMessage, showStackTrace);
            }
        }
        #endregion
    }
}
