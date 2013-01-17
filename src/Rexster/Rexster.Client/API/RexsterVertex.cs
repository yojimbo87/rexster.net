using System.Collections.Generic;

namespace Rexster.Client
{
    public class RexsterVertex
    {
        public long ID { get; set; }
        public List<RexsterEdge> InEdges { get; set; }
        public List<RexsterEdge> OutEdges { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public RexsterVertex()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}
