using libMatrix.Responses.Pushers;
using System;

namespace libMatrix
{
    public partial class Events
    {
        public class NotificationEventArgs : EventArgs
        {
            public Notification Notification { get; set; }
        }

        public delegate void NotificationEventDelegate(object sender, NotificationEventArgs e);

        public event NotificationEventDelegate NotificationEvent;

        internal void FireNotificationEvent(Notification notif) => NotificationEvent?.Invoke(this, new NotificationEventArgs() { Notification = notif });
    }
}
