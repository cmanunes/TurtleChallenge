using System.Collections.Generic;

namespace TurtleChallenge
{
    public class GameSettings
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Position initialPosition { get; set; }
        public Point exitPosition { get; set; }
        public Dictionary<string, string> mines = new Dictionary<string, string>();
    }
}
