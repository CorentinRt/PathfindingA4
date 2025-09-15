// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

namespace PathfindingDemoA;

public class Node
{
    public string name = "";
    public Dictionary<Node, int> adjacents = new Dictionary<Node, int>();
}
public class NodeConfig
{
    public string NodeA = "";
    public string NodeB = "";
    public int cost = 0;

    public NodeConfig(string nodeA, string nodeB, int cost)
    {
        NodeA = nodeA;
        NodeB = nodeB;
        this.cost = cost;
    }
}

public class PathDemo
{

    List<string> nodeNames = new List<string>() {"A", "B", "C", "D", "E", "F", "G"};
    List<NodeConfig> nodeConfigs = new List<NodeConfig>()
    {
        new NodeConfig("A", "B", 1),
        new NodeConfig("A", "C", 3),
        new NodeConfig("A", "E", 2),
        new NodeConfig("B", "C", 1),
        new NodeConfig("B", "D", 4),
        new NodeConfig("C", "D", 2),
        new NodeConfig("C", "E", 1),
        new NodeConfig("C", "G", 2),
        new NodeConfig("D", "F", 1),
        new NodeConfig("D", "G", 4),
        new NodeConfig("F", "G", 1),
    };

    Dictionary<string, Node> nodes = new Dictionary<string, Node> ();

    public PathDemo()
    {
    }

    void Initialize()
    {
        nodes = new Dictionary<string, Node>();
        foreach (string name in nodeNames)
        {
            Node node = new Node();
            node.name = name;
            nodes.Add(name, node);
        }

        foreach (NodeConfig nodeConfig in nodeConfigs)
        {
            if (!nodes.ContainsKey(nodeConfig.NodeA))
                throw new Exception("Missing node");
            if (!nodes.ContainsKey(nodeConfig.NodeB))
                throw new Exception("Missing node");
            Node nodeA = nodes[nodeConfig.NodeA];
            Node nodeB = nodes[nodeConfig.NodeB];
            nodeA.adjacents.Add(nodeB, nodeConfig.cost);
            nodeB.adjacents.Add(nodeA, nodeConfig.cost);
        }
    }

    public void Run()
    {
        Initialize();

        List<Node> path = FindPathGlouton("A", "G");

        string pathString = "Path is : ";
        foreach (Node node in path)
        {
            pathString += $"-> {node.name} ";
        }
        
        Console.WriteLine(pathString);
    }
    
    public List<Node> FindPathGlouton(string start, string goal)
    {
        if (!nodes.ContainsKey(start))
            throw new Exception("Missing node");
        
        if (!nodes.ContainsKey(goal))
            throw new Exception("Missing node");
        
        List<Node> path = new List<Node>();

        HashSet<Node> visited = new HashSet<Node>();
        
        Node startNode = nodes[start];
        
        Node goalNode = nodes[goal];
        
        path.Add(startNode);
        visited.Add(startNode);
        
        Node currentNode = startNode;
        
        while (currentNode != goalNode && currentNode != null)
        {
            int lowestCost = -1;
            Node lowestNode = null;
            
            foreach (KeyValuePair<Node, int> adjacent in currentNode.adjacents)
            {
                if (visited.Contains(adjacent.Key))
                    continue;
                
                /*
                 Ici je check si l'adjacent est le goal. Cela peut-être viable en fonction du contexte.
                 Si je veux un chemin très rapidement mais pas forcement viable alors pas besoin.
                 Si je veux agrandir mes chances de trouver un chemin viable mais pas forcement alors pourquoi pas.
                if (adjacent.Key == goalNode)
                {
                    lowestCost = adjacent.Value;
                    lowestNode = adjacent.Key;
                    break;
                }
                */
                
                if (adjacent.Value < lowestCost || lowestCost == -1)
                {
                    lowestCost = adjacent.Value;
                    lowestNode = adjacent.Key;
                }
            }

            // Revenir en arriere si impossible
            if (lowestNode == null)
            {
                currentNode = path[^2];
                path.RemoveAt(path.Count - 1);
                continue;
            }
            
            currentNode = lowestNode;
            
            path.Add(currentNode);
            visited.Add(currentNode);
        }

        return path;
    }
    
    public List<Node> FindPathBreadthFirstSearch(string start, string goal)
    {
        List<Node> result = new List<Node>();

        

        return result;
    }
}