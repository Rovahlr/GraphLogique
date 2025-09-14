# GraphLibrary

A generic graph library for .NET 8.0 that supports both directed and undirected graphs with advanced traversal algorithms.

## Overview

This library provides a complete implementation of graph data structures with:
- Support for directed and undirected graphs
- Traversal algorithms (DFS, BFS)
- Path finding and distance calculation
- Cycle detection and connected components
- Robust error handling with custom exceptions

## Architecture

### Main Classes

- **`GraphModel<T>`** : Main class implementing the `IGraph<T>` interface
- **`Node<T>`** : Represents a node with a name and data of type T
- **`GraphTraversal<T>`** : Provides traversal algorithms (DFS, BFS)
- **`TraversalResult<T>`** : Contains traversal results
- **`GraphType`** : Enumeration for graph types (Directed/Undirected)

### Interface

```csharp
public interface IGraph<T>
{
    // Basic operations
    void AddNode(Node<T> node);
    void RemoveNode(Node<T> node);
    bool NodeExists(Node<T> node);
    List<Node<T>> GetAllNodes();
    int CountNodes();
    int CountEdges();
    
    // Edge operations
    void AddEdge(Node<T> from, Node<T> to);
    void RemoveEdge(Node<T> from, Node<T> to);
    bool HasEdge(Node<T> from, Node<T> to);
    
    // Node information
    List<Node<T>> GetNeighbors(Node<T> node);
    int GetNodeDegree(Node<T> node);
    
    // Graph analysis
    bool IsConnected(Node<T> from, Node<T> to);
    List<Node<T>> FindPath(Node<T> start, Node<T> end);
    List<Node<T>> GetShortestPath(Node<T> start, Node<T> end);
    bool HasCycle();
    List<List<Node<T>>> GetConnectedComponents();
    
    // Utilities
    void Clear();
}
```

## Usage

### Create an undirected graph
```csharp
using graph;

var graph = new GraphModel<string>(GraphType.Undirected);

// Create nodes
var a = new Node<string>("A", "Node A");
var b = new Node<string>("B", "Node B");
var c = new Node<string>("C", "Node C");

// Add nodes
graph.AddNode(a);
graph.AddNode(b);
graph.AddNode(c);

// Add edges
graph.AddEdge(a, b);
graph.AddEdge(b, c);
```

### Create a directed graph
```csharp
var directedGraph = new GraphModel<int>(GraphType.Directed);

var node1 = new Node<int>("1", 1);
var node2 = new Node<int>("2", 2);

directedGraph.AddNode(node1);
directedGraph.AddNode(node2);
directedGraph.AddEdge(node1, node2); // Direction: 1 -> 2
```

### Path finding
```csharp
// Any path (DFS)
var path = graph.FindPath(startNode, endNode);

// Shortest path (BFS)
var shortestPath = graph.GetShortestPath(startNode, endNode);

// Check connectivity
bool isConnected = graph.IsConnected(nodeA, nodeB);
bool isReachable = graph.IsReachable(startNode, targetNode);
```

### Advanced analysis
```csharp
// Cycle detection
bool hasCycle = graph.HasCycle();

// Connected components
var components = graph.GetConnectedComponents();

// Distance between nodes
int distance = graph.GetDistance(startNode, endNode);

// All reachable nodes
var reachableNodes = graph.GetAllReachableNodesDFS(startNode);
var reachableWithDistances = graph.GetAllReachableNodesBFS(startNode);
```

## Features

### Basic Operations
- Add/remove nodes and edges
- Existence and connectivity verification
- Node and edge counting

### Traversal Algorithms
- **DFS (Depth-First Search)** : Depth-first traversal
- **BFS (Breadth-First Search)** : Breadth-first traversal
- **Iterative DFS** : Non-recursive version of DFS

### Graph Analysis
- Cycle detection
- Connected components search
- Minimum distance calculation
- Connectivity verification

### Error Handling
- `NodeNotFoundException` : Node not found
- `InvalidGraphOperationException` : Invalid operation
- `NoPathExistsException` : No path found

### Supported Types
The library is generic and supports all types:
- `GraphModel<string>`
- `GraphModel<int>`
- `GraphModel<CustomClass>`
- etc.

## Complete Example

```csharp
using graph;

// Create an undirected graph
var graph = new GraphModel<string>(GraphType.Undirected);

// Create and add nodes
var nodes = new[]
{
    new Node<string>("A", "City A"),
    new Node<string>("B", "City B"),
    new Node<string>("C", "City C"),
    new Node<string>("D", "City D")
};

foreach (var node in nodes)
{
    graph.AddNode(node);
}

// Create connections
graph.AddEdge(nodes[0], nodes[1]); // A-B
graph.AddEdge(nodes[1], nodes[2]); // B-C
graph.AddEdge(nodes[2], nodes[3]); // C-D
graph.AddEdge(nodes[0], nodes[2]); // A-C (triangle)

// Analyze the graph
Console.WriteLine($"Nodes: {graph.CountNodes()}");
Console.WriteLine($"Edges: {graph.CountEdges()}");
Console.WriteLine($"Has cycles: {graph.HasCycle()}");

// Find a path
var path = graph.GetShortestPath(nodes[0], nodes[3]);
Console.WriteLine($"Path A->D: {string.Join(" -> ", path.Select(n => n.Name))}");

// Connected components
var components = graph.GetConnectedComponents();
Console.WriteLine($"Components: {components.Count}");
```

## Performance

- Uses adjacency lists for efficient representation
- Optimized algorithms for common operations
- Supports large graphs (tested with 1000+ nodes)
- Efficient memory management with automatic cleanup 