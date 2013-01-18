using System.Collections.Generic;
using Rexster.Client.Protocol;

namespace Rexster.Client
{
    public class RexsterGraph
    {
        #region Properties

        private RexsterNode _node;

        #endregion

        public RexsterGraph(string alias)
        {
            _node = RexsterClient.GetNode(alias);
        }

        public List<T> Gremlin<T>(string script) where T : new()
        {
            return Gremlin<T>(script, null);
        }

        public List<T> Gremlin<T>(string script, string[] load) where T : new()
        {
            var gremlin = new Gremlin(_node);

            return gremlin.Get<T>(script, load);
        }
    }
}
