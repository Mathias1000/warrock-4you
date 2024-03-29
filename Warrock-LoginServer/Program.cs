﻿using System;
using System.Linq;
using System.IO;
using System.Security.Permissions;
using Warrock.Database;
using Warrock.Util;
using System.Collections.Generic;

namespace Warrock_LoginServer
{
    public class Program
    {
        internal static DatabaseManager DatabaseManager { get; set; }
        public List<GameServer> GamServerList = new List<GameServer>();

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            //if debug we always start with default settings :)
#if DEBUG
            //File.Delete("Login.xml");
#endif

            Console.Title = "Warrock.Login";
            Config.Instance = new Config();
            if (Config.Instance.LoadConfig())
            {
                Log.WriteLine(LogLevel.Info, "Load Settings Sucess");
            }
            else
            {
                Log.WriteLine(LogLevel.Error, "Could not start server. Press RETURN to exit.");
                Console.ReadLine();
            }
            if (Load())
            {
                Log.IsDebug = true;
                while (true)
                    Console.ReadLine();
            }
            else
            {
                Log.WriteLine(LogLevel.Error, "Could not start server. Press RETURN to exit.");
                Console.ReadLine();
            }
        }
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            #region Logging
            #region Write Errors to a log file
            // Create a writer and open the file:
            StreamWriter log;

            if (!File.Exists("errorlog.txt"))
            {
                log = new StreamWriter("errorlog.txt");
            }
            else
            {
                log = File.AppendText("errorlog.txt");
            }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(e.ToString());
            log.WriteLine();

            // Close the stream:
            log.Close();
            #endregion
            #endregion

            Log.WriteLine(LogLevel.Exception, "Unhandled Exception : " + e.ToString());
            Console.ReadKey(true);
        }
        public static bool Load()
        {
             DatabaseManager = new DatabaseManager(Config.Instance.LoginMysqlServer, (uint)Config.Instance.LoginMysqlPort,Config.Instance.LoginMysqlUser, Config.Instance.LoginMysqlPassword, Config.Instance.LoginMysqlDatabase,Config.Instance.LoginDBMinPoolSize, Config.Instance.LoginDBMaxPoolSize,Config.Instance.QuerCachePerClient,Config.Instance.OverloadFlags);
            DatabaseManager.GetClient().Connect(); //testclient

            Log.SetLogToFile(string.Format(@"Logs\Login\{0}.log", DateTime.Now.ToString("d_M_yyyy HH_mm_ss")));

            if (Reflector.GetInitializerMethods().Any(method => !method.Invoke()))
            {
                Log.WriteLine(LogLevel.Error, "Server could not be started. Errors occured.");
                return false;
            }
            else return true;
        }
    }
}
