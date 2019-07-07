using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    public interface IGame
    {
        Position UpdatePosition(Position actualPosition);

        string UpdateDirection(string direction);

        bool IsNextMovementOutsideBorders(Position actualPosition);

        bool HasNextMovementHitAMine(Position actualPosition);

        bool HasNextMovementReachedExit(Position actualPosition);
    }
}
