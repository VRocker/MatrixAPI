using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix
{
    public partial class MatrixAPI
    {
        private string[] ParseClientVersions(string resp)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(resp)))
                {
                    var ser = new DataContractJsonSerializer(typeof(Responses.VersionResponse));
                    Responses.VersionResponse response = (ser.ReadObject(stream) as Responses.VersionResponse);

                    return response.Versions;
                }
            }
            catch
            {
                throw new MatrixException("Failed to parse ClientVersions");
            }
        }

        private void ParseLoginResponse(string resp)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(resp)))
                {
                    var ser = new DataContractJsonSerializer(typeof(Responses.Session.LoginResponse));
                    Responses.Session.LoginResponse response = (ser.ReadObject(stream) as Responses.Session.LoginResponse);

                    this.UserID = response.UserID;
                    this.DeviceID = response.DeviceID;
                    this.HomeServer = response.HomeServer;

                    this._backend.SetAccessToken(response.AccessToken);

                    _events.FireLoginEvent();
                }
            }
            catch
            {
                throw new MatrixException("Failed to parse LoginResponse");
            }
        }

        private Responses.UserData.UserProfileResponse ParseUserProfile(string resp)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(resp)))
                {
                    var ser = new DataContractJsonSerializer(typeof(Responses.UserData.UserProfileResponse));
                    Responses.UserData.UserProfileResponse response = (ser.ReadObject(stream) as Responses.UserData.UserProfileResponse);

                    return response;
                }
            }
            catch
            {
                return null;
                //throw new MatrixException("Failed to parse UserProfile");
            }
        }

        private void ParseClientSync(string resp)
        {
            try
            {
                //using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(resp)))
                {
                    /*DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;

                    var ser = new DataContractJsonSerializer(typeof(Responses.MatrixSync), settings);
                    Responses.MatrixSync response = (ser.ReadObject(stream) as Responses.MatrixSync);*/

                    Responses.MatrixSync response = Newtonsoft.Json.JsonConvert.DeserializeObject<Responses.MatrixSync>(resp);

                    _syncToken = response.NextBatch;

                    foreach (var room in response.Rooms.Join)
                    {
                        // Fire event for room joined
                        _events.FireRoomJoinEvent(room.Key, room.Value);
                    }

                    foreach (var room in response.Rooms.Invite)
                    {
                        // Fire event for invite
                        _events.FireRoomInviteEvent(room.Key, room.Value);
                    }

                    foreach (var room in response.Rooms.Leave)
                    {
                        // Fire event for room leave
                        _events.FireRoomLeaveEvent(room.Key, room.Value);
                    }

                    if (response.Presense != null)
                    {
                        foreach (var evt in response.Presense.Events)
                        {
                            var actualEvent = evt as Responses.MatrixEventsPresence;
                            bool active = actualEvent.Content.CurrentlyActive;
                        }
                    }

                    // Do stuff
                    IsConnected = true;
                }
            }
            catch (Exception e)
            {
                throw new MatrixException("Failed to parse ClientSync - " + e.Message);
            }
        }
            catch
            {
                throw new MatrixException("Failed to parse ParseClientSync");
            }
        }
    }
}
