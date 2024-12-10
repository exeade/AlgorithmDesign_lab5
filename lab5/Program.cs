namespace lab5;

static class Program
{
    static void Main()
    {
        int vertexCount = 100;
        int populationSize = 50;
        int maxIterations = 3000;
        int localImprovementStart = 100;
        int localImprovementInterval = 10;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==============================================");
        Console.WriteLine("  Graph Coloring Problem Solving Using      ");
        Console.WriteLine("          Genetic Algorithm                 ");
        Console.WriteLine("==============================================");

        Console.ResetColor();
        var geneticAlgorithm = new GeneticColoring(vertexCount, populationSize);
        geneticAlgorithm.FindMinimumColoring(maxIterations, localImprovementStart, localImprovementInterval);
        
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();
    }
}