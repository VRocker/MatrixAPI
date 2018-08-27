using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Backends
{
    public class HttpBackend : IMatrixAPIBackend
    {
        private string _baseUrl;
        private string _accessToken;
        private string _userId;
        private HttpClient _client;

        public HttpBackend(string apiUrl, string userId = null, HttpClient client = null)
        {
            _baseUrl = apiUrl;
            if (_baseUrl.EndsWith("/"))
                _baseUrl = _baseUrl.Substring(0, _baseUrl.Length - 1);

            _client = client ?? new HttpClient();
            _userId = userId;
        }

        public async Task<Tuple<MatrixRequestError, string>> Get(string path, bool authenticate)
        {
            string apiPath = GetPath(path, authenticate);
            HttpResponseMessage task = await _client.GetAsync(apiPath);
            return await RequestWrap(task);
        }

        public async Task<Tuple<MatrixRequestError, string>> Post(string path, bool authenticate, string request)
        {
            StringContent content;
            if (!string.IsNullOrEmpty(request))
                content = new StringContent(request, Encoding.UTF8, "application/json");
            else
                content = new StringContent("{}", Encoding.UTF8, "application/json");

            string apiPath = GetPath(path, authenticate);
            HttpResponseMessage task = await _client.PostAsync(apiPath, content);
            return await RequestWrap(task);
        }

        public async Task<Tuple<MatrixRequestError, string>> Post(string path, bool authenticate, string request, Dictionary<string, string> headers)
        {
            StringContent content;
            if (!string.IsNullOrEmpty(request))
                content = new StringContent(request, Encoding.UTF8, "application/json");
            else
                content = new StringContent("{}", Encoding.UTF8, "application/json");

            foreach(var header in headers)
            {
                content.Headers.Add(header.Key, header.Value);
            }

            string apiPath = GetPath(path, authenticate);
            HttpResponseMessage task = await _client.PostAsync(apiPath, content);
            return await RequestWrap(task);
        }

        public async Task<Tuple<MatrixRequestError, string>> Post(string path, bool authenticate, byte[] request, Dictionary<string, string> headers)
        {
            ByteArrayContent content;
            if (request != null)
                content = new ByteArrayContent(request);
            else
                content = new ByteArrayContent(new byte[0]);

            foreach (var header in headers)
            {
                content.Headers.Add(header.Key, header.Value);
            }

            string apiPath = GetPath(path, authenticate);
            HttpResponseMessage task = await _client.PostAsync(apiPath, content);
            return await RequestWrap(task);

        }

        public async Task<Tuple<MatrixRequestError, string>> Put(string path, bool authenticate, string request)
        {
            StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

            string apiPath = GetPath(path, authenticate);
            HttpResponseMessage task = await _client.PutAsync(apiPath, content);
            return await RequestWrap(task);
        }

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        private string GetPath(string apiPath, bool auth)
        {
            string path = _baseUrl + apiPath;
            if (auth)
            {
                path += (apiPath.Contains("?") ? "&" : "?") + "access_token=" + _accessToken;
                if (_userId != null)
                    path += "&user_id=" + _userId;
            }

            return path;
        }

        private async Task<Tuple<MatrixRequestError, string>> RequestWrap(HttpResponseMessage task)
        {
            try
            {
                HttpStatusCode code = task.StatusCode;
                string result = await GenericRequest(task);

                return new Tuple<MatrixRequestError, string>(
                    new MatrixRequestError("", MatrixErrorCode.CL_NONE, code),
                    result);
            }
            catch (MatrixServerError e)
            {
                return new Tuple<MatrixRequestError, string>(
                    new MatrixRequestError(e.Message, e.ErrorCode, HttpStatusCode.OK),
                    "");
            }
        }

        private async Task<string> GenericRequest(HttpResponseMessage task)
        {
            string result = await task.Content.ReadAsStringAsync();
            // We should probably catch json exceptions here..
            if (!task.IsSuccessStatusCode)
            {
                // If its not a success, parse the error code from the json
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
                {
                    var ser = new DataContractJsonSerializer(typeof(Responses.Error));
                    Responses.Error response = (ser.ReadObject(stream) as Responses.Error);
                    if (!string.IsNullOrEmpty(response.ErrorCode))
                    {
                        throw new MatrixServerError(response.ErrorCode, response.ErrorMsg);
                    }
                }
            }

            return result;
        }
    }
}
