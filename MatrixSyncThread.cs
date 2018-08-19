using System;
using Windows.System.Threading;

namespace libMatrix
{
    public partial class MatrixAPI
    {
        private ThreadPoolTimer _pollThread;

        public void StartSyncThreads()
        {
            TimeSpan period = TimeSpan.FromMilliseconds(250);

            _pollThread = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                try
                {
                    ClientSync(true);
                }
                catch (Exception e)
                {


                }

                FlushMessageQueue();
            }, period);
        }

        public void StopSyncThreads()
        {
            _pollThread.Cancel();

            FlushMessageQueue();
        }
    }
}
