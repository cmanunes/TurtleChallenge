using System;
using StructureMap;
using StructureMap.Graph;

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
                    var container = Container.For<InitIoC>();
                    var app = container.GetInstance<Game>();
                    app.Run(args[0], args[1]);

                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("This application accepts 2 parameters. 1st parameter value should the file name for game-settings and 2nd parameter should have the file name for the moves");
                return;
            }
        }
    }

    public class InitIoC : Registry
    {
        public InitIoC()
        {
            Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
            // requires explicit registration; doesn't follow convention
            For<IGameSettings>().Use<GameSettings>();
            For<IPosition>().Use<Position>();
        }
    }
}
