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

            //List<RexsterVertex> vertices = graph.Gremlin<RexsterVertex>("vx", new string[] { "vertices" });
            long total = 0;

            for (int i = 0; i < 50; i++)
            {
                long tps = Do(graph);
                total += tps;

                System.Console.WriteLine("TPS: " + tps);
            }

            System.Console.WriteLine("Average: " + total / 50);

            /*foreach (var item in vertices)
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
            }*/

            System.Console.ReadLine();
        }

        static long Do(RexsterGraph graph)
        {
            DateTime start = DateTime.Now;
            bool running = true;
            long tps = 0;

            do
            {
                List<RexsterVertex> vertices = graph.Gremlin<RexsterVertex>("vx", new string[] { "vertices" });
                tps++;

                TimeSpan dif = DateTime.Now - start;

                if (dif.TotalMilliseconds > 1000)
                {
                    running = false;
                }
            }
            while (running);

            return tps;

            /*for (int i = 0; i < 1000; i++)
            {
                List<RexsterVertex> vertices = graph.Gremlin<RexsterVertex>("vx", new string[] { "vertices" });
            }

            //Parallel.For(0, 1000, i =>
            //{
            //    List<RexsterVertex> vertices = graph.Gremlin<RexsterVertex>("vx", new string[] { "vertices" });
            //});

            TimeSpan dif = DateTime.Now - start;

            System.Console.WriteLine("Time: " + dif.TotalSeconds);*/
        }
    }
}
