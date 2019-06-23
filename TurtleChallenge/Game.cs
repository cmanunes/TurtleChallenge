using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    public class Game
    {
        private GameSettings gameSettings = new GameSettings();
        private ArrayList movesList = new ArrayList();
        private ArrayList sequences = new ArrayList();
        private bool correctGameSettings = true;
        private string gameSettingFileName = string.Empty;
        private string movesFileName = string.Empty;
        Position initialPosition = new Position();


        public Game(string gameSettingFileName, string movesFileName)
        {
            this.gameSettingFileName = gameSettingFileName;
            this.movesFileName = movesFileName;

            StartGame();
        }

        private void GetMoves()
        {
            if (File.Exists(movesFileName))
            {
                using (var fileStream = File.OpenRead(movesFileName))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 1024))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        switch (line.ToLowerInvariant())
                        {
                            case "begin":
                                movesList = new ArrayList();
                                break;
                            case "end":
                                sequences.Add(movesList);
                                break;
                            default:
                                if (line != "" && (line == "r" || line == "m"))
                                {
                                    movesList.Add(line);
                                }
                                break;
                        }
                    }                    
                }
            }
            else
            {
                correctGameSettings = false;
                Console.WriteLine("Moves file does not exists!");
            }
        }

        private void GetGameSettings()
        {
            if (File.Exists(gameSettingFileName))
            {
                using (var fileStream = File.OpenRead(gameSettingFileName))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 1024))
                {
                    int lineNumber = 1;
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        switch (lineNumber)
                        {
                            case 1: // number of columns and rows
                                var data = line.Trim().Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                if (data.Length == 2)
                                {
                                    gameSettings.Columns = data[0];
                                    gameSettings.Rows = data[1];
                                }
                                else
                                {
                                    correctGameSettings = false;
                                    return;
                                }
                                break;
                            case 2: // initial position
                                var initPosition = line.Trim().Split(',');
                                if (initPosition.Length == 3)
                                {
                                    gameSettings.initialPosition = new Position
                                    {
                                        point = new Point
                                        {
                                            x = Convert.ToInt32(initPosition[0]),
                                            y = Convert.ToInt32(initPosition[1])
                                        },
                                        direction = initPosition[2].ToLowerInvariant()
                                    };

                                    initialPosition = new Position
                                    {
                                        point = new Point
                                        {
                                            x = Convert.ToInt32(initPosition[0]),
                                            y = Convert.ToInt32(initPosition[1])
                                        },
                                        direction = initPosition[2].ToLowerInvariant()
                                    };
                                }
                                else
                                {
                                    correctGameSettings = false;
                                    return;
                                }
                                break;
                            case 3: // exit position
                                var exitPosition = line.Trim().Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                if (exitPosition.Length == 2)
                                {
                                    gameSettings.exitPosition = new Point
                                    {
                                        x = exitPosition[0],
                                        y = exitPosition[1]
                                    };
                                }
                                else
                                {
                                    correctGameSettings = false;
                                    return;
                                }
                                break;
                            default: // mines
                                if (line.Trim() != "")
                                {
                                    string minePosition = line.Trim();
                                    if (!gameSettings.mines.ContainsKey(minePosition))
                                    {
                                        gameSettings.mines.Add(minePosition, minePosition);
                                    }
                                }
                                break;
                        }

                        ++lineNumber;
                    }

                }
            }
            else
            {
                correctGameSettings = false;
                Console.WriteLine("Game settings file does not exists!");
            }
        }

        private void StartGame()
        {
            GetGameSettings();

            if (!correctGameSettings)
            {
                Console.WriteLine("Incorrect game settings!");
            }
            else
            {
                GetMoves();

                if (movesList.Count == 0)
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

            for (int k = 0; k < sequences.Count; k++)
            {
                int counter = 1;
                bool continueGame = true;
                movesList = (ArrayList)sequences[k];
                int numberOfMoves = movesList.Count;
                Position actualPosition = new Position
                {
                    direction = initialPosition.direction,
                    point = new Point
                    {
                        x = initialPosition.point.x,
                        y = initialPosition.point.y
                    }
                };

                while (counter <= numberOfMoves && continueGame)
                {
                    if ((string)movesList[counter - 1] == "r")
                    {
                        actualPosition.direction = UpdateDirection(actualPosition.direction);
                    }
                    if ((string)movesList[counter - 1] == "m")
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
            else if (actualPosition.direction == "east" && actualPosition.point.x + 1 >= gameSettings.Columns)
            {
                return true;
            }
            else if (actualPosition.direction == "north" && actualPosition.point.y - 1 < 0)
            {
                return true;
            }
            else if (actualPosition.direction == "south" && actualPosition.point.y + 1 >= gameSettings.Rows)
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
                if (gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "east")
            {
                newPointCoord = (actualPosition.point.x + 1).ToString() + "," + (actualPosition.point.y).ToString();
                if (gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "north")
            {
                 newPointCoord = (actualPosition.point.x).ToString() + "," + (actualPosition.point.y - 1).ToString();
                if (gameSettings.mines.ContainsKey(newPointCoord))
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "south")
            {
                newPointCoord = (actualPosition.point.x).ToString() + "," + (actualPosition.point.y + 1).ToString();
                if (gameSettings.mines.ContainsKey(newPointCoord))
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
                if (gameSettings.exitPosition.x == actualPosition.point.x - 1 &&
                    gameSettings.exitPosition.y == actualPosition.point.y)
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "east")
            {
                string newPointCoord = (actualPosition.point.x + 1).ToString() + "," + (actualPosition.point.y).ToString();
                if (gameSettings.exitPosition.x == actualPosition.point.x + 1 &&
                    gameSettings.exitPosition.y == actualPosition.point.y)
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "north")
            {
                string newPointCoord = (actualPosition.point.x).ToString() + "," + (actualPosition.point.y - 1).ToString();
                if (gameSettings.exitPosition.x == actualPosition.point.x &&
                    gameSettings.exitPosition.y == actualPosition.point.y - 1)
                {
                    return true;
                }
            }
            else if (actualPosition.direction == "south")
            {
                if (gameSettings.exitPosition.x == actualPosition.point.x &&
                    gameSettings.exitPosition.y == actualPosition.point.y + 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
