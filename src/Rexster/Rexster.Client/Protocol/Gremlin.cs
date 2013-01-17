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

        internal string Get(string script, string[] load)
        {
            var request = new Request();
            request.RelativeUri = _apiUri;
            request.Method = RequestMethod.GET.ToString();

            request.QueryString.Add("script", script);

            if ((load != null) && (load.Length > 0))
            {
                request.QueryString.Add("load", "[" + string.Join(",", load) + "]");
            }

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
