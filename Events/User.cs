using System;

namespace libMatrix
{
    public partial class Events
    {
        public class UserProfileEventArgs : EventArgs
        {
            public string UserID;
            public string AvatarUrl;
            public string DisplayName;
        }

        public delegate void UserProfileDelegate(object sender, UserProfileEventArgs e);

        public event UserProfileDelegate UserProfileEvent;

        internal void FireUserProfileReceivedEvent(string userId, string avatarUrl, string displayName) => UserProfileEvent?.Invoke(this, new UserProfileEventArgs() { UserID = userId, AvatarUrl = avatarUrl, DisplayName = displayName });
    }
}
