using graph;

namespace GraphLibrary.Tests
{
    /// <summary>
    /// Exemples d'utilisation de la bibliothèque de graphes
    /// </summary>
    public static class GraphExamples
    {
        /// <summary>
        /// Exemple d'utilisation d'un graphe non orienté
        /// </summary>
        public static void ExampleUndirectedGraph()
        {
            var graph = new GraphModel<string>(GraphType.Undirected);
            
            // Créer des nœuds
            var a = new Node<string>("A", "Nœud A");
            var b = new Node<string>("B", "Nœud B");
            var c = new Node<string>("C", "Nœud C");
            
            // Ajouter des nœuds
            graph.AddNode(a);
            graph.AddNode(b);
            graph.AddNode(c);
            
            // Ajouter des arêtes
            graph.AddEdge(a, b);
            graph.AddEdge(b, c);
            
            // Utiliser les fonctionnalités
            var path = graph.FindPath(a, c);
            var isConnected = graph.IsConnected(a, c);
        }
        
        /// <summary>
        /// Exemple d'utilisation d'un graphe orienté
        /// </summary>
        public static void ExampleDirectedGraph()
        {
            var graph = new GraphModel<int>(GraphType.Directed);
            
            var node1 = new Node<int>("1", 1);
            var node2 = new Node<int>("2", 2);
            var node3 = new Node<int>("3", 3);
            
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            
            graph.AddEdge(node1, node2);
            graph.AddEdge(node2, node3);
            
            var shortestPath = graph.GetShortestPath(node1, node3);
        }
    }
} 