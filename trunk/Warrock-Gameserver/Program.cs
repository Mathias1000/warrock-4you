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

namespace Warrock
{
    public class Program
    {
        public static DateTime CurrentTime { get; set; }
        public static DatabaseManager DatabaseManager;
        public static cUDPServers sockUdpServers = new cUDPServers();
        public static string LoginDbConnectionString { get; set; }

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
                    switch (arguments[0])
                    {
                        case "shutdown":
                            break;
                    }
                }
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
            // so the startup works
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
#endif
            #region MysqStingBuild
            MySqlConnectionStringBuilder ConnString = new MySqlConnectionStringBuilder();//generate string for LoginDatabase action
            ConnString.Database = Config.Instance.LoginMysqlDatabase;
            ConnString.UserID = Config.Instance.LoginMysqlUser;
            ConnString.Server = Config.Instance.LoginMysqlServer;
            ConnString.MinimumPoolSize = Config.Instance.LoginDBMinPoolSize;
            ConnString.MaximumPoolSize = Config.Instance.LoginDBMaxPoolSize;
            ConnString.Password = Config.Instance.LoginMysqlPassword;
            ConnString.Port = (uint)Config.Instance.LoginMysqlPort;
            LoginDbConnectionString = ConnString.ToString();
            #endregion
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
