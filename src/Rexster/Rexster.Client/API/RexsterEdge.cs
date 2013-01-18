using System.Collections.Generic;

namespace Rexster.Client
{
    public class RexsterEdge
    {
        public string ID { get; set; }
        public RexsterVertex InVertex { get; set; }
        public RexsterVertex OutVertex { get; set; }
        public string Label { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public RexsterEdge()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}
