namespace graph
{
    /// <summary>
    /// Represents the result of a graph traversal operation.
    /// </summary>
    /// <typeparam name="T">The type of data stored in graph nodes</typeparam>
    public class TraversalResult<T>
    {
        /// <summary>
        /// List of nodes visited during traversal.
        /// </summary>
        public List<Node<T>> VisitedNodes { get; set; } = new();

        /// <summary>
        /// Mapping of nodes to their parent nodes for path reconstruction.
        /// </summary>
        public Dictionary<Node<T>, Node<T>> Parents { get; set; } = new();

        /// <summary>
        /// Mapping of nodes to their distances from the start node.
        /// </summary>
        public Dictionary<Node<T>, int> Distances { get; set; } = new();

        /// <summary>
        /// Whether the target node was reached during traversal.
        /// </summary>
        public bool TargetReached { get; set; }

        /// <summary>
        /// Creates a new traversal result.
        /// </summary>
        public TraversalResult() { }

        /// <summary>
        /// Creates a new traversal result with initial data.
        /// </summary>
        public TraversalResult(List<Node<T>> visitedNodes, Dictionary<Node<T>, Node<T>> parents, Dictionary<Node<T>, int> distances, bool targetReached = false)
        {
            VisitedNodes = visitedNodes ?? new List<Node<T>>();
            Parents = parents ?? new Dictionary<Node<T>, Node<T>>();
            Distances = distances ?? new Dictionary<Node<T>, int>();
            TargetReached = targetReached;
        }
    }

    /// <summary>
    /// Represents the result of a path finding operation.
    /// </summary>
    /// <typeparam name="T">The type of data stored in graph nodes</typeparam>
    public class PathResult<T>
    {
        /// <summary>
        /// The path as a list of nodes.
        /// </summary>
        public List<Node<T>> Path { get; set; } = new();

        /// <summary>
        /// The total distance/cost of the path.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Whether a path exists between the start and end nodes.
        /// </summary>
        public bool Exists => Path.Count > 0;

        /// <summary>
        /// Creates a new path result.
        /// </summary>
        public PathResult() { }

        /// <summary>
        /// Creates a new path result with initial data.
        /// </summary>
        public PathResult(List<Node<T>> path, int distance = 0)
        {
            Path = path ?? new List<Node<T>>();
            Distance = distance;
        }
    }
} 