using libMatrix.Backends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix
{
    public partial class MatrixAPI
    {
        public const string VERSION = "r0.0.1";
        IMatrixAPIBackend _backend = null;

        public string UserID { get; private set; }
        public string DeviceID { get; private set; }
        public string HomeServer { get; private set; }

        private string _syncToken = "";
        public int SyncTimeout = 10000;

        public bool RunningInitialSync { get; private set; }
        public bool IsConnected { get; private set; }

        public MatrixAPI(string Url, string token = "")
        {
            if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                throw new MatrixException("URL is not valid.");

            _backend = new HttpBackend(Url);

            _syncToken = token;
            if (string.IsNullOrEmpty(_syncToken))
                RunningInitialSync = true;

        }

        private void FlushMessageQueue()
        {
            //throw new NotImplementedException();
        }

        public void ClientSync(bool connectionFailureTimeout = false)
        {
            string url = "/_matrix/client/r0/sync?timeout=" + SyncTimeout;
            if (!string.IsNullOrEmpty(_syncToken))
                url += "&since=" + _syncToken;

            MatrixRequestError err = _backend.Get(url, true, out string response);
            if (err.IsOk)
            {
                ParseClientSync(response);
            }
            else if (connectionFailureTimeout)
            {

            }

            if (RunningInitialSync)
            {
                // Fire an event to say sync has been done

                RunningInitialSync = false;
            }
        }

        [MatrixSpec("r0.0.1/client_server.html#get-matrix-client-versions")]
        public string[] ClientVersions()
        {
            MatrixRequestError err = _backend.Get("/_matrix/client/versions", false, out string result);
            if (err.IsOk)
            {
                // Parse the version request

                return ParseClientVersions(result);
            }
            else
            {
                throw new MatrixException("Failed to validate version.");
            }
        }

        [MatrixSpec("r0.0.1/client_server.html#post-matrix-client-r0-login")]
        public void ClientLogin(Requests.Session.MatrixLogin login)
        {
            MatrixRequestError err = _backend.Post("/_matrix/client/r0/login", false, Helpers.JsonHelper.Serialize(login), out string result);
            if (err.IsOk)
            {
                // We logged in!
                ParseLoginResponse(result);
            }
            else
            {
                throw new MatrixException(err.ToString());
            }
        }

        public Responses.UserData.UserProfileResponse ClientProfile(string userId)
        {
            MatrixRequestError err = _backend.Get("/_matrix/client/r0/profile/" + userId, true, out string result);
            if (err.IsOk)
            {
                return ParseUserProfile(result);
            }
            else
            {
                return null;
            }
        }

        public bool InviteToRoom(string roomId, string userId)
        {
            Requests.Rooms.MatrixRoomInvite invite = new Requests.Rooms.MatrixRoomInvite() { UserID = userId };
            MatrixRequestError err = _backend.Post(string.Format("/_matrix/client/r0/rooms/{0}/invite", System.Uri.EscapeDataString(roomId)), true, Helpers.JsonHelper.Serialize(invite), out string result);
            if (err.IsOk)
            {
                return true;
            }

            throw new MatrixException(err.ToString());
        }
    }
}
