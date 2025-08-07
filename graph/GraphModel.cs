using System.Reflection;
using System.Xml.Linq;
namespace graph
{
    /// <summary>
    /// Generic graph implementation supporting both directed and undirected graphs.
    /// Provides comprehensive graph traversal algorithms and analysis methods.
    /// </summary>
    /// <typeparam name="T">The type of data stored in graph nodes</typeparam>
    public class GraphModel<T> : IGraph<T>
    {
        /// <summary>
        /// Internal representation of the graph as adjacency list.
        /// Key: Node, Value: List of neighboring nodes
        /// </summary>
        private readonly Dictionary<Node<T>, List<Node<T>>> _adjacencyList = new();

        /// <summary>
        /// Type of the graph (directed or undirected).
        /// </summary>
        private readonly GraphType _graphType;

        /// <summary>
        /// Graph traversal algorithms provider.
        /// </summary>
        private readonly GraphTraversal<T> _traversal;

        /// <summary>
        /// Creates a new graph with specified type.
        /// </summary>
        /// <param name="graphType">Type of graph (directed or undirected)</param>
        public GraphModel(GraphType graphType = GraphType.Undirected)
        {
            _graphType = graphType;
            _traversal = new GraphTraversal<T>(_adjacencyList);
        }

        // === BASIC GRAPH OPERATIONS ===

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add</param>
        public void AddNode(Node<T> node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));
            
            if (!_adjacencyList.ContainsKey(node))
            {
                _adjacencyList[node] = new List<Node<T>>();
            }
        }

        /// <summary>
        /// Removes a node and all its connections from the graph.
        /// </summary>
        /// <param name="node">The node to remove</param>
        public void RemoveNode(Node<T> node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));
            
            if (!_adjacencyList.ContainsKey(node))
                throw new NodeNotFoundException(node.Name);

            // Remove the node
            _adjacencyList.Remove(node);

            // Remove all references to this node from other nodes
            foreach (var neighbors in _adjacencyList.Values)
            {
                neighbors.RemoveAll(n => n == node);
            }
        }

        /// <summary>
        /// Checks if a node exists in the graph.
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <returns>True if the node exists, false otherwise</returns>
        public bool NodeExists(Node<T> node)
        {
            return node != null && _adjacencyList.ContainsKey(node);
        }

        /// <summary>
        /// Gets all nodes in the graph.
        /// </summary>
        /// <returns>List of all nodes</returns>
        public List<Node<T>> GetAllNodes()
        {
            return _adjacencyList.Keys.ToList();
        }

        /// <summary>
        /// Counts the total number of nodes in the graph.
        /// </summary>
        /// <returns>Number of nodes</returns>
        public int CountNodes()
        {
            return _adjacencyList.Count;
        }

        /// <summary>
        /// Counts the total number of edges in the graph.
        /// For undirected graphs, each edge is counted once.
        /// </summary>
        /// <returns>Number of edges</returns>
        public int CountEdges()
        {
            int count = 0;
            foreach (var neighbors in _adjacencyList.Values)
            {
                count += neighbors.Count;
            }
            return _graphType == GraphType.Undirected ? count / 2 : count;
        }

        // === EDGE OPERATIONS ===

        /// <summary>
        /// Adds an edge between two nodes.
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Target node</param>
        public void AddEdge(Node<T> from, Node<T> to)
        {
            ValidateEdgeParameters(from, to);

            // Add nodes if they don't exist
            AddNode(from);
            AddNode(to);

            // Add edge from 'from' to 'to'
            if (!_adjacencyList[from].Contains(to))
            {
                _adjacencyList[from].Add(to);
            }

            // For undirected graphs, add edge from 'to' to 'from'
            if (_graphType == GraphType.Undirected && !_adjacencyList[to].Contains(from))
            {
                _adjacencyList[to].Add(from);
            }
        }

        /// <summary>
        /// Removes an edge between two nodes.
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Target node</param>
        public void RemoveEdge(Node<T> from, Node<T> to)
        {
            ValidateEdgeParameters(from, to);

            if (!NodeExists(from) || !NodeExists(to))
                throw new NodeNotFoundException($"One or both nodes do not exist");

            _adjacencyList[from].Remove(to);

            // For undirected graphs, remove edge from 'to' to 'from'
            if (_graphType == GraphType.Undirected)
            {
                _adjacencyList[to].Remove(from);
            }
        }

        /// <summary>
        /// Checks if there is an edge between two nodes.
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Target node</param>
        /// <returns>True if edge exists, false otherwise</returns>
        public bool HasEdge(Node<T> from, Node<T> to)
        {
            if (!NodeExists(from) || !NodeExists(to))
                return false;

            return _adjacencyList[from].Contains(to);
        }

        // === NODE INFORMATION ===

        /// <summary>
        /// Gets all neighbors of a given node.
        /// </summary>
        /// <param name="node">The node to get neighbors for</param>
        /// <returns>List of neighboring nodes, empty list if node doesn't exist</returns>
        public List<Node<T>> GetNeighbors(Node<T> node)
        {
            if (!NodeExists(node))
                return new List<Node<T>>();

            return _adjacencyList[node].ToList();
        }

        /// <summary>
        /// Gets the degree (number of neighbors) of a node.
        /// </summary>
        /// <param name="node">The node to get degree for</param>
        /// <returns>Number of neighbors, 0 if node doesn't exist</returns>
        public int GetNodeDegree(Node<T> node)
        {
            if (!NodeExists(node))
                return 0;

            return _adjacencyList[node].Count;
        }

        // === GRAPH ANALYSIS ===

        /// <summary>
        /// Checks if there is a direct connection between two nodes.
        /// </summary>
        /// <param name="from">First node</param>
        /// <param name="to">Second node</param>
        /// <returns>True if nodes are directly connected, false otherwise</returns>
        public bool IsConnected(Node<T> from, Node<T> to)
        {
            if (!NodeExists(from) || !NodeExists(to))
                return false;

            if (from == to)
                return true;

            return HasEdge(from, to);
        }

        /// <summary>
        /// Finds a path between two nodes using DFS.
        /// Returns any path, not necessarily the shortest.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="end">Target node</param>
        /// <returns>List of nodes forming the path, empty if no path exists</returns>
        public List<Node<T>> FindPath(Node<T> start, Node<T> end)
        {
            ValidatePathParameters(start, end);

            if (start == end)
                return new List<Node<T>> { start };

            var result = _traversal.GetDFSTraversalData(start, end);
            if (!result.TargetReached)
                return new List<Node<T>>();

            return ReconstructPath(start, end, result.Parents);
        }

        /// <summary>
        /// Finds the shortest path between two nodes using BFS.
        /// Returns the path with minimum number of edges.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="end">Target node</param>
        /// <returns>List of nodes forming the shortest path, empty if no path exists</returns>
        public List<Node<T>> GetShortestPath(Node<T> start, Node<T> end)
        {
            ValidatePathParameters(start, end);

            if (start == end)
                return new List<Node<T>> { start };

            var result = _traversal.GetBFSTraversalData(start, end);
            if (!result.TargetReached)
                return new List<Node<T>>();

            return ReconstructPath(start, end, result.Parents);
        }

        /// <summary>
        /// Detects if the graph contains any cycles using DFS.
        /// </summary>
        /// <returns>True if cycles exist, false otherwise</returns>
        public bool HasCycle()
        {
            return _traversal.HasCycle();
        }

        /// <summary>
        /// Finds all connected components in the graph.
        /// A connected component is a subgraph where all nodes are reachable from each other.
        /// </summary>
        /// <returns>List of connected components, each component is a list of nodes</returns>
        public List<List<Node<T>>> GetConnectedComponents()
        {
            return _traversal.GetConnectedComponents();
        }

        // === UTILITY METHODS ===

        /// <summary>
        /// Clears all nodes and edges from the graph.
        /// </summary>
        public void Clear()
        {
            _adjacencyList.Clear();
        }

        // === PUBLIC TRAVERSAL DATA ACCESS METHODS ===

        /// <summary>
        /// Gets DFS traversal data including visited nodes and parent mapping.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="target">Target node</param>
        /// <returns>Traversal result with visited nodes and parent mapping</returns>
        public TraversalResult<T> GetDFSTraversalData(Node<T> start, Node<T> target)
        {
            if (!NodeExists(start))
                return new TraversalResult<T>();

            return _traversal.GetDFSTraversalData(start, target);
        }

        /// <summary>
        /// Gets BFS traversal data including distances and parent mapping.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="target">Target node</param>
        /// <returns>Traversal result with distances and parent mapping</returns>
        public TraversalResult<T> GetBFSTraversalData(Node<T> start, Node<T> target)
        {
            if (!NodeExists(start))
                return new TraversalResult<T>();

            return _traversal.GetBFSTraversalData(start, target);
        }

        // === ADDITIONAL PUBLIC METHODS FOR TRAVERSAL DATA ===

        /// <summary>
        /// Gets all nodes reachable from a starting node using DFS.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <returns>List of all reachable nodes</returns>
        public List<Node<T>> GetAllReachableNodesDFS(Node<T> start)
        {
            if (!NodeExists(start))
                return new List<Node<T>>();

            return _traversal.GetAllReachableNodesDFS(start);
        }

        /// <summary>
        /// Gets all nodes reachable from a starting node using BFS.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <returns>Dictionary mapping nodes to their distances from start</returns>
        public Dictionary<Node<T>, int> GetAllReachableNodesBFS(Node<T> start)
        {
            if (!NodeExists(start))
                return new Dictionary<Node<T>, int>();

            return _traversal.GetAllReachableNodesBFS(start);
        }

        /// <summary>
        /// Gets all nodes reachable from a starting node using iterative DFS.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <returns>List of all reachable nodes</returns>
        public List<Node<T>> GetAllReachableNodesIterativeDFS(Node<T> start)
        {
            if (!NodeExists(start))
                return new List<Node<T>>();

            return _traversal.GetAllReachableNodesDFS(start); // Utilise la version itérative en interne
        }

        /// <summary>
        /// Checks if a target node is reachable from a starting node.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="target">Target node</param>
        /// <returns>True if target is reachable, false otherwise</returns>
        public bool IsReachable(Node<T> start, Node<T> target)
        {
            if (!NodeExists(start) || !NodeExists(target))
                return false;

            if (start == target)
                return true;

            var visited = _traversal.GetAllReachableNodesDFS(start);
            return visited.Contains(target);
        }

        /// <summary>
        /// Gets the minimum distance between two nodes.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="target">Target node</param>
        /// <returns>Minimum distance, -1 if target is unreachable</returns>
        public int GetDistance(Node<T> start, Node<T> target)
        {
            if (!NodeExists(start) || !NodeExists(target))
                return -1;

            if (start == target)
                return 0;

            var distances = _traversal.GetAllReachableNodesBFS(start);
            return distances.TryGetValue(target, out int distance) ? distance : -1;
        }

        /// <summary>
        /// Gets the graph type.
        /// </summary>
        /// <returns>The type of the graph</returns>
        public GraphType GetGraphType()
        {
            return _graphType;
        }

        // === PRIVATE HELPER METHODS ===

        /// <summary>
        /// Validates edge parameters and throws appropriate exceptions.
        /// </summary>
        private void ValidateEdgeParameters(Node<T> from, Node<T> to)
        {
            ArgumentNullException.ThrowIfNull(from, nameof(from));
            ArgumentNullException.ThrowIfNull(to, nameof(to));

            if (from == to)
                throw new InvalidGraphOperationException("Self-loops are not allowed", _graphType);
        }

        /// <summary>
        /// Validates path parameters and throws appropriate exceptions.
        /// </summary>
        private void ValidatePathParameters(Node<T> start, Node<T> end)
        {
            ArgumentNullException.ThrowIfNull(start, nameof(start));
            ArgumentNullException.ThrowIfNull(end, nameof(end));

            if (!NodeExists(start))
                throw new NodeNotFoundException(start.Name);

            if (!NodeExists(end))
                throw new NodeNotFoundException(end.Name);
        }

        /// <summary>
        /// Reconstructs a path from start to end using parent mapping.
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="end">Target node</param>
        /// <param name="parents">Parent mapping from traversal</param>
        /// <returns>List of nodes forming the path</returns>
        private List<Node<T>> ReconstructPath(Node<T> start, Node<T> end, Dictionary<Node<T>, Node<T>> parents)
        {
            var path = new List<Node<T>>();
            var current = end;

            while (current != null)
            {
                path.Insert(0, current);
                parents.TryGetValue(current, out current);
            }

            return path;
        }
    }
}