using graph;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== GRAPH IMPLEMENTATION TESTING ===\n");

        // Test 1: Undirected Graph
        TestUndirectedGraph();

        Console.WriteLine("\n" + new string('=', 60) + "\n");

        // Test 2: Directed Graph
        TestDirectedGraph();

        Console.WriteLine("\n" + new string('=', 60) + "\n");

        // Test 3: Error Handling
        TestErrorHandling();

        Console.WriteLine("\n" + new string('=', 60) + "\n");

        // Test 4: Advanced Features
        TestAdvancedFeatures();

        Console.WriteLine("\n" + new string('=', 60) + "\n");

        // Test 5: Performance Test - Deep Graph
        TestDeepGraphPerformance();

        Console.WriteLine("\n=== ALL TESTS COMPLETED ===");
    }

    static void TestUndirectedGraph()
    {
        Console.WriteLine("TEST 1: UNDIRECTED GRAPH");
        Console.WriteLine("Creating an undirected graph...");

        var graph = new GraphModel<string>(GraphType.Undirected);
        
        // Create nodes
        var a = new Node<string>("A", "Node A");
        var b = new Node<string>("B", "Node B");
        var c = new Node<string>("C", "Node C");
        var d = new Node<string>("D", "Node D");
        var e = new Node<string>("E", "Node E");

        // Add nodes
        graph.AddNode(a);
        graph.AddNode(b);
        graph.AddNode(c);
        graph.AddNode(d);
        graph.AddNode(e);

        // Add edges
        graph.AddEdge(a, b);
        graph.AddEdge(b, c);
        graph.AddEdge(c, d);
        graph.AddEdge(d, e);
        graph.AddEdge(a, c); // Creates a triangle

        Console.WriteLine($"Graph created with {graph.CountNodes()} nodes and {graph.CountEdges()} edges");
        Console.WriteLine($"Graph type: {graph.GetGraphType()}");

        // Test basic operations
        Console.WriteLine("\nBASIC OPERATIONS:");
        Console.WriteLine($"Nodes: {string.Join(", ", graph.GetAllNodes().Select(n => n.Name))}");
        Console.WriteLine($"Neighbors of A: {string.Join(", ", graph.GetNeighbors(a).Select(n => n.Name))}");
        Console.WriteLine($"Degree of A: {graph.GetNodeDegree(a)}");
        Console.WriteLine($"A connected to B: {graph.IsConnected(a, b)}");
        Console.WriteLine($"A connected to E: {graph.IsConnected(a, e)}");

        // Test path finding
        Console.WriteLine("\n PATH FINDING:");
        var path1 = graph.FindPath(a, e);
        Console.WriteLine($"Path from A to E (DFS): {string.Join(" -> ", path1.Select(n => n.Name))}");

        var shortestPath = graph.GetShortestPath(a, e);
        Console.WriteLine($"Shortest path from A to E (BFS): {string.Join(" -> ", shortestPath.Select(n => n.Name))}");

        // Test traversal data
        Console.WriteLine("\n TRAVERSAL DATA:");
        var dfsData = graph.GetDFSTraversalData(a, e);
        Console.WriteLine($"DFS visited nodes: {string.Join(", ", dfsData.VisitedNodes.Select(n => n.Name))}");
        Console.WriteLine($"DFS target reached: {dfsData.TargetReached}");

        var bfsData = graph.GetBFSTraversalData(a, e);
        Console.WriteLine($"BFS visited nodes: {string.Join(", ", bfsData.VisitedNodes.Select(n => n.Name))}");
        Console.WriteLine($"BFS target reached: {bfsData.TargetReached}");

        // Test reachabilit
        Console.WriteLine("\nREACHABILITY:");
        Console.WriteLine($"Is E reachable from A: {graph.IsReachable(a, e)}");
        Console.WriteLine($"Distance from A to E: {graph.GetDistance(a, e)}");

        // Test all reachable nodes
        var reachableDFS = graph.GetAllReachableNodesDFS(a);
        Console.WriteLine($"All nodes reachable from A (DFS): {string.Join(", ", reachableDFS.Select(n => n.Name))}");

        var reachableBFS = graph.GetAllReachableNodesBFS(a);
        Console.WriteLine($"All nodes reachable from A (BFS): {string.Join(", ", reachableBFS.Select(kvp => $"{kvp.Key.Name}({kvp.Value})"))}");

        // Test graph analysis
        Console.WriteLine("\n GRAPH ANALYSIS:");
        Console.WriteLine($"Has cycles: {graph.HasCycle()}");
        
        var components = graph.GetConnectedComponents();
        Console.WriteLine($"Connected components: {components.Count}");
        for (int i = 0; i < components.Count; i++)
        {
            Console.WriteLine($"  Component {i + 1}: {string.Join(", ", components[i].Select(n => n.Name))}");
        }
    }

    static void TestDirectedGraph()
    {
        Console.WriteLine(" TEST 2: DIRECTED GRAPH");
        Console.WriteLine("Creating a directed graph...");

        var graph = new GraphModel<string>(GraphType.Directed);
        
        // Create nodes
        var a = new Node<string>("A", "Node A");
        var b = new Node<string>("B", "Node B");
        var c = new Node<string>("C", "Node C");
        var d = new Node<string>("D", "Node D");

        // Add nodes
        graph.AddNode(a);
        graph.AddNode(b);
        graph.AddNode(c);
        graph.AddNode(d);

        // Add directed edges
        graph.AddEdge(a, b);
        graph.AddEdge(b, c);
        graph.AddEdge(c, d);
        graph.AddEdge(d, a); // Creates a cycle

        Console.WriteLine($"Directed graph created with {graph.CountNodes()} nodes and {graph.CountEdges()} edges");
        Console.WriteLine($"Graph type: {graph.GetGraphType()}");

        // Test directed connections
        Console.WriteLine("\nDIRECTED CONNECTIONS:");
        Console.WriteLine($"A -> B: {graph.HasEdge(a, b)}");
        Console.WriteLine($"B -> A: {graph.HasEdge(b, a)}"); // Should be false in directed graph
        Console.WriteLine($"Neighbors of A: {string.Join(", ", graph.GetNeighbors(a).Select(n => n.Name))}");
        Console.WriteLine($"Neighbors of B: {string.Join(", ", graph.GetNeighbors(b).Select(n => n.Name))}");

        // Test path finding in directed graph
        Console.WriteLine("\nDIRECTED PATH FINDING:");
        var path = graph.FindPath(a, d);
        Console.WriteLine($"Path from A to D: {string.Join(" -> ", path.Select(n => n.Name))}");

        var reversePath = graph.FindPath(d, a);
        Console.WriteLine($"Path from D to A: {string.Join(" -> ", reversePath.Select(n => n.Name))}");

        // Test cycle detection
        Console.WriteLine("\nCYCLE DETECTION:");
        Console.WriteLine($"Has cycles: {graph.HasCycle()}");

        // Test reachability in directed graph
        Console.WriteLine("\nDIRECTED REACHABILITY:");
        Console.WriteLine($"A reachable from D: {graph.IsReachable(d, a)}");
        Console.WriteLine($"D reachable from A: {graph.IsReachable(a, d)}");
        Console.WriteLine($"Distance from A to D: {graph.GetDistance(a, d)}");
        Console.WriteLine($"Distance from D to A: {graph.GetDistance(d, a)}");
    }

    static void TestErrorHandling()
    {
        Console.WriteLine("TEST 3: ERROR HANDLING");
        Console.WriteLine("Testing error handling and validation...");

        var graph = new GraphModel<string>(GraphType.Undirected);
        var a = new Node<string>("A", "Node A");
        var b = new Node<string>("B", "Node B");

        // Test null parameter handling
        Console.WriteLine("\nNULL PARAMETER TESTS:");
        try
        {
            graph.AddNode(null);
            Console.WriteLine("Should have thrown exception for null node");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Correctly caught null parameter: {ex.Message}");
        }

        try
        {
            graph.AddEdge(null, b);
            Console.WriteLine("Should have thrown exception for null edge");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Correctly caught null edge parameter: {ex.Message}");
        }

        // Test non-existent node operations
        Console.WriteLine("\nNON-EXISTENT NODE TESTS:");
        try
        {
            graph.RemoveNode(a);
            Console.WriteLine("Should have thrown exception for non-existent node");
        }
        catch (NodeNotFoundException ex)
        {
            Console.WriteLine($"Correctly caught non-existent node: {ex.Message}");
        }

        try
        {
            graph.GetNeighbors(a);
            Console.WriteLine("Correctly handled non-existent node (returned empty list)");
        }
        catch
        {
            Console.WriteLine("Should not have thrown exception");
        }

        // Test self-loop prevention
        Console.WriteLine("\nSELF-LOOP TESTS:");
        graph.AddNode(a);
        try
        {
            graph.AddEdge(a, a);
            Console.WriteLine("Should have thrown exception for self-loop");
        }
        catch (InvalidGraphOperationException ex)
        {
            Console.WriteLine($"Correctly caught self-loop: {ex.Message}");
        }

        // Test path finding with non-existent nodes
        Console.WriteLine("\nPATH FINDING WITH NON-EXISTENT NODES:");
        try
        {
            graph.FindPath(a, b);
            Console.WriteLine("❌ Should have thrown exception for non-existent target");
        }
        catch (NodeNotFoundException ex)
        {
            Console.WriteLine($"Correctly caught non-existent target: {ex.Message}");
        }
    }

    static void TestAdvancedFeatures()
    {
        Console.WriteLine("TEST 4: ADVANCED FEATURES");
        Console.WriteLine("Testing advanced graph features...");

        var graph = new GraphModel<string>(GraphType.Undirected);
        
        // Create a more complex graph
        var nodes = new List<Node<string>>();
        for (char c = 'A'; c <= 'G'; c++)
        {
            var node = new Node<string>(c.ToString(), $"Node {c}");
            nodes.Add(node);
            graph.AddNode(node);
        }

        // Create a complex structure
        graph.AddEdge(nodes[0], nodes[1]); // A-B
        graph.AddEdge(nodes[1], nodes[2]); // B-C
        graph.AddEdge(nodes[2], nodes[3]); // C-D
        graph.AddEdge(nodes[3], nodes[4]); // D-E
        graph.AddEdge(nodes[4], nodes[5]); // E-F
        graph.AddEdge(nodes[5], nodes[6]); // F-G
        graph.AddEdge(nodes[0], nodes[2]); // A-C (creates triangle)
        graph.AddEdge(nodes[1], nodes[4]); // B-E (creates shortcut)

        Console.WriteLine($"Complex graph created with {graph.CountNodes()} nodes and {graph.CountEdges()} edges");

        // Test iterative DFS
        Console.WriteLine("\nITERATIVE DFS:");
        var iterativeDFS = graph.GetAllReachableNodesIterativeDFS(nodes[0]);
        Console.WriteLine($"Iterative DFS from A: {string.Join(" -> ", iterativeDFS.Select(n => n.Name))}");

        // Test different starting points
        Console.WriteLine("\nMULTIPLE STARTING POINTS:");
        foreach (var startNode in nodes.Take(3))
        {
            var reachable = graph.GetAllReachableNodesDFS(startNode);
            Console.WriteLine($"From {startNode.Name}: {string.Join(", ", reachable.Select(n => n.Name))}");
        }

        // Test edge removal
        Console.WriteLine("\n✂️ EDGE REMOVAL:");
        Console.WriteLine($"Before removal - A connected to B: {graph.IsConnected(nodes[0], nodes[1])}");
        graph.RemoveEdge(nodes[0], nodes[1]);
        Console.WriteLine($"After removal - A connected to B: {graph.IsConnected(nodes[0], nodes[1])}");

        // Test node removal
        Console.WriteLine("\nNODE REMOVAL:");
        Console.WriteLine($"Before removal - nodes: {graph.CountNodes()}, edges: {graph.CountEdges()}");
        graph.RemoveNode(nodes[2]); // Remove C
        Console.WriteLine($"After removing C - nodes: {graph.CountNodes()}, edges: {graph.CountEdges()}");
        Console.WriteLine($"Remaining nodes: {string.Join(", ", graph.GetAllNodes().Select(n => n.Name))}");

        // Test graph clearing
        Console.WriteLine("\nGRAPH CLEARING:");
        graph.Clear();
        Console.WriteLine($"After clearing - nodes: {graph.CountNodes()}, edges: {graph.CountEdges()}");
        Console.WriteLine($"Is graph empty: {graph.CountNodes() == 0}");

        // Test with new graph
        Console.WriteLine("\nNEW GRAPH TEST:");
        var newGraph = new GraphModel<string>(GraphType.Directed);
        var x = new Node<string>("X", "Node X");
        var y = new Node<string>("Y", "Node Y");
        
        newGraph.AddNode(x);
        newGraph.AddNode(y);
        newGraph.AddEdge(x, y);
        
        Console.WriteLine($"New directed graph: {newGraph.CountNodes()} nodes, {newGraph.CountEdges()} edges");
        Console.WriteLine($"X -> Y: {newGraph.HasEdge(x, y)}");
        Console.WriteLine($"Y -> X: {newGraph.HasEdge(y, x)}");
    }

    static void TestDeepGraphPerformance()
    {
        Console.WriteLine("TEST 5: PERFORMANCE TEST - DEEP GRAPH");
        Console.WriteLine("Creating a deep graph...");

        var graph = new GraphModel<string>(GraphType.Undirected);
        
        // Create a deep structure
        var nodes = new List<Node<string>>();
        for (int i = 0; i < 1000; i++)
        {
            var node = new Node<string>(i.ToString(), $"Node {i}");
            nodes.Add(node);
            graph.AddNode(node);
        }

        // Add edges
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            graph.AddEdge(nodes[i], nodes[i + 1]);
        }

        Console.WriteLine($"Deep graph created with {graph.CountNodes()} nodes and {graph.CountEdges()} edges");

        // Test path finding
        Console.WriteLine("\n PATH FINDING:");
        var path = graph.FindPath(nodes[0], nodes[999]);
        Console.WriteLine($"Path from Node 0 to Node 999: {string.Join(" -> ", path.Select(n => n.Name))}");

        // Test traversal data
        Console.WriteLine("\n TRAVERSAL DATA:");
        var dfsData = graph.GetDFSTraversalData(nodes[0], nodes[999]);
        Console.WriteLine($"DFS visited nodes: {string.Join(", ", dfsData.VisitedNodes.Select(n => n.Name))}");
        Console.WriteLine($"DFS target reached: {dfsData.TargetReached}");

        var bfsData = graph.GetBFSTraversalData(nodes[0], nodes[999]);
        Console.WriteLine($"BFS visited nodes: {string.Join(", ", bfsData.VisitedNodes.Select(n => n.Name))}");
        Console.WriteLine($"BFS target reached: {bfsData.TargetReached}");

        // Test reachabilit
        Console.WriteLine("\nREACHABILITY:");
        Console.WriteLine($"Is Node 999 reachable from Node 0: {graph.IsReachable(nodes[0], nodes[999])}");
        Console.WriteLine($"Distance from Node 0 to Node 999: {graph.GetDistance(nodes[0], nodes[999])}");
    }
}

