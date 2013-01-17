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

        public string Gremlin(string script)
        {
            return Gremlin(script, null);
        }

        public string Gremlin(string script, string[] load)
        {
            var gremlin = new Gremlin(_node);

            return gremlin.Get(script, load);
        }
    }
}
