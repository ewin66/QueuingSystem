﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace Queue_Show_TV
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                CefRuntime.Load();
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            catch (CefRuntimeException ex) 
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 3;
            }

            var mainArgs = new CefMainArgs(args);
            var app = new CefWebApp();

         //   var exitCode = CefRuntime.ExecuteProcess(mainArgs, app);
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
                return exitCode;

            //var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //var localFolder = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
            //var browserProcessPath = CombinePaths(localFolder, "..", "..", "..",
            //    "CefGlue.Demo.WinForms", "bin", "Release", "Xilium.CefGlue.Demo.WinForms.exe");

            var settings = new CefSettings
            {
             //   BrowserSubprocessPath = browserProcessPath,
                SingleProcess = false,
                MultiThreadedMessageLoop = true,
                LogSeverity = CefLogSeverity.Disable,
                LogFile = "CefGlue.log",
            };
           // CefRuntime.Initialize(mainArgs, settings, app);
           CefRuntime.Initialize(mainArgs, settings, app,IntPtr.Zero);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!settings.MultiThreadedMessageLoop)
            {
                Application.Idle += (sender, e) => { CefRuntime.DoMessageLoopWork(); };
            }

            Application.Run(new mainForm());

            CefRuntime.Shutdown();
            return 0;
        }

    }
}
