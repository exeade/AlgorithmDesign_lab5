namespace lab5;

public class GeneticColoring
{
    private readonly Graph _graph;
    private readonly Population _population;
    private readonly Crossover _crossover;
    private readonly Random _random;

    public GeneticColoring(int vertexCount, int populationSize)
    {
        _graph = new Graph(vertexCount);
        _population = new Population(populationSize, _graph, _graph.GetMaxColors());
        _crossover = new Crossover();
        _random = new Random();
    }

    private bool RunForColorCount(int maxColors, int maxIterations, int localImprovementStart, int localImprovementInterval)
    {
        _population.InitializePopulation(maxColors);

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            var bestChromosome = _population.Chromosomes.OrderBy(c => c.CalculateFitness()).First();
            int currentBestFitness = bestChromosome.CalculateFitness();
            
            if (currentBestFitness == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Solution found with ");
                Console.ForegroundColor = ConsoleColor.Yellow; 
                Console.Write($"{maxColors} ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("colors!");
                Console.ResetColor();
                PrintResults(iteration, bestChromosome);
                return true;
            }
            
            var (parent1, parent2) = _population.TournamentSelection(5);
            var (child1, child2) = _crossover.OnePointCrossover(parent1, parent2, _graph);

            double mutationProbability = iteration < 200 ? 0.65 : iteration < 400 ? 0.5 : 0.15;

            if (_random.NextDouble() < mutationProbability)
                child1.MutationSwap();
            if (_random.NextDouble() < mutationProbability)
                child2.MutationSwap();

            if (iteration >= localImprovementStart && (iteration - localImprovementStart) % localImprovementInterval == 0)
            {
                var randomChromosomes = _population.Chromosomes.OrderBy(_ => _random.Next()).Take(5).ToList();
                foreach (var chromosome in randomChromosomes)
                    chromosome.LocalMutation();
            }
            
            _population.RemoveWeakest(2);
            _population.AddChromosome(child1);
            _population.AddChromosome(child2);
            
            if (iteration % 100 == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{"Iteration", -4}: {iteration, 4},   " +
                                  $"{"Conflicts", -4}: {currentBestFitness, 4},   " +
                                  $"{"Colors", -4}: {bestChromosome.CalculateUnique(), 4}");
                Console.ResetColor();
            }
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkRed; 
        Console.Write("Failed to find a solution with ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{maxColors} ");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("colors within ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{maxIterations} ");
        Console.ForegroundColor = ConsoleColor.DarkRed; 
        Console.WriteLine("iterations.\n");
        Console.ResetColor();
        return false;
    }


    public void FindMinimumColoring(int maxIterations, int localImprovementStart, int localImprovementInterval)
    {
        int maxColors = _graph.GetMaxColors();

        while (maxColors > 1)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkBlue;  
            Console.Write("Trying to color with ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{maxColors} ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;  
            Console.WriteLine("colors...");
            Console.WriteLine();
            bool success = RunForColorCount(maxColors, maxIterations, localImprovementStart, localImprovementInterval);

            if (!success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Minimum coloring found with ");
                Console.ForegroundColor = ConsoleColor.Yellow; 
                Console.Write($"{maxColors + 1} ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("colors.");
                Console.ResetColor();
                break;
            }

            maxColors--;
        }
    }

    private void PrintResults(int iteration, Chromosome bestChromosome)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 100));
        Console.WriteLine($"Iteration: {iteration}\n");
        
        Console.WriteLine($"Number of conflicts: {bestChromosome.CalculateFitness()}");
        
        string text = "Number of colors used: ";

        Console.Write(text);
        
        string colors = "colors";
        foreach (char letter in colors)
        {
            if (letter == 'c')
                Console.ForegroundColor = ConsoleColor.Red;
            else if (letter == 'o')
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (letter == 'l')
                Console.ForegroundColor = ConsoleColor.Green;
            else if (letter == 'r')
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (letter == 's')
                Console.ForegroundColor = ConsoleColor.Cyan;

            Console.Write(letter);
        }
        
        Console.ResetColor();
        Console.WriteLine($" {bestChromosome.CalculateUnique()}\n");
        
        int totalWidth = 180;
        int padding = 3; 
        int contentWidth = totalWidth - padding * 2; 
        
        Console.Write(new string(' ', padding));
        
        for (int i = 0; i < bestChromosome.Genes.Length; i++)
        {
            if (bestChromosome.Genes[i] == 1)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (bestChromosome.Genes[i] == 2)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (bestChromosome.Genes[i] == 3)
                Console.ForegroundColor = ConsoleColor.Blue;
            else
                Console.ForegroundColor = ConsoleColor.Gray;
            
            Console.Write($"{bestChromosome.Genes[i]} ");
            
            if ((i + 1) % (contentWidth / 5) == 0)
            {
                Console.WriteLine();
                Console.Write(new string(' ', padding));
            }
        }

        Console.ResetColor();
        
        if (bestChromosome.Genes.Length % (contentWidth / 5) != 0)
            Console.WriteLine();
        Console.WriteLine(new string('=', 100));
    }

}
