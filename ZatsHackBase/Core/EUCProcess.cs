using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public class EUCProcess
    {
        #region VARIABLES
        private Dictionary<string, ProcessModule> modules;
        #endregion

        #region PROPERTIES
        public Process Process { get; private set; }
        public IntPtr Handle { get; private set; }
        public bool IsRunning
        {
            get
            {
                if (Process == null)
                    return false;

                return !Process.HasExited;
            }
        }
        public Memory Memory { get; private set; }
        public bool IsInForeground { get { return WinAPI.GetForegroundWindow() == Process.MainWindowHandle; } }
        #endregion

        #region CONSTRUCTORS
        private EUCProcess(Process proc)
        {
            modules = new Dictionary<string, ProcessModule>();

            Process = proc;
            Handle = WinAPI.OpenProcess(WinAPI.ProcessAccessFlags.All, false, proc.Id);
            Memory = new Memory(this);
        }

        public static EUCProcess WaitForProcess(string name, int timeOut = -1)
        {
            DateTime start = DateTime.Now;

            while (!IsProcessRunning(name) &&
                    (timeOut <= 0 || 
                        (timeOut > 0 && 
                        (start - DateTime.Now).TotalMilliseconds >= timeOut)))
                Thread.Sleep(500);

            return GetProcessByName(name);
        }
        public static EUCProcess GetProcessByName(string name)
        {
            var procs = Process.GetProcessesByName(name);
            if (procs.Length == 0)
                throw new Exception("Could not find any matching process");

            return new EUCProcess(procs[0]);
        }
        #endregion

        #region METHODS
        public static bool IsProcessRunning(string name)
        {
            return Process.GetProcessesByName(name).Length > 0;
        }
        #endregion

        #region PROPERTIES
        public ProcessModule this[string name]
        {
            get
            {
                if (modules.ContainsKey(name))
                    return modules[name];
                else
                {
                    foreach(ProcessModule mod in Process.Modules)
                    {
                        if (mod.FileName == name)
                        {
                            modules[name] = mod;
                            return mod;
                        }
                    }
                    return null;
                }
            }
        }
        #endregion
    }
}
