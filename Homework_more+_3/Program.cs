public class Vertex
{
    public int Name { get; set; }
    public Vertex[] Children { get; set; }
    public Dictionary<Vertex, int> Distances { get; set; }
    public int Value { get; set; }
    public Vertex? Parent { get; set; }
    public bool Processed { get; set; }

    public Vertex(int name)
    {
        Name = name;
        Value = int.MaxValue;
    }
}

static class Program
{
    static public Vertex FindCurrentVertex(Vertex[] graph)
    {
        HashSet<Vertex> visited = new HashSet<Vertex>();

        int minDist = int.MaxValue;
        Vertex currentVertex = null;

        foreach (Vertex item in graph)
        {
            if (item.Processed) visited.Add(item);
            if (visited.Contains(item)) continue;
            if (item.Value < minDist)
            {
                minDist = item.Value;
                currentVertex = item;
            }
            visited.Add(item);
        }
        return currentVertex;
    }

    static void Main()
    {
        Vertex v1 = new Vertex(1);
        Vertex v2 = new Vertex(2);
        Vertex v3 = new Vertex(3);
        Vertex v4 = new Vertex(4);
        Vertex v5 = new Vertex(5);
        Vertex v6 = new Vertex(6);

        v1.Children = new Vertex[] { v2, v4, v6 };
        v1.Distances = new Dictionary<Vertex, int>() { { v2, 3 }, { v4, 16 }, { v6, 9 } };

        v2.Children = new Vertex[] { v1, v3, v6 };
        v2.Distances = new Dictionary<Vertex, int>() { { v1, 3 }, { v3, 6 }, { v6, 8 } };

        v3.Children = new Vertex[] { v2, v5, v6 };
        v3.Distances = new Dictionary<Vertex, int>() { { v2, 6 }, { v5, 11 }, { v6, 5 } };

        v4.Children = new Vertex[] { v1, v5, v6 };
        v4.Distances = new Dictionary<Vertex, int>() { { v1, 16 }, { v5, 12 }, { v6, 7 } };

        v5.Children = new Vertex[] { v3, v4, v6 };
        v5.Distances = new Dictionary<Vertex, int>() { { v3, 11 }, { v4, 12 }, { v6, 4 } };

        v6.Children = new Vertex[] { v1, v2, v3, v4, v5 };
        v6.Distances = new Dictionary<Vertex, int>() { { v1, 9 }, { v2, 8 }, { v3, 5 }, { v4, 7 }, { v5, 4 } };

        Vertex[] graph = new Vertex[] { v1, v2, v3, v4, v5, v6 };


        Vertex start = graph[0];
        foreach (var item in graph)
        {
            start = start.Name > item.Name ? item : start;
        }
        start.Value = 0;
        start.Parent = start;

        Vertex current = start;
        int lgraph = graph.Length;
        for (int i = 0; i < lgraph; i++)
        {
            if (current is null) continue;

            if (current.Processed) continue;

            foreach (Vertex child in current.Children)
            {
                int d = current.Value + current.Distances[child];
                if (d < child.Value)
                {
                    child.Value = d;
                    child.Parent = current;
                }
            }
            current.Processed = true;
            current = FindCurrentVertex(graph);
        }

        int[] processed = new int[6];
        int[] dist = new int[6];
        int[] parent = new int[6];
        int[,] matrix = new int[4, 6];

        for (int i = 0; i < lgraph; i++)
        {
            processed[i] = graph[i].Processed ? 1 : 0;
            dist[i] = graph[i].Value;
            parent[i] = graph[i].Parent is null ? 0 : graph[i].Parent.Name;
        }

        for (int i = 0; i < 6; i++)
        {
            matrix[0, i] = graph[i].Name;
            matrix[1, i] = processed[i];
            matrix[2, i] = dist[i];
            matrix[3, i] = parent[i];
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}