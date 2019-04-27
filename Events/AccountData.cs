using libMatrix.Responses;
using System;

namespace libMatrix
{
    public partial class Events
    {
        public class AccountDataEventArgs : EventArgs
        {
            public MatrixEvents Event;
        }

        public delegate void AccountDataDelegate(object sender, AccountDataEventArgs e);

        public event AccountDataDelegate AccountDataEvent;

        internal void FireAccountDataEvent(MatrixEvents evt) => AccountDataEvent?.Invoke(this, new AccountDataEventArgs() { Event = evt });
    }
}
