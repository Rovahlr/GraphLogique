namespace graph
{
    /// <summary>
    /// Base exception for graph-related errors.
    /// </summary>
    public class GraphException : Exception
    {
        public GraphException(string message) : base(message) { }
        public GraphException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when a node is not found in the graph.
    /// </summary>
    public class NodeNotFoundException : GraphException
    {
        public NodeNotFoundException(string nodeName) : base($"Node '{nodeName}' not found in the graph") { }
    }

    /// <summary>
    /// Exception thrown when an operation is invalid for the graph type.
    /// </summary>
    public class InvalidGraphOperationException : GraphException
    {
        public InvalidGraphOperationException(string operation, GraphType graphType) 
            : base($"Operation '{operation}' is not valid for {graphType} graph") { }
    }

    /// <summary>
    /// Exception thrown when no path exists between two nodes.
    /// </summary>
    public class NoPathExistsException : GraphException
    {
        public NoPathExistsException(string fromNode, string toNode) 
            : base($"No path exists from '{fromNode}' to '{toNode}'") { }
    }
} 