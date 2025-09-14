# GraphLibrary

Une bibliothèque de graphes générique pour .NET 8.0 qui supporte les graphes orientés et non orientés.

## Installation

### Via NuGet (recommandé)
```bash
dotnet add package GraphLibrary
```

### Via référence de projet
```bash
dotnet add reference path/to/GraphLibrary.dll
```

## Utilisation

### Créer un graphe non orienté
```csharp
using graph;

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
```

### Créer un graphe orienté
```csharp
var directedGraph = new GraphModel<int>(GraphType.Directed);

var node1 = new Node<int>("1", 1);
var node2 = new Node<int>("2", 2);

directedGraph.AddNode(node1);
directedGraph.AddNode(node2);
directedGraph.AddEdge(node1, node2); // Direction: 1 -> 2
```

### Trouver un chemin
```csharp
var path = graph.FindPath(startNode, endNode);
var shortestPath = graph.GetShortestPath(startNode, endNode);
```

### Vérifier la connectivité
```csharp
bool isConnected = graph.IsConnected(nodeA, nodeB);
bool isReachable = graph.IsReachable(startNode, targetNode);
```

## Fonctionnalités

- ✅ Graphes orientés et non orientés
- ✅ Recherche de chemins (DFS, BFS)
- ✅ Détection de cycles
- ✅ Composantes connexes
- ✅ Calcul de distances
- ✅ Gestion d'erreurs robuste
- ✅ Documentation XML complète

## Types supportés

La bibliothèque est générique et supporte tous les types de données :
- `GraphModel<string>`
- `GraphModel<int>`
- `GraphModel<CustomClass>`
- etc.

## Utilisation comme base de données en mémoire

La bibliothèque peut être utilisée pour modéliser une base de données en mémoire avec des relations complexes :

```csharp
using GraphLibrary.Database;

var db = new InMemoryDatabase();

// Insérer des données
var user = new { Name = "John", Email = "john@example.com" };
var product = new { Name = "Laptop", Price = 999.99m };

db.Insert("users", user);
db.Insert("products", product);

// Créer des relations
db.CreateRelation("users", userId, "orders", orderId);

// Requêtes avec relations
var userProducts = db.FindRelated<Product>("users", userId, "products");

// Analyse du graphe
var stats = db.GetStats();
Console.WriteLine($"Entités: {stats.TotalEntities}, Relations: {stats.TotalRelations}");
```

### Cas d'usage pour la base de données :

1. **E-commerce** : Relations User -> Order -> Product
2. **Réseaux sociaux** : Relations User -> User (amitiés)
3. **Systèmes de fichiers** : Relations Directory -> File
4. **Workflows** : Relations Step -> Step
5. **Graphes de connaissances** : Relations Entity -> Entity 