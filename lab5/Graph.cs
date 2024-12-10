namespace lab5;
public class Graph
{
    public readonly int VertexCount;
    private readonly Dictionary<int, HashSet<int>> _adjacencyList;

    public Graph(int vertexCount)
    {
        if (vertexCount < 1)
            throw new ArgumentException("The number of vertices must be positive.", nameof(vertexCount));

        VertexCount = vertexCount;
        _adjacencyList = new Dictionary<int, HashSet<int>>();

        for (int i = 0; i < vertexCount; i++)
        {
            _adjacencyList[i] = new HashSet<int>();
        }

        GenerateGraph();
    }
    
    private void GenerateGraph()
    {
        Random random = new Random();

        for (int vertex = 0; vertex < VertexCount; vertex++)
        {
            int targetDegree = random.Next(2, 31);

            while (_adjacencyList[vertex].Count < targetDegree)
            {
                int neighbor = random.Next(VertexCount);
                
                if (neighbor != vertex && !_adjacencyList[vertex].Contains(neighbor))
                {
                    AddEdge(vertex, neighbor);
                }
            }
        }
        
        for (int vertex = 0; vertex < VertexCount; vertex++)
        {
            while (_adjacencyList[vertex].Count > 30)
            {
                int neighbor = _adjacencyList[vertex].First();
                RemoveEdge(vertex, neighbor);
            }
        }
    }

    private void AddEdge(int vertex1, int vertex2)
    {
        if (vertex1 == vertex2)
            throw new ArgumentException("Self-loops are not allowed (vertex1 cannot be equal to vertex2).");

        if (vertex1 < 0 || vertex1 >= VertexCount || vertex2 < 0 || vertex2 >= VertexCount)
            throw new ArgumentException("Vertices must be within the valid range.");

        _adjacencyList[vertex1].Add(vertex2);
        _adjacencyList[vertex2].Add(vertex1);
    }

    private void RemoveEdge(int vertex1, int vertex2)
    {
        if (vertex1 < 0 || vertex1 >= VertexCount || vertex2 < 0 || vertex2 >= VertexCount)
            throw new ArgumentException("Vertices must be within the valid range.");

        _adjacencyList[vertex1].Remove(vertex2);
        _adjacencyList[vertex2].Remove(vertex1);
    }
    
    public IEnumerable<int> GetNeighbors(int vertex)
    {
        if (vertex < 0 || vertex >= VertexCount)
            throw new ArgumentException("Vertex must be within the valid range.");

        return _adjacencyList[vertex];
    }
    
    public bool AreAdjacent(int vertex1, int vertex2)
    {
        if (vertex1 < 0 || vertex1 >= VertexCount || vertex2 < 0 || vertex2 >= VertexCount)
            throw new ArgumentException("Vertices must be within the valid range.");

        return _adjacencyList[vertex1].Contains(vertex2);
    }
    
    public int GetMaxColors()
    {
        int maxColors = 0;
        
        for (int i = 0; i < VertexCount; i++)
        {
            int currentColors = 0;
            
            foreach (var _ in _adjacencyList[i])
            {
                currentColors++;
            }
            
            maxColors = Math.Max(maxColors, currentColors);
        }

        return maxColors;
    }
    
}
