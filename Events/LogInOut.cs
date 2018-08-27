using System;

namespace libMatrix
{
    public partial class Events
    {
        public class LoginEventArgs : EventArgs
        {

        }

        public delegate void LoginFailDelegate(object sender, ErrorEventArgs e);
        public delegate void LoginDelegate(object sender, LoginEventArgs e);
        public delegate void LogoutDelegate(object sender, LoginEventArgs e);

        public event LoginFailDelegate LoginFailEvent;
        public event LoginDelegate LoginEvent;
        public event LogoutDelegate LogoutEvent;

        internal void FireLoginFailEvent(string message) => LoginFailEvent?.Invoke(this, new ErrorEventArgs() { Message = message });
        internal void FireLoginEvent() => LoginEvent?.Invoke(this, new LoginEventArgs() { });
        internal void FireLogoutEvent() => LogoutEvent?.Invoke(this, new LoginEventArgs() { });
    }
}
