using System;
using System.Collections.Concurrent;
using System.Threading;
using Warrock.Util;

namespace Warrock
{
    [ServerModule(InitializationStage.DataStore)]
    public sealed class Worker
    {
        public static Worker Instance { get;  set; }
		private readonly ConcurrentQueue<Action> callbacks = new ConcurrentQueue<Action>();
		private readonly Thread main;
		private int sleep = 1;
		private ulong ticksToSleep = 1500;
		public ulong TicksPerSecond { get; set; }
        public bool IsRunning { get; set; }

        public Worker()
        {
            main = new Thread(Work);
            TicksPerSecond = 0;
            IsRunning = true;
            main.Start();
            Log.WriteLine(LogLevel.Info, "Worker Initialized success");
        }

        public static bool Load()
        {
            try
            {
                Instance = new Worker();
                return true;
            }
            catch { return false; }
        }

        public void AddCallback(Action pCallback)
        {
            callbacks.Enqueue(pCallback);
        }


        public void Stop()
        {
            if (main != null)
            {
                main.Abort();
            }
        }

        private void Work()
        {
            try
            {
  
              // Zepheus.Database.DatabaseHelper.Initialize(Settings.Instance.WorldConnString, "WorkerConn");
              //  Program.Entity.Characters.Count(); //test if database is online
                Log.WriteLine(LogLevel.Info, "Database Initialized.");
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error initializing database: {0}", ex.ToString());
                return;
            }
            Action action;
            ulong last = 0;
            DateTime lastCheck = DateTime.Now;
            DateTime lastPing = DateTime.Now;
			DateTime lastGC = DateTime.Now;
			DateTime lastClientTime = DateTime.Now;
            DateTime LastMountCheck = DateTime.Now;
            for (ulong i = 0; ; i++)
            {
                if (!this.IsRunning)
                {
                    break;
                }

                try
                {
                    DateTime now = Program.CurrentTime;

                    while (callbacks.TryDequeue(out action))
                    {
                        try
                        {
                            UserWorkItem Work = new UserWorkItem(action);
                            Work.Queue();
                          //  action();
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLine(LogLevel.Exception, ex.ToString());
                        }
                    }

                    if (now.Subtract(lastCheck).TotalSeconds >= 1)
                    {
                        TicksPerSecond = i - last;
                        last = i;
                        lastCheck = now;
                        //Log.WriteLine(LogLevel.Debug, "TicksPerSecond: {0}", TicksPerSecond);
                        if (TicksPerSecond <= 100)
                        {
                            Log.WriteLine(LogLevel.Warn, "Server overload! Only {0} ticks per second!", TicksPerSecond);
                        }
                    }

                    if (now.Subtract(lastPing).TotalSeconds >= 12)
                    {
                        ClientManager.Instance.UpdatePing();
                        lastPing = now;
                    }

                    if (now.Subtract(lastGC).TotalSeconds >= 300)
                    {
                        GC.Collect();
                        lastGC = now;
                    }
                    if (now.Subtract(lastClientTime).TotalSeconds >= 60)
                    {
                     
                    }

                    if (i % ticksToSleep == 0) // No max load but most ticks to be parsed. Epic win!
                    {
                        Program.CurrentTime = DateTime.Now; // Laaast update
                        Thread.Sleep(sleep);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLine(LogLevel.Exception, "Ohgod. {0}", ex.ToString());
                }
            }
            Log.WriteLine(LogLevel.Info, "Server stopped handling callbacks.");
        }
    }
}
