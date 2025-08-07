namespace graph
{
    /// <summary>
    /// Interface defining the basic contract for a graph implementation.
    /// </summary>
    /// <typeparam name="T">The type of data stored in graph nodes</typeparam>
    public interface IGraph<T>
    {
        // Basic graph operations
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
        
        // Utility
        void Clear();
    }
} 