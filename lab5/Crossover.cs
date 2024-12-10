namespace lab5;
public class Crossover
{
    private readonly Random _random = new();

    public (Chromosome, Chromosome) OnePointCrossover(Chromosome parent1, Chromosome parent2, Graph graph)
    {
        int crossoverPoint = _random.Next(1, parent1.Genes.Length);

        var child1Genes = parent1.Genes.Take(crossoverPoint).Concat(parent2.Genes.Skip(crossoverPoint)).ToArray();
        var child2Genes = parent2.Genes.Take(crossoverPoint).Concat(parent1.Genes.Skip(crossoverPoint)).ToArray();

        var child1 = new Chromosome(child1Genes, graph);
        var child2 = new Chromosome(child2Genes, graph);

        return (child1, child2);
    }
        
    public (Chromosome, Chromosome) TwoPointCrossover(Chromosome parent1, Chromosome parent2, Graph graph)
    {
        int crossoverPoint1 = _random.Next(1, parent1.Genes.Length);
        int crossoverPoint2 = _random.Next(crossoverPoint1 + 1, parent1.Genes.Length);

        var child1Genes = parent1.Genes.Take(crossoverPoint1).Concat(parent2.Genes.Skip(crossoverPoint1)
            .Take(crossoverPoint2 - crossoverPoint1)).Concat(parent1.Genes.Skip(crossoverPoint2)).ToArray();
        var child2Genes = parent2.Genes.Take(crossoverPoint1).Concat(parent1.Genes.Skip(crossoverPoint1)
            .Take(crossoverPoint2 - crossoverPoint1)).Concat(parent2.Genes.Skip(crossoverPoint2)).ToArray();

        var child1 = new Chromosome(child1Genes, graph);
        var child2 = new Chromosome(child2Genes, graph);

        return (child1, child2);
    }

    public Chromosome UniformCrossover(Chromosome parent1, Chromosome parent2, Graph graph)
    {
        var childGenes = new int[parent1.Genes.Length];
        for (int i = 0; i < parent1.Genes.Length; i++)
        {
            childGenes[i] = _random.Next(2) == 0 ? parent1.Genes[i] : parent2.Genes[i];
        }

        var child = new Chromosome(childGenes, graph);
        return child;
    }
}