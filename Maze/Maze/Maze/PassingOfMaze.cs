using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Maze
{
    public class PassingOfMaze : IPassingMaze
    {
        protected readonly ObjectsInMaze[,] maze;
        protected Point currentPoint;
        protected Point startPoint;
        protected readonly List<Point> finishPoints = new List<Point>();
        protected Orientation currentOrientation;

        protected bool isStarted = false;

        protected enum Orientation
        {
            Left,
            Top,
            Right,
            Bottom
        }

        public ObjectsInMaze[,] Maze
        {
            get
            {
                return maze;
            }
        }

        public Point Current
        {
            get
            {
                return currentPoint;
            }
        }

        public Point StartPoint
        {
            get
            {
                return startPoint;
            }
        }

        public List<Point> FinishPoints
        {
            get
            {
                return finishPoints;
            }
        }

        public PassingOfMaze(ObjectsInMaze[,] maze, Point startPoint, Point[] finishPoints)
        {
            this.maze = maze;
            this.currentPoint = startPoint;
            for (int i = 0; i < finishPoints.Length; i++)
            {
                this.finishPoints.Add(finishPoints[i]);
            }
            this.startPoint = startPoint;
            currentOrientation = Orientation.Right;
        }

        public PassingOfMaze(ObjectsInMaze[,] maze, Point startPoint, Point finishPoint)
        {
            this.maze = maze;
            this.currentPoint = startPoint;
            this.finishPoints.Add(finishPoint);
            this.startPoint = startPoint;
            currentOrientation = Orientation.Right;
        }

        public PassingOfMaze(ObjectsInMaze[,] maze)
        {
            Point[] inputsAndOutputs = GetInputAndOutputs(maze);
            if (inputsAndOutputs.Length > 1)
            {
                this.maze = maze;
                this.startPoint = inputsAndOutputs[0];
                this.currentPoint = startPoint;
                for (int i = 1; i < inputsAndOutputs.Length; i++)
                {
                    this.finishPoints.Add(inputsAndOutputs[i]);
                }
                currentOrientation = Orientation.Right;
            }
            else
            {
                throw new Exception("В лабиринте нет выхода.");
            }
        }

        public PassingOfMaze(ObjectsInMaze[,] maze, Point startPoint)
        {
            Point[] inputsAndOutputs = GetInputAndOutputs(maze);
            if (inputsAndOutputs.Length > 1)
            {
                this.maze = maze;
                this.startPoint = startPoint;
                this.currentPoint = startPoint;
                for (int i = 0; i < inputsAndOutputs.Length; i++)
                {
                    if (inputsAndOutputs[i] != startPoint)
                        this.finishPoints.Add(inputsAndOutputs[i]);
                }
                currentOrientation = Orientation.Right;
            }
            else
            {
                throw new Exception("В лабиринте нет выхода.");
            }
        }

        public static Point[] GetInputAndOutputs(ObjectsInMaze[,] maze)
        {
            List<Point> points = new List<Point>();

            int rows = maze.GetLength(0);
            int columns = maze.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                if (maze[i, 0] != ObjectsInMaze.Wall)
                    points.Add(new Point(0, i));

                if (maze[i, columns - 1] != ObjectsInMaze.Wall)
                    points.Add(new Point(columns - 1, i));
            }

            for (int i = 0; i < columns; i++)
            {
                if (maze[0, i] != ObjectsInMaze.Wall)
                    points.Add(new Point(i, 0));

                if (maze[rows - 1, i] != ObjectsInMaze.Wall)
                    points.Add(new Point(i, rows - 1));
            }

            return points.ToArray();
        }

        public virtual bool MoveNext()
        {
            if (finishPoints.Contains(Current))
                return false;

            if (isStarted && currentPoint == startPoint)
                throw new Exception("В лабиринте нет выхода. Либо к нему нет прохода.");

            if (currentOrientation == Orientation.Right)
            {
                if (currentPoint.Y + 1 < maze.GetLength(0) && maze[currentPoint.Y + 1, currentPoint.X] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Bottom;
                    currentPoint.Y++;
                    return true;
                }
                else
                if (currentPoint.X + 1 < maze.GetLength(1) && maze[currentPoint.Y, currentPoint.X + 1] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Right;
                    currentPoint.X++;
                    return true;
                }
                else
                {
                    currentOrientation = Orientation.Top;
                    return MoveNext();
                }
            }
            else if (currentOrientation == Orientation.Top)
            {
                if (currentPoint.X + 1 < maze.GetLength(1) && maze[currentPoint.Y, currentPoint.X + 1] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Right;
                    currentPoint.X++;
                    return true;
                }
                else if (currentPoint.Y - 1 >= 0 && maze[currentPoint.Y - 1, currentPoint.X] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Top;
                    currentPoint.Y--;
                    return true;
                }
                else
                {
                    currentOrientation = Orientation.Left;
                    return MoveNext();
                }
            }
            else if (currentOrientation == Orientation.Left)
            {
                if (currentPoint.Y - 1 >= 0 && maze[currentPoint.Y - 1, currentPoint.X] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Top;
                    currentPoint.Y--;
                    return true;
                }
                else if (currentPoint.X - 1 >= 0 && maze[currentPoint.Y, currentPoint.X - 1] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Left;
                    currentPoint.X--;
                    return true;
                }
                else
                {
                    currentOrientation = Orientation.Bottom;
                    return MoveNext();
                }
            }
            else if (currentOrientation == Orientation.Bottom)
            {
                if (currentPoint.X - 1 >= 0 && maze[currentPoint.Y, currentPoint.X - 1] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Left;
                    currentPoint.X--;
                    return true;
                }
                else if (currentPoint.Y + 1 < maze.GetLength(0) && maze[currentPoint.Y + 1, currentPoint.X] != ObjectsInMaze.Wall)
                {
                    isStarted = true;
                    currentOrientation = Orientation.Bottom;
                    currentPoint.Y++;
                    return true;
                }
                else
                {
                    currentOrientation = Orientation.Right;
                    return MoveNext();
                }
            }

            return false;
        }

    }
}
