using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Rexster.Client.Protocol;

namespace Rexster.Client
{
    public class RexsterNode
    {
        private string _userAgent = "Rexster.net/alpha";

        #region Properties

        /// <summary>
        /// Node host name or IP address (without http(s) prefix, e.g. rexster.example.com).
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Node port number.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Name of the graph database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Name of the user which will be used for authentication.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password which will be used for authentication.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Alias which will uniquely identify specified database connection.
        /// </summary>
        public string Alias { get; set; }

        internal Uri BaseUri { get; set; }

        internal CredentialCache Credentials { get; set; }

        #endregion

        /// <summary>
        /// Creates instance of ArangoDB node object which holds database connection parameters identified by unique alias string.
        /// </summary>
        /// <param name="server">Node host name or IP address (without http(s) prefix, e.g. arango.example.com).</param>
        /// <param name="port">Node port number.</param>
        /// <param name="userName">Name of the user which will be used for authentication.</param>
        /// <param name="password">Password which will be used for authentication.</param>
        /// <param name="alias">Alias which will uniquely identify specified database connection.</param>
        public RexsterNode(string server, int port, string database, string userName, string password, string alias)
        {
            Server = server;
            Port = port;
            Database = database;
            Username = userName;
            Password = password;
            Alias = alias;

            BaseUri = new Uri("http://" + server + ":" + port + "/");
            
            Credentials = new CredentialCache();
            Credentials.Add(BaseUri, "Basic", new NetworkCredential(userName, password));
        }

        internal Response Process(Request request)
        {
            var httpRequest = (HttpWebRequest)HttpWebRequest.Create(BaseUri + request.RelativeUri);
            httpRequest.KeepAlive = true;
            httpRequest.Method = request.Method;
            httpRequest.UserAgent = _userAgent;

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                httpRequest.Credentials = Credentials;
            }

            if ((request.Headers.Count > 0))
            {
                httpRequest.Headers = request.Headers;
            }

            if (!string.IsNullOrEmpty(request.Body))
            {
                byte[] data = Encoding.UTF8.GetBytes(request.Body);

                Stream stream = httpRequest.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            else
            {
                httpRequest.ContentLength = 0;
            }

            var response = new Response();

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                var reader = new StreamReader(httpResponse.GetResponseStream());

                response.StatusCode = httpResponse.StatusCode;
                response.Headers = httpResponse.Headers;
                response.JsonString = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(response.JsonString))
                {
                    response.JsonObject.Load(response.JsonString);
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                var reader = new StreamReader(httpResponse.GetResponseStream());

                var jsonString = reader.ReadToEnd();
                

                throw new Exception(
                    webException.Message,
                    webException.InnerException
                );

                /*if ((httpResponse.StatusCode == HttpStatusCode.NotModified) ||
                    ((httpResponse.StatusCode == HttpStatusCode.NotFound) && (request.Method == RequestMethod.HEAD.ToString())))
                {
                    response.StatusCode = httpResponse.StatusCode;
                    response.Headers = httpResponse.Headers;
                    response.JsonString = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(response.JsonString))
                    {
                        response.JsonObject = parser.Deserialize(response.JsonString);
                    }
                }
                else
                {
                    var jsonString = reader.ReadToEnd();
                    dynamic jsonObject;
                    string errorMessage = "";

                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        jsonObject = parser.Deserialize(reader.ReadToEnd());
                        errorMessage = string.Format("ArangoDB responded with error code {0}:\n{1} [error number {2}]", jsonObject.code, jsonObject.errorMessage, jsonObject.errorNum);
                    }

                    throw new ArangoException(
                        httpResponse.StatusCode,
                        errorMessage,
                        webException.Message,
                        webException.InnerException
                    );
                }*/
            }

            return response;
        }
    }
}
