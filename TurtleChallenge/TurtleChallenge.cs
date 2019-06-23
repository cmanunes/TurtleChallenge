using System;

namespace TurtleChallenge
{
    public class TurtleChallenge
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[0] != "" && args[1] != "")
                {
                    Game game = new Game(args[0], args[1]);
                }
            }
            else
            {
                Console.WriteLine("This application accepts 2 parameters. 1st parameter value should the file name for game-settings and 2nd parameter should have the file name for the moves");
                return;
            }
        }
    }
}
