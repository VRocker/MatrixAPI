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
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(resp)))
                {
                    var ser = new DataContractJsonSerializer(typeof(Responses.MatrixSync));
                    Responses.MatrixSync response = (ser.ReadObject(stream) as Responses.MatrixSync);

                    _syncToken = response.NextBatch;

                    // Do stuff
                    IsConnected = true;
                }
            }
            catch
            {
                throw new MatrixException("Failed to parse ParseClientSync");
            }
        }
    }
}
