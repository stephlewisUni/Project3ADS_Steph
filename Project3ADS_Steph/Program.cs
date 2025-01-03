using System;
using System.Collections.Generic;

class Graph
{
    public int NodesCount { get; set; }
    public List<int>[] AdjList { get; set; } //un weighted
    public List<Tuple<int, int>>[] WeightedAdjList { get; set; } // weighted

    public Graph(int nodesCount, bool isWeighted = false)
    {
        NodesCount = nodesCount;

        if (isWeighted)
        {
            WeightedAdjList = new List<Tuple<int, int>>[nodesCount];
            for (int i = 0; i < nodesCount; i++)
            {
                WeightedAdjList[i] = new List<Tuple<int, int>>();
            }
        }
        else
        {
            AdjList = new List<int>[nodesCount];
            for (int i = 0; i < nodesCount; i++)
            {
                AdjList[i] = new List<int>();
            }
        }
    }

    
    public void AddEdge(int from, int to)
    {
        AdjList[from].Add(to);
        AdjList[to].Add(from); 
    }

  
    public void AddWeightedEdge(int from, int to, int weight)
    {
        WeightedAdjList[from].Add(new Tuple<int, int>(to, weight));
        WeightedAdjList[to].Add(new Tuple<int, int>(from, weight)); 
    }

  
    public List<int> GetNeighbours(int node)
    {
        return AdjList[node];
    }

    
    public List<Tuple<int, int>> GetWeightedNeighbours(int node)
    {
        return WeightedAdjList[node];
    }
}

class AlgorithmCalculator
{
    // BFS 
    public static double[] ScoreCalcBFS(Graph G, int n)
    {
        double[] influenceScores = new double[n];

        
        for (int u = 0; u < n; u++)
        {
            Queue<int> Q = new Queue<int>();
            int[] distances = new int[n];
            for (int i = 0; i < n; i++)
            {
                distances[i] = -1;
            }

            Q.Enqueue(u);
            distances[u] = 0;

            while (Q.Count > 0)
            {
                int v = Q.Dequeue();

                foreach (int w in G.GetNeighbours(v))
                {
                    if (distances[w] == -1)
                    {
                        Q.Enqueue(w);
                        distances[w] = distances[v] + 1;
                    }
                }
            }

            int sumOfDistances = 0;
            for (int i = 0; i < n; i++)
            {
                if (distances[i] != -1)
                {
                    sumOfDistances += distances[i];
                }
            }

            if (sumOfDistances != 0)
            {
                influenceScores[u] = (double)(n - 1) / sumOfDistances;
            }
            else
            {
                influenceScores[u] = 0;
            }
        }

        return influenceScores;
    }

    // Dijkstra
    public static double[] ScoreCalcDijkstra(Graph G, int n)
    {
        double[] influenceScores = new double[n];

        
        for (int u = 0; u < n; u++)
        {
            PriorityQueue<int, int> PQ = new PriorityQueue<int, int>();
            int[] distances = new int[n];
            for (int i = 0; i < n; i++)
            {
                distances[i] = int.MaxValue; 
            }

            PQ.Enqueue(u, 0);
            distances[u] = 0;

            while (PQ.Count > 0)
            {
                int v = PQ.Dequeue();

                foreach (var neighbour in G.GetWeightedNeighbours(v))
                {
                    int w = neighbour.Item1;
                    int weight = neighbour.Item2;

                    if (distances[v] + weight < distances[w])
                    {
                        distances[w] = distances[v] + weight;
                        PQ.Enqueue(w, distances[w]);
                    }
                }
            }

            int sumOfDistances = 0;
            for (int i = 0; i < n; i++)
            {
                if (distances[i] != int.MaxValue)
                {
                    sumOfDistances += distances[i];
                }
            }

            if (sumOfDistances != 0)
            {
                influenceScores[u] = (double)(n - 1) / sumOfDistances;
            }
            else
            {
                influenceScores[u] = 0;
            }
        }

        return influenceScores;
    }
}

class Program
{
    static void Main()
    {
        //BFS
        //0 Alice
        //1 Britney
        //2 Claire
        //3 Diana
        //4 Edward
        //5 Gloria
        //6 Harry
        //7 Fred
        int nUnweighted = 8;
        Graph unweightedGraph = new Graph(nUnweighted);

        unweightedGraph.AddEdge(0, 1);
        unweightedGraph.AddEdge(1, 2);
        unweightedGraph.AddEdge(2, 3);
        unweightedGraph.AddEdge(3, 4);
        unweightedGraph.AddEdge(3, 6);
        unweightedGraph.AddEdge(4, 6);
        unweightedGraph.AddEdge(4, 5);
        unweightedGraph.AddEdge(4, 7);
        unweightedGraph.AddEdge(5, 7);
        unweightedGraph.AddEdge(6, 5);

        double[] bfsScores = AlgorithmCalculator.ScoreCalcBFS(unweightedGraph, nUnweighted);
        Console.WriteLine("Influence Scores (BFS - Unweighted):");
        for (int i = 0; i < nUnweighted; i++)
        {
            Console.WriteLine($"Node {i}: {bfsScores[i]}");
        }

        // Dijkstra
        //A: 0
        //B: 1
        //c: 2
        //D: 3
        //E: 4
        //F: 5
        //G: 6
        //H: 7
        //I: 8
        //J: 9

        int nWeighted = 10;
        Graph weightedGraph = new Graph(nWeighted, isWeighted: true);

        weightedGraph.AddWeightedEdge(0, 1, 1);
        weightedGraph.AddWeightedEdge(0, 2, 1);
        weightedGraph.AddWeightedEdge(0, 4, 5);
        weightedGraph.AddWeightedEdge(1, 2, 4);
        weightedGraph.AddWeightedEdge(1, 4, 1);
        weightedGraph.AddWeightedEdge(1, 6, 1);
        weightedGraph.AddWeightedEdge(1, 7, 1);
        weightedGraph.AddWeightedEdge(2, 3, 3);
        weightedGraph.AddWeightedEdge(2, 4, 1);
        weightedGraph.AddWeightedEdge(3, 4, 2);
        weightedGraph.AddWeightedEdge(3, 5, 1);
        weightedGraph.AddWeightedEdge(3, 6, 5);
        weightedGraph.AddWeightedEdge(4, 6, 2);
        weightedGraph.AddWeightedEdge(5, 6, 1);
        weightedGraph.AddWeightedEdge(6, 7, 2);
        weightedGraph.AddWeightedEdge(7, 8, 3);
        weightedGraph.AddWeightedEdge(8, 9, 3);

        double[] dijkstraScores = AlgorithmCalculator.ScoreCalcDijkstra(weightedGraph, nWeighted);
        Console.WriteLine("\nInfluence Scores (Dijkstra - Weighted):");
        for (int i = 0; i < nWeighted; i++)
        {
            Console.WriteLine($"Node {i}: {dijkstraScores[i]}");
        }
    }
}
