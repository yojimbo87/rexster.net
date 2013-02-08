using System.Dynamic;
using System.Net;

namespace Rexster.Client.Protocol
{
    internal class Response
    {
        internal HttpStatusCode StatusCode { get; set; }
        internal WebHeaderCollection Headers { get; set; }
        internal string JsonString { get; set; }
        internal Json JsonObject { get; set; }

        internal Response()
        {
            JsonObject = new Json();
        }
    }
}
