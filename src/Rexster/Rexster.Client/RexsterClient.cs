using System.Collections.Generic;
using System.Linq;

namespace Rexster.Client
{
    public class RexsterClient
    {
        #region Properties

        public static string DriverVersion
        {
            get { return "Alpha 1.0"; }
        }

        /// <summary>
        /// Collection of rexster nodes which consists of database connection parameters identified by unique alias string.
        /// </summary>
        public static List<RexsterNode> Nodes { get; set; }

        #endregion

        static RexsterClient()
        {
            Nodes = new List<RexsterNode>();
        }

        internal static RexsterNode GetNode(string alias)
        {
            return Nodes.Where(node => node.Alias == alias).FirstOrDefault();
        }
    }
}
