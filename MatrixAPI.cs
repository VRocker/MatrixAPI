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
        private Events _events = null;

        public string UserID { get; private set; }
        public string DeviceID { get; private set; }
        public string HomeServer { get; private set; }

        private string _syncToken = "";
        public int SyncTimeout = 10000;

        public bool RunningInitialSync { get; private set; }
        public bool IsConnected { get; private set; }
        public Events Events { get => _events; set => _events = value; }

        public MatrixAPI(string Url, string token = "")
        {
            if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                throw new MatrixException("URL is not valid.");

            _backend = new HttpBackend(Url);
            _events = new Events();

            _syncToken = token;
            if (string.IsNullOrEmpty(_syncToken))
                RunningInitialSync = true;

        }

        private void FlushMessageQueue()
        {
            //throw new NotImplementedException();
        }

        public async Task ClientSync(bool connectionFailureTimeout = false)
        {
            string url = "/_matrix/client/r0/sync?timeout=" + SyncTimeout;
            if (!string.IsNullOrEmpty(_syncToken))
                url += "&since=" + _syncToken;

            var tuple = await _backend.Get(url, true);
            MatrixRequestError err = tuple.Item1;
            string response = tuple.Item2;
            if (err.IsOk)
            {
                await ParseClientSync(response);
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
        public async Task<string[]> ClientVersions()
        {
            var tuple = await _backend.Get("/_matrix/client/versions", false);
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
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

        [MatrixSpec("r0.0.1/client_server.html#post-matrix-client-r0-register")]
        public async void ClientRegister(Requests.Session.MatrixRegister registration)
        {
            var tuple = await _backend.Post("/_matrix/client/r0/register", false, Helpers.JsonHelper.Serialize(registration));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                // Parse registration response
            }
            else
                throw new MatrixException(err.ToString());
        }

        [MatrixSpec("r0.0.1/client_server.html#post-matrix-client-r0-login")]
        public async void ClientLogin(Requests.Session.MatrixLogin login)
        {
            var tuple = await _backend.Post("/_matrix/client/r0/login", false, Helpers.JsonHelper.Serialize(login));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                // We logged in!
                ParseLoginResponse(result);
            }
            else
            {
                //throw new MatrixException(err.ToString());
                Events.FireLoginFailEvent(err.ToString());
            }
        }

        public async void ClientProfile(string userId)
        {
            var tuple = await _backend.Get("/_matrix/client/r0/profile/" + userId, true);
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                Responses.UserData.UserProfileResponse profileResponse = ParseUserProfile(result);

                Events.FireUserProfileReceivedEvent(userId, profileResponse.AvatarUrl, profileResponse.DisplayName);
            }
            else
            {
                // Fire an error
            }
        }

        public async Task<bool> ClientSetDisplayName(string displayName)
        {
            Requests.UserData.UserProfileSetDisplayName req = new Requests.UserData.UserProfileSetDisplayName() { DisplayName = displayName };
            var tuple = await _backend.Put(string.Format("/_matrix/client/r0/profile/{0}/displayname", Uri.EscapeDataString(UserID)), true, Helpers.JsonHelper.Serialize(req));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ClientSetAvatar(string avatarUrl)
        {
            Requests.UserData.UserProfileSetAvatar req = new Requests.UserData.UserProfileSetAvatar() { AvatarUrl = avatarUrl };
            var tuple = await _backend.Put(string.Format("/_matrix/client/r0/profile/{0}/displayname", Uri.EscapeDataString(UserID)), true, Helpers.JsonHelper.Serialize(req));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ClientSetPresence(string presence, string statusMessage = null)
        {
            Requests.Presence.MatrixSetPresence req = new Requests.Presence.MatrixSetPresence()
            {
                Presence = presence
            };

            if (statusMessage != null)
            {
                req.StatusMessage = statusMessage;
            }

            var tuple = await _backend.Put(string.Format("/_matrix/client/r0/presence/{0}/status", Uri.EscapeDataString(UserID)), true, Helpers.JsonHelper.Serialize(req));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }
            return false;
        }

        public async Task<string> MediaUpload(string contentType, byte[] data)
        {
            var tuple = await _backend.Post("/_matrix/media/r0/upload", true, data, new Dictionary<string, string>() { { "Content-Type", contentType } });
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                // Parse response
                return ParseMediaUpload(result);
            }

            return "";
        }

        public async Task<bool> InviteToRoom(string roomId, string userId)
        {
            Requests.Rooms.MatrixRoomInvite invite = new Requests.Rooms.MatrixRoomInvite() { UserID = userId };
            var tuple = await _backend.Post(string.Format("/_matrix/client/r0/rooms/{0}/invite", System.Uri.EscapeDataString(roomId)), true, Helpers.JsonHelper.Serialize(invite));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }

            throw new MatrixException(err.ToString());
        }

        public async Task<bool> JoinRoom(string roomId)
        {
            Requests.Rooms.MatrixRoomJoin roomJoin = new Requests.Rooms.MatrixRoomJoin();
            var tuple = await _backend.Post(string.Format("/_matrix/client/r0/rooms/{0}/join", Uri.EscapeDataString(roomId)), true, Helpers.JsonHelper.Serialize(roomJoin));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> LeaveRoom(string roomId)
        {
            var tuple = await _backend.Post(string.Format("/_matrix/client/r0/rooms/{0}/leave", Uri.EscapeDataString(roomId)), true, "");
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }

            return false;
        }

        public async void RoomTypingSend(string roomId, bool typing, int timeout = 0)
        {
            Requests.Rooms.MatrixRoomSendTyping req = new Requests.Rooms.MatrixRoomSendTyping() { Typing = typing };
            if (timeout > 0)
                req.Timeout = timeout;

            var tuple = await _backend.Put(string.Format("/_matrix/client/r0/rooms/{0}/typing/{1}", Uri.EscapeDataString(roomId), Uri.EscapeDataString(UserID)), true, Helpers.JsonHelper.Serialize(req));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (!err.IsOk)
                throw new MatrixException(err.ToString());
        }
    }
}
