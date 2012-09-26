using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Util
{
    public class Config
    {
        public static Config Instance { get; set; }
        //base Variabels
        public byte GameServerID { get; set; }
        public string GameServerIP = "127.0.0.1";
        public int GameServerPort { get; set; }
        public ushort GameInterServerPort = 1000;

        public string LoginServerIP = "127.0.0.1";
        public int LoginServerPort = 5330;
        public ushort LoginInterServerPort = 1000;

        public string InterServerPassword = "LOL";

        //Login
        public string LoginMysqlServer = "127.0.0.1";
        public int LoginMysqlPort = 3306;
        public string LoginMysqlUser = "root";
        public string LoginMysqlPassword = "test";
        public string LoginMysqlDatabase = "testdatabase";
        public uint LoginDBMinPoolSize = 10;
        public uint LoginDBMaxPoolSize = 20;

        //Game
        public string GameMysqlServer = "127.0.0.1";
        public int GameMysqlPort = 3306;
        public string GameMysqlUser = "root";
        public string GameMysqlPassword = "test";
        public string GameMysqlDatabase = "testdatabase";
        public uint GameDBMinPoolSize = 10;
        public uint GameDBMaxPoolSize = 20;

        public int OverloadFlags = 1;
        public int QuerCachePerClient = 2;

        public  bool LoadConfig()
        {
            if (!File.Exists(@"Config.xml"))
            {
                Log.WriteLine(LogLevel.Info, "Config File Not Found Will you Generate ConfigFiele from Programm? N/Y");
                string answer = Console.ReadLine();
                if (answer == "Y")
                {
                   Config.Instance.WriteXML();
                    return false;
                }
                else
                {
                    Log.WriteLine(LogLevel.Info, "Please Restart you Application");
                    return false;
                }
            }
            else
            {
                Config.Instance.ReadXML();
                return true;
            }
        }
        public void WriteXML()
        {

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Config));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                @"Config.xml");
            writer.Serialize(file, this);
            file.Close();
        }
        public Config()
        {
        }
        public void ReadXML()
        {
            if (File.Exists(@"Config.xml"))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Config));
                System.IO.StreamReader file = new System.IO.StreamReader(@"Config.xml");
                Config cfg = (Config)reader.Deserialize(file);
                Config.Instance = cfg;
                file.Close();
            }
        }
    }
}
