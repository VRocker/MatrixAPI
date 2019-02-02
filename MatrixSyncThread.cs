using System;
using System.Diagnostics;
using Windows.System.Threading;

namespace libMatrix
{
    public partial class MatrixAPI
    {
        private ThreadPoolTimer _pollThread;

        private bool isRunningSync = false;

        public void StartSyncThreads()
        {
            TimeSpan period = TimeSpan.FromMilliseconds(250);

            _pollThread = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                if (!isRunningSync)
                {
                    isRunningSync = true;
                    try
                    {
                        await ClientSync(true);
                    }
                    catch (Exception e)
                    {

                        Debug.WriteLine("Sync Exception: " + e.Message);
                    }

                    FlushMessageQueue();

                    isRunningSync = false;
                }
            }, period);
        }

        public void StopSyncThreads()
        {
            _pollThread.Cancel();

            FlushMessageQueue();
        }
    }
}
