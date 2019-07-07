using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace TurtleChallenge
{
    public class GameSettings : IGameSettings
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Position initialPosition { get; set; }
        public Point exitPosition { get; set; }
        public Dictionary<string, string> mines { get; set; }
        public bool correctGameSettings { get; set; }
        public ArrayList sequences { get; set; }
        public ArrayList movesList { get; set; }

        public GameSettings ()
        {
            correctGameSettings = true;
        }
        public void GetMoves(string movesFileName)
        {
            sequences = new ArrayList();
            movesList = new ArrayList();

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

        public void GetGameSettings(string gameSettingFileName)
        {
            if (File.Exists(gameSettingFileName))
            {
                mines = new Dictionary<string, string>();
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
                                    Columns = data[0];
                                    Rows = data[1];
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
                                var exitPos = line.Trim().Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                if (exitPos.Length == 2)
                                {
                                    exitPosition = new Point
                                    {
                                        x = exitPos[0],
                                        y = exitPos[1]
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
                                    if (!mines.ContainsKey(minePosition))
                                    {
                                        mines.Add(minePosition, minePosition);
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
    }
}
