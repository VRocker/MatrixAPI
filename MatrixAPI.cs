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

        private MatrixAppInfo _appInfo = null;

        public string UserID { get; private set; }
        public string DeviceID { get; private set; }
        public string HomeServer { get; private set; }
        public string DeviceName { get; private set; } = "libMatrix";

        private string _syncToken = "";
        public int SyncTimeout = 10000;

        public bool RunningInitialSync { get; private set; }
        public bool IsConnected { get; private set; }
        public Events Events { get => _events; set => _events = value; }
        public MatrixAppInfo AppInfo { get => _appInfo; }

        public MatrixAPI(string Url, string token = "")
        {
            if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                throw new MatrixException("URL is not valid.");

            _backend = new HttpBackend(Url);
            _events = new Events();
            _appInfo = new MatrixAppInfo();

            _syncToken = token;
            if (string.IsNullOrEmpty(_syncToken))
                RunningInitialSync = true;

        }

        private void FlushMessageQueue()
        {
            //throw new NotImplementedException();
        }

        public void SetDeviceName(string _deviceName)
        {
            DeviceName = _deviceName;
        }
        public async Task ClientSync(bool connectionFailureTimeout = false, bool fullState = false)
        {
            string url = "/_matrix/client/r0/sync?timeout=" + SyncTimeout;
            if (!string.IsNullOrEmpty(_syncToken))
                url += "&since=" + _syncToken;
            if (fullState)
                url += "&full_state=true";

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

        public string GetMediaDownloadUri(string contentUrl)
        {
            if (!contentUrl.StartsWith("mxc://"))
                return string.Empty;

            var newUrl = contentUrl.Remove(0, 6);
            var contentUrlSplit = newUrl.Split('/');
            if (contentUrlSplit.Count() < 2)
                return string.Empty;

            string uriPath = _backend.GetPath(string.Format("/_matrix/media/r0/download/{0}/{1}", contentUrlSplit[0], contentUrlSplit[1]), false);
            return uriPath;
        }

        public async void JoinedRooms()
        {
            var tuple = await _backend.Get("/_matrix/client/r0/joined_rooms", true);
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                // Parse joined rooms
                ParseJoinedRooms(result);
            }
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

        public async Task<bool> CreateRoom(string roomName, string roomTopic, bool isDirect = false)
        {
            if (string.IsNullOrEmpty(roomName))
                return false;

            Requests.Rooms.MatrixRoomCreate roomCreate = new Requests.Rooms.MatrixRoomCreate
            {
                Name = roomName,
                IsDirect = isDirect
            };
            if (string.IsNullOrEmpty(roomTopic))
                roomCreate.Topic = roomTopic;

            var tuple = await _backend.Post("/_matrix/client/r0/createRoom", true, Helpers.JsonHelper.Serialize(roomCreate));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                try
                {
                    ParseCreatedRoom(result);
                    return true;
                }
                catch (MatrixException)
                {
                    // Failed to create the room
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> AddRoomAlias(string roomId, string alias)
        {
            Requests.Rooms.MatrixRoomAddAlias roomAddAlias = new Requests.Rooms.MatrixRoomAddAlias
            {
                RoomID = roomId
            };

            var tuple = await _backend.Put(string.Format("/_matrix/client/r0/directory/room/{0}", Uri.EscapeDataString(alias)), true, Helpers.JsonHelper.Serialize(roomAddAlias));
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRoomAlias(string roomAlias)
        {
            var tuple = await _backend.Delete(string.Format("/_matrix/client/r0/directory/room/{0}", Uri.EscapeDataString(roomAlias)), true);
            MatrixRequestError err = tuple.Item1;
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

        public async Task<bool> GetRoomState(string roomId, string eventType = null, string stateKey = null)
        {
            string url = string.Format("/_matrix/client/r0/rooms/{0}/state", roomId);
            if (!string.IsNullOrEmpty(eventType))
                url += "/" + eventType;
            if (!string.IsNullOrEmpty(stateKey))
                url += "/" + stateKey;

            var tuple = await _backend.Get(url, true);
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
            {
                // Parse stuff
                // Parsing will differ if there is no eventType specified

                if (!string.IsNullOrEmpty(eventType))
                {

                }
                else
                {

                }

                return true;
            }

            return false;
        }

        private async Task<bool> SendEventToRoom(string roomId, string eventType, string content)
        {
            var tuple = await _backend.Put(string.Format("/_matrix/client/r0/rooms/{0}/send/{1}/{2}", Uri.EscapeDataString(roomId), eventType, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()), true, content);
            MatrixRequestError err = tuple.Item1;
            string result = tuple.Item2;
            if (err.IsOk)
                return true;

            return false;
        }
        public async Task<bool> SendTextMessageToRoom(string roomId, string message)
        {
            Requests.Rooms.Message.MatrixRoomMessageText req = new Requests.Rooms.Message.MatrixRoomMessageText()
            {
                Body = message
            };

            return await SendEventToRoom(roomId, "m.room.message", Helpers.JsonHelper.Serialize(req));
        }

        public async Task<bool> SendLocationToRoom(string roomId, string description, double lat, double lon)
        {
            StringBuilder sb = new StringBuilder("geo:");
            sb.Append(lat);
            sb.Append(",");
            sb.Append(lon);
            Requests.Rooms.Message.MatrixRoomMessageLocation req = new Requests.Rooms.Message.MatrixRoomMessageLocation()
            {
                Description = description,
                GeoUri = sb.ToString()
            };

            return await SendEventToRoom(roomId, "m.room.message", Helpers.JsonHelper.Serialize(req));
        }
    }
}
