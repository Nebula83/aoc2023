namespace day_21;

class Program
{
    static void Main(string[] args)
    {
        Day1.Run();
        // Day2.Run();
        var p2 = new Solution().PartTwo(File.ReadAllText("input.txt"));
        Console.WriteLine(p2);
    }
}
