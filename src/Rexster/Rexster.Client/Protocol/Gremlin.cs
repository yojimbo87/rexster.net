using System.Collections.Generic;
using System.Net;

namespace Rexster.Client.Protocol
{
    internal class Gremlin
    {
        private string _apiUri 
        { 
            get 
            { 
                return "graphs/" + _node.Database + "/tp/gremlin"; 
            } 
        }
        private JsonParser _parser = new JsonParser();
        private RexsterNode _node { get; set; }

        internal Gremlin(RexsterNode node)
        {
            _node = node;
        }

        #region GET

        internal string Get(string script)
        {
            var request = new Request();
            request.RelativeUri = _apiUri;
            request.QueryString.Add("script", script);
            request.Method = RequestMethod.GET.ToString();

            var response = _node.Process(request);
            string data = "";

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    data = response.JsonString;
                    break;
                default:
                    break;
            }

            return data;
        }

        #endregion
    }
}
