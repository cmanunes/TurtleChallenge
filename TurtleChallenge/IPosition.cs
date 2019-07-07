using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    interface IPosition
    {
        Point point { get; set; }
        string direction { get; set; }
    }
}
