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

            System.Console.WriteLine("Vertices:");

            List<RexsterVertex> vertices = graph.Gremlin<RexsterVertex>("vx", new string[] { "vertices" });

            foreach (var item in vertices)
            {
                System.Console.WriteLine(item.ID);

                foreach (var prop in item.Properties)
                {
                    System.Console.WriteLine("- {0} -> {1}", prop.Key, prop.Value);
                }
            }

            System.Console.WriteLine("Edges:");

            List<RexsterEdge> edges = graph.Gremlin<RexsterEdge>("ex", new string[] { "edges" });

            foreach (var item in edges)
            {
                System.Console.WriteLine(item.ID);

                foreach (var prop in item.Properties)
                {
                    System.Console.WriteLine("- {0} -> {1}", prop.Key, prop.Value);
                }
            }

            System.Console.ReadLine();
        }
    }
}
