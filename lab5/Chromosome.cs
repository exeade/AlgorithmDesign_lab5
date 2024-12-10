namespace lab5;
public class Chromosome
{
    public int[] Genes { get; private set; }
    private Graph Graph { get; }
    private int MaxColors { get; }

    public Chromosome(int n, Graph graph, int maxColors)
    {
        Genes = new int[n];
        Graph = graph;
        MaxColors = maxColors;
        GenerateRandomChromosome();
    }

    public Chromosome(int[] genes, Graph graph)
    {
        Genes = genes.ToArray();
        Graph = graph;
        MaxColors = genes.Max();
    }

    private void GenerateRandomChromosome()
    {
        Random rand = new Random();
        for (int i = 0; i < Graph.VertexCount; i++)
        {
            Genes[i] = rand.Next(1, MaxColors + 1);
        }
    }

    public int CalculateUnique()
    {
        return Genes.Distinct().Count();
    }

    public void MutationSwap()
    {
        Random rand = new Random();
        int index1 = rand.Next(Genes.Length);
        int index2;

        do
        {
            index2 = rand.Next(Genes.Length);
        } while (index1 == index2);
        
        (Genes[index1], Genes[index2]) = (Genes[index2], Genes[index1]);
    }

    public void MutationReplace()
    {
        Random rand = new Random();
        int index = rand.Next(Genes.Length);
        int newColor;

        do
        {
            newColor = rand.Next(1, MaxColors + 1);
        } while (Genes[index] == newColor);

        Genes[index] = newColor;
    }

    public int CalculateFitness()
    {
        int conflictCount = 0;

        for (int i = 0; i < Graph.VertexCount; i++)
        {
            foreach (var neighbor in Graph.GetNeighbors(i))
            {
                if (Genes[i] == Genes[neighbor])
                {
                    conflictCount++;
                }
            }
        }
        
        return conflictCount / 2;
    }

    public void LocalMutation()
    {
        Random rand = new Random();
        var conflictingVertices = new List<int>();
        for (int i = 0; i < Graph.VertexCount; i++)
        {
            foreach (var neighbor in Graph.GetNeighbors(i))
            {
                if (Genes[i] == Genes[neighbor])
                {
                    conflictingVertices.Add(i);
                    break;
                }
            }
        }

        if (conflictingVertices.Count == 0)
            return;

        int vertexToMutate = conflictingVertices[rand.Next(conflictingVertices.Count)];
        var availableColors = Enumerable.Range(1, MaxColors).ToList();
        foreach (var neighbor in Graph.GetNeighbors(vertexToMutate))
        {
            availableColors.Remove(Genes[neighbor]);
        }

        if (availableColors.Count > 0)
        {
            Genes[vertexToMutate] = availableColors[rand.Next(availableColors.Count)];
        }
    }
    
    public void ImprovedLocalMutation()
    {
        Random rand = new Random();
        var bestFitness = CalculateFitness();
        var bestGenes = Genes.ToArray();

        for (int i = 0; i < Graph.VertexCount; i++)
        {
            int oldColor = Genes[i];
            var availableColors = Enumerable.Range(1, MaxColors).ToList();
            
            foreach (var neighbor in Graph.GetNeighbors(i))
            {
                availableColors.Remove(Genes[neighbor]);
            }
            
            if (availableColors.Count > 0)
            {
                Genes[i] = availableColors[rand.Next(availableColors.Count)];
                
                if (CalculateFitness() < bestFitness)
                {
                    bestFitness = CalculateFitness();
                    bestGenes = Genes.ToArray();
                }
                else
                {
                    Genes[i] = oldColor;
                }
            }
        }
        
        Genes = bestGenes;
    }
}