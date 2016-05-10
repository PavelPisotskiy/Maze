using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze;
using System.Drawing;

namespace MazeUnitTest
{
    [TestClass]
    public class PassingOfMazeTest
    {
        [TestMethod]
        public void PassingTest()
        {
            ObjectsInMaze[,] maze = new ObjectsInMaze[,]
            {
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Path,ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Path },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },

            };

            IPassingMaze passingMaze = new PassingOfMaze(maze, new Point(2, 4), new Point(0, 2));

            do
            {
            } while (passingMaze.MoveNext());

            Assert.AreEqual(passingMaze.FinishPoints[0], passingMaze.Current);
        }

        [TestMethod]
        public void AutomaticFindingTheFinishTest()
        {
            ObjectsInMaze[,] maze = new ObjectsInMaze[,]
            {
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Path,ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Path },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },

            };

            IPassingMaze passingMaze = new PassingOfMaze(maze, new Point(2, 4));

            do
            {

            } while (passingMaze.MoveNext());

            Assert.AreEqual(3, passingMaze.FinishPoints.Count);
            CollectionAssert.Contains(passingMaze.FinishPoints, new Point(0, 2));
            CollectionAssert.Contains(passingMaze.FinishPoints, new Point(4, 2));
            CollectionAssert.Contains(passingMaze.FinishPoints, new Point(2, 0));

            CollectionAssert.Contains(passingMaze.FinishPoints, passingMaze.Current);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "В лабиринте нет выхода.")]
        public void TheFinishIsNotFoundTest()
        {

            ObjectsInMaze[,] maze = new ObjectsInMaze[,]
            {
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Wall, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },

            };

            IPassingMaze passingMaze = new PassingOfMaze(maze, new Point(2, 4));

            do
            {

            } while (passingMaze.MoveNext());

            Assert.AreEqual(0, passingMaze.FinishPoints.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "В лабиринте нет выхода. Либо к нему нет прохода.")]
        public void CanNotPathToTheFinishTest()
        {

            ObjectsInMaze[,] maze = new ObjectsInMaze[,]
            {
                {ObjectsInMaze.Wall,ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Path, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },
                {ObjectsInMaze.Wall,ObjectsInMaze.Wall, ObjectsInMaze.Path, ObjectsInMaze.Wall, ObjectsInMaze.Wall },

            };

            IPassingMaze passingMaze = new PassingOfMaze(maze, new Point(2, 4), new Point(1, 0));

            do
            {

            } while (passingMaze.MoveNext());

            Assert.AreEqual(1, passingMaze.FinishPoints.Count);

        }
    }
}
