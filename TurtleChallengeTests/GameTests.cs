using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TurtleChallenge;

namespace TurtleChallengeTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void WhenDirectionIsNorth_UpdatingDirection_ShouldChangeToEast()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "north", point = new Point { x = 0, y = 0 }
            };

            //Act
            actualPosition.direction = game.UpdateDirection(actualPosition.direction);

            //Assert
            Assert.AreEqual("east", actualPosition.direction);
        }

        [TestMethod]
        public void WhenDirectionIsEast_UpdatingDirection_ShouldChangeToSouth()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "east",
                point = new Point { x = 0, y = 0 }
            };

            //Act
            actualPosition.direction = game.UpdateDirection(actualPosition.direction);

            //Assert
            Assert.AreEqual("south", actualPosition.direction);
        }

        [TestMethod]
        public void WhenDirectionIsSouth_UpdatingDirection_ShouldChangeToWest()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "south",
                point = new Point { x = 0, y = 0 }
            };

            //Act
            actualPosition.direction = game.UpdateDirection(actualPosition.direction);

            //Assert
            Assert.AreEqual("west", actualPosition.direction);
        }

        [TestMethod]
        public void WhenDirectionIsWest_UpdatingDirection_ShouldChangeToNorth()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "west",
                point = new Point { x = 0, y = 0 }
            };

            //Act
            actualPosition.direction = game.UpdateDirection(actualPosition.direction);

            //Assert
            Assert.AreEqual("north", actualPosition.direction);
        }

        [TestMethod]
        public void WhenDirectionIsNorth_UpdatingPosition_ShouldDecreaseY()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "north",
                point = new Point { x = 1, y = 1 }
            };

            //Act
            actualPosition = game.UpdatePosition(actualPosition);

            //Assert
            Assert.AreEqual(0, actualPosition.point.y);
        }

        [TestMethod]
        public void WhenDirectionIsSouth_UpdatingPosition_ShouldIncreaseY()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "south",
                point = new Point { x = 1, y = 1 }
            };

            //Act
            actualPosition = game.UpdatePosition(actualPosition);

            //Assert
            Assert.AreEqual(2, actualPosition.point.y);
        }

        [TestMethod]
        public void WhenDirectionIsWest_UpdatingPosition_ShouldDecreaseX()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "west",
                point = new Point { x = 1, y = 1 }
            };

            //Act
            actualPosition = game.UpdatePosition(actualPosition);

            //Assert
            Assert.AreEqual(0, actualPosition.point.x);
        }

        [TestMethod]
        public void WhenDirectionIsEast_UpdatingPosition_ShouldIncreaseX()
        {
            //Arrange
            Mock<IGameSettings> _gameSettings = new Mock<IGameSettings>();
            Game game = new Game(_gameSettings.Object);
            Position actualPosition = new Position
            {
                direction = "east",
                point = new Point { x = 1, y = 1 }
            };

            //Act
            actualPosition = game.UpdatePosition(actualPosition);

            //Assert
            Assert.AreEqual(2, actualPosition.point.x);
        }
    }
}
