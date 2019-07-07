using System.Collections;
using System.Collections.Generic;

namespace TurtleChallenge
{
    public interface IGameSettings
    {
        int Columns { get; set; }
        int Rows { get; set; }
        Position initialPosition { get; set; }
        Point exitPosition { get; set; }
        Dictionary<string, string> mines { get; set; }
        bool correctGameSettings { get; set; }
        ArrayList sequences { get; set; }
        ArrayList movesList { get; set; }

        void GetMoves(string movesFileName);

        void GetGameSettings(string gameSettingFileName);
    }
}
