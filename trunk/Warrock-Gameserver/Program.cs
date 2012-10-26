using Warrock.Util;
using Warrock.Database;
using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Permissions;
using Warrock_Emulator.UdpServers;
using Warrock.Game;
using Warrock.CommandHandlers;
using Warrock.CommandHandlers.CommandInfo;

namespace Warrock
{
    public class Program
    {
        public static DateTime CurrentTime { get; set; }
        internal static DatabaseManager DatabaseManager;
        internal static DatabaseManager LoginDatabaseManager { get; set; }
   

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {
            Console.Title = "Warrock.GameServer";
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
                while (true)
                {
                    string cmd = Console.ReadLine();
                    string[] arguments = cmd.Split(' ');
                    string command = arguments[0];
                    string[] Args = new string[arguments.Length];
                    string[] temp = new string[arguments.Length-1];
                    for (int i = 0; i < arguments.Length - 1; i++)//resize console command
                    {
                        temp[i]= arguments[i + 1];
                    }
                    CommandStatus status = CmdCommandHandler.Instance.ExecuteCommand(command, arguments);
                    switch (status)
                    {
                        case CommandStatus.Error:
                           Console.WriteLine("Error executing command.");
                            break;
                        case CommandStatus.NotFound:
                            Console.WriteLine("Command not found.");
                            break;
                    }
                }
            }
        }
        public static long currTimeStamp
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return Convert.ToInt64(ts.TotalSeconds);
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
        private static bool Load()
        {
            Log.SetLogToFile(string.Format(@"Logs\GameServer\{0}.log", DateTime.Now.ToString("yyyy-MM-dd HHmmss")));
            Log.IsDebug = true;
#if DEBUG
            DatabaseManager = new DatabaseManager(Config.Instance.GameMysqlServer, (uint)Config.Instance.GameMysqlPort, Config.Instance.GameMysqlUser, Config.Instance.GameMysqlPassword, Config.Instance.GameMysqlDatabase, Config.Instance.GameDBMinPoolSize, Config.Instance.GameDBMaxPoolSize, Config.Instance.QuerCachePerClient, Config.Instance.OverloadFlags);
          LoginDatabaseManager = new DatabaseManager(Config.Instance.LoginMysqlServer, (uint)Config.Instance.LoginMysqlPort, Config.Instance.LoginMysqlUser, Config.Instance.LoginMysqlPassword, Config.Instance.LoginMysqlDatabase, Config.Instance.LoginDBMinPoolSize, Config.Instance.LoginDBMaxPoolSize, Config.Instance.QuerCachePerClient, Config.Instance.OverloadFlags);
            // so the startup works
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
#endif
            try
            {
                if (Reflector.GetInitializerMethods().Any(method => !method.Invoke()))
                {
                    Log.WriteLine(LogLevel.Error, "Server could not be started. Errors occured.");
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading Initializer methods: {0}", ex.ToString());
                return false;
            }
        }
    }
}
