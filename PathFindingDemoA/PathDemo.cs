// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

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
    }

}