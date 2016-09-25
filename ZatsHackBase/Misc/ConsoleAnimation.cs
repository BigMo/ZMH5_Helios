using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZatsHackBase.Misc
{
    public class ConsoleAnimation
    {
        #region VARIABLES
        private string text;
        private string currentString;
        private int interval;
        private Thread thread;
        #endregion

        #region PROPERTIES
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    currentString = text + Delimiter;
                }
            }
        }
        public string Delimiter { get; set; }
        public int Interval
        {
            get { return interval; }
            set
            {
                if (interval != value)
                    interval = Math.Max(1, value);
            }
        }
        #endregion

        #region EVENTS
        public EventHandler<EventArgs> RunningOutOfNews { get; set; }
        #endregion

        #region CONSTRUCTORS
        public ConsoleAnimation()
        {
            Text = Assembly.GetExecutingAssembly().GetName().FullName;
            currentString = Text;
            Delimiter = "  -  ";
            interval = (int)(1000f / 30);
            thread = new Thread(() => Loop());
            thread.IsBackground = true;
        }
        #endregion
        
        #region METHODS
        public void AppendNews(string news)
        {
            currentString += news + Delimiter + Text + Delimiter;
        }

        private void Loop()
        {
            while (true)
            {
                if (currentString != null)
                {
                    if (currentString.Length < 200)
                    {
                        RunningOutOfNews?.Invoke(this, new EventArgs());
                    }
                    currentString = currentString.Substring(1);
                    Console.Title = currentString;
                }
                Thread.Sleep(interval);
            }
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            thread.Abort();
        }
        #endregion
    }
}
