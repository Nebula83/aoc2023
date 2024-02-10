using System.IO;

class Day2 : Day1
{    
    internal static new void Run()
    {
        var day = new Day2();
        // day.ParseFile("test-1.txt");
        // day.ParseFile("test-2.txt");
        day.ParseFile("input.txt");

        day.DropItLikeItsHot();

        var result = 0;
        foreach (var kapow in day._bricks)
        {
            var queue =  new Queue<Brick>();
            var sand = new Dictionary<int, bool>();

            queue.Enqueue(kapow);
            sand[kapow.Index] = true;
            
            while (queue.Count > 0)
            {
                var brick = queue.Dequeue();
                foreach (var supported in brick.Supports)
                {
                    if (sand.ContainsKey(supported.Index))
                    { 
                        continue;
                    }

                    var supportsTurnedSand = supported.SupportedBy.Where(b => sand.ContainsKey(b.Index)).Count();
                    if (supportsTurnedSand == supported.SupportedBy.Count)
                    {
                        sand[supported.Index] = true;
                    }
                    
                    queue.Enqueue(supported);
                }
            }
            result += sand.Count - 1;
        }

        Console.WriteLine($"Result 2: {result}");
    }
}

