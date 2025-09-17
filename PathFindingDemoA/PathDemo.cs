// See https://aka.ms/new-console-template for more information

// Corentin REMOT

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

        //List<Node> path = FindPathGlouton("A", "G");
        //List<Node> path = FindPathBreadthFirstSearch("A", "G");
        List<Node> path = FindPathDijkstra("A", "G");

        PrintPath(path);
    }

    private void PrintPath(List<Node> path)
    {
        string pathString = "Path is : ";
        foreach (Node node in path)
        {
            pathString += $"-> {node.name} ";
        }
        
        Console.WriteLine(pathString);
    }
    
    // ---------------------------------------
    // Glouton pathfinding
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
    
    // ---------------------------------------
    // BFS pathfinding
    public List<Node> FindPathBreadthFirstSearch(string start, string goal)
    {
        Queue<Node> toExplore = new Queue<Node>();
        
        HashSet<Node> visited = new HashSet<Node>();
        
        Dictionary<Node, Node> childToParentDictionary =  new Dictionary<Node, Node>();
        
        Node startNode = nodes[start];
        Node goalNode = nodes[goal];
        
        toExplore.Enqueue(startNode);
        visited.Add(startNode);
        
        Console.WriteLine("----------------------------");
        Console.WriteLine("Start algo BFS");
        
        while (toExplore.Count > 0)
        {
            // Process next toExplore node
            Node currentNode = toExplore.Dequeue();
            Console.WriteLine("----------------------------");
            
            Console.WriteLine("Current node: " + currentNode.name);

            if (currentNode == goalNode)
            {
                Console.WriteLine("Find goal node !");
                break;
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine("Start processing adjacents");
            
            // Add every adjacent in to explore and visited to be checked later
            foreach (KeyValuePair<Node, int> adjacent in currentNode.adjacents)
            {
                if (visited.Contains(adjacent.Key))
                    continue;

                Console.WriteLine($"Add to visited and to explore {adjacent.Key.name}");
                
                visited.Add(adjacent.Key);
                toExplore.Enqueue(adjacent.Key);

                Console.WriteLine($"Set parent of {adjacent.Key.name} to {currentNode.name}");
                
                // Create child->parent path entry in dict
                childToParentDictionary[adjacent.Key] = currentNode;
            }
            Console.WriteLine("End processing adjacents");

            string visitedText = "Visited : ";
            string toExploreText = "To explore : ";

            foreach (Node node in visited)
            {
                visitedText += node.name + " ";
            }
            
            foreach (Node node in toExplore)
            {
                toExploreText += node.name + " ";
            }
            
            Console.WriteLine(visitedText);
            Console.WriteLine(toExploreText);
        }
        
        // Re-build path (but in reverse)
        Console.WriteLine("-----------------------------");

        Console.WriteLine("Retrace chemin de parent en parent depuis le goalNode");
        
        List<Node> path = new List<Node>();
        
        if (!childToParentDictionary.ContainsKey(goalNode))
            return path;
        
        Node currentParent = goalNode;
        path.Add(goalNode);
        
        Console.WriteLine("Goal node : " + goalNode.name);
        
        while (currentParent != startNode)
        {
            Console.WriteLine($"Parent of {currentParent.name} is : " + childToParentDictionary[currentParent].name);
            
            path.Add(childToParentDictionary[currentParent]);
            currentParent = childToParentDictionary[currentParent];
        }

        // Path need to be reversed to get start first
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Reverse path");
        path.Reverse();

        Console.WriteLine("-----------------------------");
        
        return path;
    }

    // ---------------------------------------
    // Dijkstra pathfinding
    public List<Node> FindPathDijkstra(string start, string goal)
    {
        List<Node> path = new List<Node>();

        Node startNode = nodes[start];
        Node goalNode = nodes[goal];
        
        List<Node> unExplored = new List<Node>();
        Dictionary<Node, int> nodeDistance = new Dictionary<Node, int>();
        Dictionary<Node, Node> nodeParent = new Dictionary<Node, Node>();

        foreach (KeyValuePair<string, Node> pair in nodes)
        {
            unExplored.Add(pair.Value);
            
            if (pair.Value == startNode)
            {
                nodeDistance[pair.Value] = 0;
                continue;
            }
            
            nodeDistance[pair.Value] = int.MaxValue;
        }


        while (unExplored.Count > 0)
        {
            Console.WriteLine("-------------------------------");
            Node currentNode = null;
            int currentDistance = -1;
            
            foreach (Node node in unExplored)
            {
                if (currentDistance == -1 || currentDistance > nodeDistance[node])
                {
                    currentNode = node;
                    currentDistance = nodeDistance[node];
                }
            }

            if (currentNode == null)
            {
                Console.WriteLine($"No path possible to goal : {goalNode.name} from start : {startNode.name}");
                return path;
            }

            Console.WriteLine($"Current node: {currentNode.name}");
            
            unExplored.Remove(currentNode);

            if (currentNode == goalNode)
            {
                break;
            }

            foreach (KeyValuePair<Node, int> pair in currentNode.adjacents)
            {
                if (!unExplored.Contains(pair.Key))
                    continue;
                
                Console.WriteLine($"Explore adjacent {pair.Key.name}");
                int newDistance = nodeDistance[currentNode] + pair.Value;

                if (nodeDistance[pair.Key] > newDistance)
                {
                    Console.WriteLine($"Add parent of {pair.Key.name} to {currentNode.name}");
                    nodeDistance[pair.Key] = newDistance;
                    nodeParent[pair.Key] =  currentNode;
                }
            }
        }
        
        Console.WriteLine("-------------------------------");
        
        Node currentParent = goalNode;
        path.Add(goalNode);
        
        Console.WriteLine("Goal node : " + goalNode.name);
        
        while (currentParent != startNode)
        {
            Console.WriteLine($"Parent of {currentParent.name} is : " + nodeParent[currentParent].name);
            
            path.Add(nodeParent[currentParent]);
            currentParent = nodeParent[currentParent];
        }
        
        path.Reverse();
        
        return path;
    }
}