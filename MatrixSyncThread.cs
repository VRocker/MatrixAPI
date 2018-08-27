using System;
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

            _pollThread = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                if (!isRunningSync)
                {
                    isRunningSync = true;
                    try
                    {


                        ClientSync(true);

                    }
                    catch (Exception e)
                    {


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
