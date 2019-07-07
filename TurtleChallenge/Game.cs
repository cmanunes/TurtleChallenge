using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    public class Game : IGame
    {
        private IGameSettings _gameSettings;
        private string gameSettingFileName = string.Empty;
        private string movesFileName = string.Empty;
        Position initialPosition = new Position();


        public Game(IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void Run(string gameSettingFileName, string movesFileName)
        {
            this.gameSettingFileName = gameSettingFileName;
            this.movesFileName = movesFileName;

            StartGame();
        }

        private void StartGame()
        {
            _gameSettings.GetGameSettings(gameSettingFileName);

            if (!_gameSettings.correctGameSettings)
            {
                Console.WriteLine("Incorrect game settings!");
            }
            else
            {
                _gameSettings.GetMoves(movesFileName);

                if (_gameSettings.movesList.Count == 0)
                {
                    Console.WriteLine("Incorret number of moves!");
                }
                else
                {
                    PlayGame();
                }
            }
        }
        
        private void PlayGame()
        {

            for (int k = 0; k < _gameSettings.sequences.Count; k++)
            {
                int counter = 1;
                bool continueGame = true;
                _gameSettings.movesList = (ArrayList)_gameSettings.sequences[k];
                int numberOfMoves = _gameSettings.movesList.Count;
                Position actualPosition = new Position
                {
                    direction = _gameSettings.initialPosition.direction,
                    point = new Point
                    {
                        x = _gameSettings.initialPosition.point.x,
                        y = _gameSettings.initialPosition.point.y
                    }
                };

                while (counter <= numberOfMoves && continueGame)
                {
                    if ((string)_gameSettings.movesList[counter - 1] == "r")
                    {
                        actualPosition.direction = UpdateDirection(actualPosition.direction);
                    }
                    if ((string)_gameSettings.movesList[counter - 1] == "m")
                    {
                        if (IsNextMovementOutsideBorders(actualPosition))
                        {
                            continueGame = false;
                            Console.WriteLine("Sequence "+(k+1).ToString()+": Invalid move!");
                        }

                        if (continueGame && HasNextMovementHitAMine(actualPosition))
                        {
                            continueGame = false;
                            Console.WriteLine("Sequence " + (k + 1).ToString() + ": Mine hit!");
                        }

                        if (continueGame && HasNextMovementReachedExit(actualPosition))
                        {
                            continueGame = false;
                            Console.WriteLine("Sequence " + (k + 1).ToString() + ": Success!");
                        }

                        if (continueGame)
                        {
                            actualPosition = UpdatePosition(actualPosition);
                        }
                    }

                    ++counter;
                }

                if (continueGame)
                {
                    Console.WriteLine("Sequence " + (k + 1).ToString() + ": Still in danger!");
                }
            }
        }

        public Position UpdatePosition(Position actualPosition)
        {
            Position pos = actualPosition;

            if (pos.direction == "west")
            {
                pos.point.x = pos.point.x - 1;
            }
            else if (pos.direction == "east")
            {
                pos.point.x = pos.point.x + 1;
            }
            else if (pos.direction == "north")
            {
                pos.point.y = pos.point.y - 1;
            }
            else if (pos.direction == "south")
            {
                pos.point.y = pos.point.y + 1;
            }

            return pos;
        }

        public string UpdateDirection(string direction)
        {
            string dir = direction;

            switch (direction)
            {
                case "north":
                    dir = "east";
                    break;
                case "east":
                    dir = "south";
                    break;
                case "south":
                    dir = "west";
                    break;
                case "west":
                    dir = "north";
                    break;
            }

            return dir;
        }

        public bool IsNextMovementOutsideBorders(Position actualPosition)
        {
            if (actualPosition.direction == "west" && actualPosition.point.x - 1 < 0)
            {
                return true;
            }
            else if (actualPosition.direction == "east" && actualPosition.point.x + 1 >= _gameSettings.Columns)
            {
                return true;
            }
            else if (actualPosition.direction == "north" && actualPosition.point.y - 1 < 0)
            {
                return true;
            }
            else if (actualPosition.direction == "south" && actualPosition.point.y + 1 >= _gameSettings.Rows)
            {
                return true;
            }

            return false;
        }

        public bool HasNextMovementHitAMine(Position actualPosition)
        {
            string newPointCoord = string.Empty;

            if (actualPosition.direction == "west")
            {
                newPointCoord = (actualPosition.point.x - 1).ToString() + "," + (actualPosition.point.y).ToString();
                if (_gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "east")
            {
                newPointCoord = (actualPosition.point.x + 1).ToString() + "," + (actualPosition.point.y).ToString();
                if (_gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "north")
            {
                 newPointCoord = (actualPosition.point.x).ToString() + "," + (actualPosition.point.y - 1).ToString();
                if (_gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "south")
            {
                newPointCoord = (actualPosition.point.x).ToString() + "," + (actualPosition.point.y + 1).ToString();
                if (_gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasNextMovementReachedExit(Position actualPosition)
        {
            if (actualPosition.direction == "west")
            {
                if (_gameSettings.exitPosition.x == actualPosition.point.x - 1 &&
                    _gameSettings.exitPosition.y == actualPosition.point.y)
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "east")
            {
                string newPointCoord = (actualPosition.point.x + 1).ToString() + "," + (actualPosition.point.y).ToString();
                if (_gameSettings.exitPosition.x == actualPosition.point.x + 1 &&
                    _gameSettings.exitPosition.y == actualPosition.point.y)
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "north")
            {
                string newPointCoord = (actualPosition.point.x).ToString() + "," + (actualPosition.point.y - 1).ToString();
                if (_gameSettings.exitPosition.x == actualPosition.point.x &&
                    _gameSettings.exitPosition.y == actualPosition.point.y - 1)
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "south")
            {
                if (_gameSettings.exitPosition.x == actualPosition.point.x &&
                    _gameSettings.exitPosition.y == actualPosition.point.y + 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
