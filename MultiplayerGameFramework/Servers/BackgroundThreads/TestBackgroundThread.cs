using MultiplayerGameFramework.Interfaces.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces;
using System.Diagnostics;
using ExitGames.Logging;
using System.Threading;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Implementation.Messaging;

namespace Servers.BackgroundThreads
{
    public class TestBackgroundThread : IBackgroundThread
    {
        private bool isRunning;
        public ILogger Log { get; set; }
        public IConnectionCollection<IClientPeer> ConnectionCollection { get; set; }

        public TestBackgroundThread(IConnectionCollection<IClientPeer> connectionCollection, ILogger log) // Include all IoC objects this thread needs i.e. IRegion, IStats, etc...
        {
            Log = log;
            ConnectionCollection = connectionCollection;
        }

        public void Run(object threadContext)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            isRunning = true;

            while(isRunning)
            {
                try
                {
                    // Check to see if there are any players - We need a list of players. If we have no players, sleep for a second and try again, keeps from chewing up the CPU
                    if(ConnectionCollection.GetPeers<IClientPeer>().Count <= 0)
                    {
                        Thread.Sleep(1000);
                        timer.Restart();
                        continue;
                    }
                    if(timer.Elapsed < TimeSpan.FromMilliseconds(5000)) // run every 5000ms - 5s
                    {
                        if(5000 - timer.ElapsedMilliseconds > 0)
                        {
                            Thread.Sleep(5000 - (int)timer.ElapsedMilliseconds);
                        }
                        continue;
                    }
                    Update(timer.Elapsed);
                    // Restart the timer so that, just in case it takes longer than 100ms, it'll start over as soon as the process finishes.
                    timer.Restart();
                }
                catch (Exception e)
                {
                    Log.ErrorFormat(string.Format("Exception occured in TestBackgroundThread.Run - {0}", e.StackTrace));
                }
            }
        }

        public void Setup(IServerApplication server)
        {
            // Do nothing in this setup. Normally used for setting up one time things in the background thread before it starts.
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Update(TimeSpan elapsed)
        {
            // Do the update here.
            Parallel.ForEach(ConnectionCollection.GetPeers<IClientPeer>(), SendUpdate);
        }

        public void SendUpdate(IClientPeer instance)
        {
            if(instance != null)
            {
                Log.DebugFormat("Sending test message to peer");
                instance.SendMessage(new Event(2, 3, new Dictionary<byte, object>()));
            }
        }
    }
}
