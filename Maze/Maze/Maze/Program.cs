using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Windows;

namespace Maze
{
    class Program
    {
        static int maxWidth = Console.LargestWindowWidth - 5;
        static int maxHeight = Console.LargestWindowHeight - 5;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WindowWidth = maxWidth;
            Console.WindowHeight = maxHeight;

            try
            {
                Console.WriteLine("Добро пожаловать в приложение для прохождения лабиринта.");
                Console.WriteLine("Пожалуйста, выберите файл(изображение) с лабиринтом.");

                ObjectsInMaze[,] maze = GetMaze();
                
                Console.Clear();

                Point[] inputsAndOutputs = PassingOfMaze.GetInputAndOutputs(maze);



                if (inputsAndOutputs.Length > 1)
                {
                    if (inputsAndOutputs.Length < 10)
                        PrintMaze(maze, new Point(-1, -1), inputsAndOutputs, true);
                    else
                        PrintMaze(maze, new Point(-1, -1), inputsAndOutputs);
                    Console.WriteLine("Перед собой вы видите лабиринт. Пожалуйста выберите с какой точки начать(напишите цифру 1 или 2 или....):");
                    for (int i = 0; i < inputsAndOutputs.Length; i++)
                    {
                        Console.WriteLine(i + 1 + ". " + inputsAndOutputs[i].ToString());
                    }

                    int number = 0;
                    bool correct = false;
                    do
                    {
                        int.TryParse(Console.ReadLine(), out number);

                        if (number <= inputsAndOutputs.Length && number > 0)
                        {
                            correct = true;
                        }
                        else
                        {
                            Console.WriteLine("По пробуйте еще раз");
                        }
                    } while (!correct);

                    Console.Clear();
                    Console.CursorVisible = false;
                    PassingOfMaze passing = new PassingOfMaze(maze, inputsAndOutputs[number - 1]);
                    PrintMaze(maze, passing.StartPoint, passing.FinishPoints.ToArray());
                    Console.WriteLine("Для старта нажмите Enter");
                    Console.ReadLine();
                    StartPassingMaze(passing);
                }
                else
                {
                    Console.WriteLine("Извините, но в лабиринте нет выхода. По пробуйте выбрать другой файл");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Для выхода нажмите Enter...");
            }


            Console.ReadKey();
        }

        static ObjectsInMaze[,] GetMaze()
        {
            ObjectsInMaze[,] maze = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "png files (*.png)|*.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(openFileDialog.FileName);

                if (bitmap.Width > maxWidth - 1 || bitmap.Height > maxHeight - 1)
                {
                    throw new Exception("Превышены максимально допустимые размеры файла. Width = " + (maxWidth -1) + ", Height = "+ (maxHeight - 1));
                }

                maze = new ObjectsInMaze[bitmap.Height, bitmap.Width];

                for (int i = 0; i < bitmap.Height; i++)
                {

                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        Color color = bitmap.GetPixel(j, i);
                        if (color.ToArgb() == Color.White.ToArgb())
                        {
                            maze[i, j] = ObjectsInMaze.Path;
                        }
                        else if (color.ToArgb() == Color.Black.ToArgb())
                        {
                            maze[i, j] = ObjectsInMaze.Wall;
                        }
                    }
                }
            }

            return maze;
        }

        static void StartPassingMaze(PassingOfMaze passing)
        {
            try
            {
                int steps = 0;
                do
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");

                    Console.SetCursorPosition(passing.Current.X, passing.Current.Y);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(" ");

                    Console.SetCursorPosition(passing.Current.X, passing.Current.Y);
                    int sleep = Convert.ToInt32(ConfigurationManager.AppSettings["sleep"]);
                    Thread.Sleep(sleep);
                    steps++;

                } while (passing.MoveNext());

                Console.CursorLeft = 0;
                Console.CursorTop = passing.Maze.GetLength(0) + 1;

                Console.WriteLine("Финиш: x= {0}, y= {1}, Количество пройденных шагов = {2}", passing.Current.X, passing.Current.Y, steps);
            }
            catch (Exception ex)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = passing.Maze.GetLength(0) + 1;
                Console.WriteLine(ex.Message);
            }
        }

        static void PrintMaze(ObjectsInMaze[,] maze, Point startPoint, Point[] finishPoints, bool printPositionNumber = false)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    ObjectsInMaze point = maze[i, j];
                    if (point == ObjectsInMaze.Wall)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }

                    if (startPoint != new Point(-1, -1))
                    {
                        if (j == startPoint.X && i == startPoint.Y)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                        }
                    }

                    string text = " ";

                    for (int k = 0; k < finishPoints.Length; k++)
                    {
                        if (j == finishPoints[k].X && i == finishPoints[k].Y)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            if (printPositionNumber)
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                text = (k + 1).ToString();
                            }
                                
                        }
                    }


                    Console.Write(text);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }
    }
}
