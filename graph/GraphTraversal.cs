namespace graph
{
    /// <summary>
    /// Provides graph traversal algorithms (DFS, BFS) for graph analysis.
    /// </summary>
    /// <typeparam name="T">The type of data stored in graph nodes</typeparam>
    public class GraphTraversal<T>
    {
        private readonly Dictionary<Node<T>, List<Node<T>>> _adjacencyList;

        public GraphTraversal(Dictionary<Node<T>, List<Node<T>>> adjacencyList)
        {
            _adjacencyList = adjacencyList ?? throw new ArgumentNullException(nameof(adjacencyList));
        }

        // === MÉTHODES PUBLIQUES ===

        /// <summary>
        /// Gets all nodes reachable from a starting node using DFS.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <returns>List of all reachable nodes</returns>
        public List<Node<T>> GetAllReachableNodesDFS(Node<T> startNode)
        {
            return DepthFirstSearch(startNode);
        }

        /// <summary>
        /// Gets all nodes reachable from a starting node using BFS with distances.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <returns>Dictionary mapping nodes to their distances from start</returns>
        public Dictionary<Node<T>, int> GetAllReachableNodesBFS(Node<T> startNode)
        {
            return BreadthFirstSearch(startNode);
        }

        /// <summary>
        /// Gets DFS traversal data including visited nodes and parent mapping.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <param name="targetNode">Target node</param>
        /// <returns>Traversal result with visited nodes and parent mapping</returns>
        public TraversalResult<T> GetDFSTraversalData(Node<T> startNode, Node<T> targetNode)
        {
            return DepthFirstSearchWithParents(startNode, targetNode);
        }

        /// <summary>
        /// Gets BFS traversal data including distances and parent mapping.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <param name="targetNode">Target node</param>
        /// <returns>Traversal result with distances and parent mapping</returns>
        public TraversalResult<T> GetBFSTraversalData(Node<T> startNode, Node<T> targetNode)
        {
            return BreadthFirstSearchWithParents(startNode, targetNode);
        }

        /// <summary>
        /// Detects cycles in the graph using DFS.
        /// </summary>
        /// <returns>True if cycles exist, false otherwise</returns>
        public bool HasCycle()
        {
            var visited = new HashSet<Node<T>>();
            var recursionStack = new HashSet<Node<T>>();

            foreach (var node in _adjacencyList.Keys)
            {
                if (!visited.Contains(node))
                {
                    if (HasCycleIterative(node, visited, recursionStack))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds all connected components in the graph.
        /// </summary>
        /// <returns>List of connected components</returns>
        public List<List<Node<T>>> GetConnectedComponents()
        {
            var components = new List<List<Node<T>>>();
            var visited = new HashSet<Node<T>>();

            foreach (var node in _adjacencyList.Keys)
            {
                if (!visited.Contains(node))
                {
                    var component = new List<Node<T>>();
                    DepthFirstSearchIterativeForComponent(node, component, visited);
                    components.Add(component);
                }
            }

            return components;
        }

        // === MÉTHODES PRIVÉES ===

        /// <summary>
        /// Performs Depth-First Search starting from a given node.
        /// </summary>
        /// <param name="startNode">The starting node for DFS</param>
        /// <returns>List of all nodes visited during DFS</returns>
        private List<Node<T>> DepthFirstSearch(Node<T> startNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
                return new List<Node<T>>();

            var visited = new HashSet<Node<T>>();
            var result = new List<Node<T>>();
            var stack = new Stack<Node<T>>();
            stack.Push(startNode);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!visited.Contains(current))
                {
                    visited.Add(current);
                    result.Add(current);
                    foreach (var neighbor in _adjacencyList[current])
                    {
                        if (!visited.Contains(neighbor))
                            stack.Push(neighbor);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs iterative Depth-First Search (non-recursive).
        /// </summary>
        /// <param name="startNode">The starting node for DFS</param>
        /// <returns>List of all nodes visited during DFS</returns>
        private List<Node<T>> DepthFirstSearchIterative(Node<T> startNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
                return new List<Node<T>>();

            var visited = new HashSet<Node<T>>();
            var result = new List<Node<T>>();
            var stack = new Stack<Node<T>>();
            stack.Push(startNode);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!visited.Contains(current))
                {
                    visited.Add(current);
                    result.Add(current);
                    foreach (var neighbor in _adjacencyList[current])
                    {
                        if (!visited.Contains(neighbor))
                            stack.Push(neighbor);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs Breadth-First Search starting from a given node.
        /// </summary>
        /// <param name="startNode">The starting node for BFS</param>
        /// <returns>Dictionary mapping nodes to their distances from start</returns>
        private Dictionary<Node<T>, int> BreadthFirstSearch(Node<T> startNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
                return new Dictionary<Node<T>, int>();

            var distances = new Dictionary<Node<T>, int>();
            var queue = new Queue<Node<T>>();
            
            queue.Enqueue(startNode);
            distances[startNode] = 0;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var neighbor in _adjacencyList[current])
                {
                    if (!distances.ContainsKey(neighbor))
                    {
                        distances[neighbor] = distances[current] + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return distances;
        }

        /// <summary>
        /// Performs DFS with parent tracking for path reconstruction.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <param name="targetNode">Target node to find</param>
        /// <returns>Traversal result with visited nodes and parent mapping</returns>
        private TraversalResult<T> DepthFirstSearchWithParents(Node<T> startNode, Node<T> targetNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
                return new TraversalResult<T>();

            var visited = new HashSet<Node<T>>();
            var visitedList = new List<Node<T>>();
            var parents = new Dictionary<Node<T>, Node<T>>();
            var stack = new Stack<Node<T>>();
            
            stack.Push(startNode);
            visited.Add(startNode);
            visitedList.Add(startNode);
            bool targetReached = false;

            while (stack.Count > 0 && !targetReached)
            {
                var current = stack.Pop();
                
                if (current == targetNode)
                {
                    targetReached = true;
                    break;
                }
                
                foreach (var neighbor in _adjacencyList[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        visitedList.Add(neighbor);
                        parents[neighbor] = current;
                        stack.Push(neighbor);
                    }
                }
            }

            return new TraversalResult<T>(visitedList, parents, new Dictionary<Node<T>, int>(), targetReached);
        }

        /// <summary>
        /// Performs BFS with parent tracking for path reconstruction.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <param name="targetNode">Target node to find</param>
        /// <returns>Traversal result with distances and parent mapping</returns>
        private TraversalResult<T> BreadthFirstSearchWithParents(Node<T> startNode, Node<T> targetNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
                return new TraversalResult<T>();

            var distances = new Dictionary<Node<T>, int>();
            var parents = new Dictionary<Node<T>, Node<T>>();
            var queue = new Queue<Node<T>>();
            
            queue.Enqueue(startNode);
            distances[startNode] = 0;
            bool targetReached = false;

            while (queue.Count > 0 && !targetReached)
            {
                var current = queue.Dequeue();
                
                if (current == targetNode)
                {
                    targetReached = true;
                    break;
                }
                
                foreach (var neighbor in _adjacencyList[current])
                {
                    if (!distances.ContainsKey(neighbor))
                    {
                        distances[neighbor] = distances[current] + 1;
                        parents[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return new TraversalResult<T>(new List<Node<T>>(distances.Keys), parents, distances, targetReached);
        }

        /// <summary>
        /// Iterative DFS implementation for finding connected components.
        /// </summary>
        private void DepthFirstSearchIterativeForComponent(Node<T> startNode, List<Node<T>> component, HashSet<Node<T>> visited)
        {
            if (!_adjacencyList.ContainsKey(startNode)) return;
            
            var stack = new Stack<Node<T>>();
            stack.Push(startNode);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!visited.Contains(current))
                {
                    visited.Add(current);
                    component.Add(current);
                    
                    foreach (var neighbor in _adjacencyList[current])
                    {
                        if (!visited.Contains(neighbor))
                            stack.Push(neighbor);
                    }
                }
            }
        }

        private bool HasCycleIterative(Node<T> startNode, HashSet<Node<T>> visited, HashSet<Node<T>> recursionStack)
        {
            var stack = new Stack<(Node<T> node, bool isBacktrack)>();
            stack.Push((startNode, false));

            while (stack.Count > 0)
            {
                var (currentNode, isBacktrack) = stack.Pop();

                if (isBacktrack)
                {
                    // Backtracking: remove from recursion stack
                    recursionStack.Remove(currentNode);
                    continue;
                }

                if (visited.Contains(currentNode))
                {
                    if (recursionStack.Contains(currentNode))
                        return true; // Cycle detected
                    continue;
                }

                visited.Add(currentNode);
                recursionStack.Add(currentNode);

                // Push backtrack marker
                stack.Push((currentNode, true));

                // Push all neighbors
                foreach (var neighbor in _adjacencyList[currentNode])
                {
                    stack.Push((neighbor, false));
                }
            }

            return false;
        }
    }
} 