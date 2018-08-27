using libMatrix.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix
{
    public partial class Events
    {
        public class RoomJoinEventArgs : EventArgs
        {
            public string Room { get; set; }
            public MatrixEventRoomJoined Event { get; set; }
        }

        public class RoomInviteEventArgs : EventArgs
        {
            public string Room { get; set; }
            public MatrixEventRoomInvited Event { get; set; }
        }

        public class RoomLeaveEventArgs : EventArgs
        {
            public string Room { get; set; }
            public MatrixEventRoomLeft Event { get; set; }
        }

        public delegate void RoomJoinEventDelegate(object sender, RoomJoinEventArgs e);
        public delegate void RoomInviteEventDelegate(object sender, RoomInviteEventArgs e);
        public delegate void RoomLeaveEventDelegate(object sender, RoomLeaveEventArgs e);

        public event RoomJoinEventDelegate RoomJoinEvent;
        public event RoomInviteEventDelegate RoomInviteEvent;
        public event RoomLeaveEventDelegate RoomLeaveEvent;

        internal void FireRoomJoinEvent(string room, MatrixEventRoomJoined evt) => RoomJoinEvent?.Invoke(this, new RoomJoinEventArgs() { Room = room, Event = evt });
        internal void FireRoomInviteEvent(string room, MatrixEventRoomInvited evt) => RoomInviteEvent?.Invoke(this, new RoomInviteEventArgs() { Room = room, Event = evt });
        internal void FireRoomLeaveEvent(string room, MatrixEventRoomLeft evt) => RoomLeaveEvent?.Invoke(this, new RoomLeaveEventArgs() { Room = room, Event = evt });
    }
}
