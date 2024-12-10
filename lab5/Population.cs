namespace lab5;
public class Population
{
    public List<Chromosome> Chromosomes { get; private set; }
    private int PopulationSize { get; }
    private readonly Graph _graph;
    private readonly Random _random;

    public Population(int populationSize, Graph graph, int maxColors)
    {
        _random = new Random();
        PopulationSize = populationSize;
        _graph = graph;
        Chromosomes = new List<Chromosome>();
        InitializePopulation(maxColors);
    }

    public void InitializePopulation(int maxColors)
    {
        Chromosomes.Clear();
        for (int i = 0; i < PopulationSize; i++)
        {
            Chromosomes.Add(new Chromosome(_graph.VertexCount, _graph, maxColors));
        }
    }

    public (Chromosome, Chromosome) TournamentSelection(int tournamentSize)
    {
        var tournamentParticipants = new List<Chromosome>();
        for (int i = 0; i < tournamentSize; i++)
        {
            int randomIndex = _random.Next(Chromosomes.Count);
            tournamentParticipants.Add(Chromosomes[randomIndex]);
        }

        var sortedParticipants = tournamentParticipants.OrderBy(chromosome =>
            chromosome.CalculateFitness()).ToList();
        
        return (sortedParticipants[0], sortedParticipants[1]);
    }

    public void RemoveWeakest(int count)
    {
        Chromosomes = Chromosomes.OrderBy(c => c.CalculateFitness()).Take(PopulationSize - count).ToList();
    }

    public void AddChromosome(Chromosome chromosome)
    {
        if (Chromosomes.Count < PopulationSize)
            Chromosomes.Add(chromosome);
    }
}