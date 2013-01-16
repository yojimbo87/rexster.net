using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rexster.Client;

namespace Rexster.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RexsterNode node = new RexsterNode(
                "localhost",
                8182,
                "titanexample",
                "",
                "",
                "test"
            );

            RexsterClient.Nodes.Add(node);

            RexsterGraph graph = new RexsterGraph("test");

            System.Console.WriteLine(graph.Gremlin("g.V"));

            System.Console.ReadLine();
        }
    }
}
