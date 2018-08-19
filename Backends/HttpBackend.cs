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

        public MatrixRequestError Get(string path, bool authenticate, out string result)
        {
            string apiPath = GetPath(path, authenticate);
            Task<HttpResponseMessage> task = _client.GetAsync(apiPath);
            return RequestWrap(task, out result);
        }

        public MatrixRequestError Post(string path, bool authenticate, string request, out string result)
        {
            StringContent content;
            if (!string.IsNullOrEmpty(request))
                content = new StringContent(request, Encoding.UTF8, "application/json");
            else
                content = new StringContent("{}", Encoding.UTF8, "application/json");

            string apiPath = GetPath(path, authenticate);
            Task<HttpResponseMessage> task = _client.PostAsync(apiPath, content);
            return RequestWrap(task, out result);
        }

        public MatrixRequestError Post(string path, bool authenticate, string request, Dictionary<string, string> headers, out string result)
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
            Task<HttpResponseMessage> task = _client.PostAsync(apiPath, content);
            return RequestWrap(task, out result);
        }

        public MatrixRequestError Post(string path, bool authenticate, byte[] request, Dictionary<string, string> headers, out string result)
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
            Task<HttpResponseMessage> task = _client.PostAsync(apiPath, content);
            return RequestWrap(task, out result);

        }

        public MatrixRequestError Put(string path, bool authenticate, string request, out string result)
        {
            StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

            string apiPath = GetPath(path, authenticate);
            Task<HttpResponseMessage> task = _client.PutAsync(apiPath, content);
            return RequestWrap(task, out result);
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

        private MatrixRequestError RequestWrap(Task<HttpResponseMessage> task, out string result)
        {
            try
            {
                HttpStatusCode code = GenericRequest(task, out result);
                return new MatrixRequestError("", MatrixErrorCode.CL_NONE, code);
            }
            catch (MatrixServerError e)
            {
                result = string.Empty;
                return new MatrixRequestError(e.Message, e.ErrorCode, HttpStatusCode.OK);
            }
        }

        private HttpStatusCode GenericRequest(Task<HttpResponseMessage> task, out string result)
        {
            Task<string> stask = null;
            result = string.Empty;
            try
            {
                task.Wait();
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    stask = task.Result.Content.ReadAsStringAsync();
                    stask.Wait();
                }
                else
                {
                    return task.Result.StatusCode;
                }
            }
            catch (WebException e)
            {
                throw e;
            }
            catch (AggregateException e)
            {
                throw new MatrixException(e.InnerException.Message, e.InnerException);
            }

            if (stask.Status == TaskStatus.RanToCompletion)
            {
                result = stask.Result;

                // We should probably catch json exceptions here..
                if (!task.Result.IsSuccessStatusCode)
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
            }

            return task.Result.StatusCode;
        }
    }
}
