using System.Collections.Generic;
using System.Net;
using System.Reflection;

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

        internal List<T> Get<T>(string script, string[] load) where T : new()
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

            List<T> objects = new List<T>();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    foreach (var resultItem in response.JsonObject.results)
                    {
                        IDictionary<string, object> properties = resultItem as IDictionary<string, object>;
                        T obj = new T();

                        if (properties.ContainsKey("_type"))
                        {
                            switch ((string)properties["_type"])
                            {
                                case "vertex":
                                    (obj as RexsterVertex).ID = long.Parse(properties["_id"].ToString());

                                    if (properties.ContainsKey("_inE"))
                                    {
                                        foreach (string edgeID in (List<string>)properties["_inE"])
                                        {
                                            RexsterEdge edge = new RexsterEdge();
                                            edge.ID = edgeID;
                                            (obj as RexsterVertex).InEdges.Add(edge);
                                        }
                                    }

                                    if (properties.ContainsKey("_outE"))
                                    {
                                        foreach (string edgeID in (List<string>)properties["_outE"])
                                        {
                                            RexsterEdge edge = new RexsterEdge();
                                            edge.ID = edgeID;
                                            (obj as RexsterVertex).OutEdges.Add(edge);
                                        }
                                    }

                                    foreach (var property in properties)
                                    {
                                        if ((property.Key != "_id") &&
                                            (property.Key != "_type") &&
                                            (property.Key != "_inE") &&
                                            (property.Key != "_outE")) 
                                        {
                                            (obj as RexsterVertex).Properties.Add(property.Key, property.Value);
                                        }
                                    }
                                    break;
                                case "edge":
                                    (obj as RexsterEdge).ID = properties["_id"].ToString();
                                    (obj as RexsterEdge).Label = properties["_label"].ToString();

                                    if (properties.ContainsKey("_inV"))
                                    {
                                        (obj as RexsterEdge).InVertex = new RexsterVertex();
                                        (obj as RexsterEdge).InVertex.ID = long.Parse(properties["_inV"].ToString());
                                    }

                                    if (properties.ContainsKey("_outV"))
                                    {
                                        (obj as RexsterEdge).OutVertex = new RexsterVertex();
                                        (obj as RexsterEdge).OutVertex.ID = long.Parse(properties["_outV"].ToString());
                                    }

                                    foreach (var property in properties)
                                    {
                                        if ((property.Key != "_id") &&
                                            (property.Key != "_type") &&
                                            (property.Key != "_label") &&
                                            (property.Key != "_inV") &&
                                            (property.Key != "_outV"))
                                        {
                                            (obj as RexsterEdge).Properties.Add(property.Key, property.Value);
                                        }
                                    }
                                    break;
                                default:
                                    /*foreach (var property in properties)
                                    {
                                        PropertyInfo propertyInfo = typeof(T).GetProperty(property.Key);

                                        if (propertyInfo != null)
                                        {
                                            propertyInfo.SetValue(obj, property.Value, null);
                                        }
                                    }*/
                                    break;
                            }
                        }

                        objects.Add(obj);
                    }
                    break;
                default:
                    break;
            }

            return objects;
        }

        #endregion
    }
}
