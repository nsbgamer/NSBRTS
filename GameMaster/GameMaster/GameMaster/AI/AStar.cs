using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace GameMaster.AI
{
    class AStar
    {
        #region Members

        private float[,] boardValues;
        private Container[,] boardContainer;
        private Point boardSize;
        private List<Point> boardPathway;

        private Point[] movements = new Point[]
        {
            new Point(-1, -1),
            new Point(0, -1),
            new Point(1, -1),
            new Point(1, 0),
            new Point(1, 1),
            new Point(0, 1),
            new Point(-1, 1),
            new Point(-1, 0)
        };

        #endregion

        public AStar(Point _size)
        {
            boardSize = _size;
            ClearBoard();
        }

        public List<Point> FindPath(Container[,] _container, Point _start, Point _end)
        {
            ClearBoard();

            if (ValidateBoard(_start, _end) == true)
            {
                CopyBoard(_container);
                RolloutBoard(ClosestPointStart(_start));
                ReadBoard(ClosestPointStart(_start), ClosestPointEnd(_end));
            }

            return boardPathway;
        }

        public void ClearBoard()
        {
            boardValues = new float[boardSize.X, boardSize.Y];
            boardContainer = new Container[boardSize.X, boardSize.Y];
            boardPathway = new List<Point>();
        }

        public bool ValidateBoard(Point _start, Point _end)
        {
            if (ValidCoordinates(_start.X, _start.Y) && ValidCoordinates(_end.X, _end.Y))
                return true;
            return false;
        }

        public void CopyBoard(Container[,] _container)
        {
            for(int x = 0; x < boardSize.X; x++)
                for (int y = 0; y < boardSize.Y; y++)
                {
                    boardValues[x, y] = 10000;
                    boardContainer[x, y] = _container[x, y];
                }
        }

        public void RolloutBoard(Point _start)
        {
            if(ValidCoordinates(_start.X, _start.Y))
            {
                boardValues[_start.X, _start.Y] = 0;
                Rollout(_start);
            }
        }

        public void Rollout(Point _current)
        {
            float passHere = boardValues[_current.X, _current.Y];
            foreach (Point movePoint in ValidMoves(_current.X, _current.Y))
            {
                float newPass = passHere + Distance(_current, movePoint);

                if (boardValues[movePoint.X, movePoint.Y] > newPass)
                {
                    boardValues[movePoint.X, movePoint.Y] = newPass;
                    Rollout(movePoint);
                }
            }
        }

        public void ReadBoard(Point _start, Point _end)
        {
            int pointX = _end.X;
            int pointY = _end.Y;
            int counter = 0;

            if (_end.X == -1 || _end.Y == -1 || _start.X == -1 || _start.Y == -1)
                return;

            //Process so the game keeps running until it finds a path or runs out of possible moves
            while (true)
            {
                Point lowestPoint = Point.Zero; //sets the point to empty
                float lowest = 10000;

                foreach (Point movePoint in ValidMoves(pointX, pointY))
                {
                    float count = boardValues[movePoint.X, movePoint.Y]; //checks lowest possible step

                    if (count < lowest)
                    {
                        lowest = count;
                        lowestPoint.X = movePoint.X;
                        lowestPoint.Y = movePoint.Y;
                    }
                }

                if (lowest != 10000)
                {
                    counter++;
                    boardPathway.Add(lowestPoint);
                    pointX = lowestPoint.X;
                    pointY = lowestPoint.Y;
                }
                else
                    break;

                if (lowestPoint == _start)
                {
                    boardPathway.Add(lowestPoint);
                    break;
                }
            }

            if (boardPathway.Count > 0)
            {
                //you need to reverse the list so that you can begin from the start rather than the end
                boardPathway.Reverse();
                boardPathway.Add(_end);
            }
        }

        #region Extras

        private IEnumerable<Point> AllSquares()
        {
            for (int x = 0; x < boardSize.X; x++)
                for (int y = 0; y < boardSize.Y; y++)
                    yield return new Point(x, y);
        }

        private IEnumerable<Point> ValidMoves(int _x, int _y)
        {
            foreach (Point movePoint in movements)
            {
                int newX = _x + movePoint.X;
                int newY = _y + movePoint.Y;

                if (SquareOpen(newX, newY) && CheckAdjacent(_x, _y, newX, newY))
                    yield return new Point(newX, newY);

            }
        }

        private bool SquareOpen(int _x, int _y)
        {
            if (ValidCoordinates(_x, _y))
            {
                switch (boardContainer[_x, _y])
                {
                    case Container.Empty:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        private bool CheckAdjacent(int _x, int _y, int _newX, int _newY)
        {
            int xshift = _x - _newX;
            int yshift = _y - _newY;

            if (ValidCoordinates(_newX, _newY))
            {
                if (boardContainer[_newX, _newY] == Container.Empty)
                {
                    if (xshift != 0 && yshift != 0)
                    {
                        if (boardContainer[_newX + xshift, _newY] == Container.Wall)
                            return false;
                        if (boardContainer[_newX, _newY + yshift] == Container.Wall)
                            return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool ValidCoordinates(int _x, int _y)
        {
            //coordinates are currently constrained between 0 and 14
            if (_x < 0)
                return false;
            if (_y < 0)
                return false;
            if (_x > boardSize.X - 1)
                return false;
            if (_y > boardSize.Y - 1)
                return false;

            return true;
        }

        private float Distance(Point _one, Point _two)
        {
            return (float)Math.Sqrt(Math.Pow(_one.X - _two.X, 2) + Math.Pow(_one.Y - _two.Y, 2));
        }

        private Point ClosestPointStart(Point _start)
        {
            Point returnable = _start;

            if (boardContainer[_start.X, _start.Y] == Container.Wall)
            {
                int pass = 0;
                float bestValue = 10000;
                bool breakOut = false;

                while (true)
                {
                    pass++;

                    for (int x = -pass; x < pass + 1; x++)
                    {
                        for (int y = -pass; y < pass + 1; y++)
                        {
                            int newX = _start.X + x;
                            int newY = _start.Y + y;

                            if (ValidCoordinates(newX, newY))
                            {
                                if (boardContainer[newX, newY] == Container.Empty)
                                {
                                    returnable = new Point(newX, newY);
                                    bestValue = boardValues[newX, newY];
                                    breakOut = true;
                                }
                            }
                        }
                    }

                    //extremely unlikely, but only if there are no available points.
                    //just to stop the loop
                    if (pass > boardSize.X + boardSize.Y)
                        return new Point(-1, -1);

                    if (breakOut == true)
                    {
                        return returnable;
                    }
                }
            }
            return returnable;
        }

        private Point ClosestPointEnd(Point _end)
        {
            Point returnable = _end;

            if (boardValues[_end.X, _end.Y] == 10000)
            {
                int pass = 0;
                float bestValue = 10000;
                bool breakOut = false;

                while (true)
                {
                    pass++;

                    for (int x = -pass; x < pass + 1; x++)
                    {
                        for (int y = -pass; y < pass + 1; y++)
                        {
                            int newX = _end.X + x;
                            int newY = _end.Y + y;

                            if (ValidCoordinates(newX, newY))
                            {
                                if (boardContainer[newX, newY] == Container.Empty && boardValues[newX, newY] < bestValue)
                                {
                                    returnable = new Point(newX, newY);
                                    bestValue = boardValues[newX, newY];
                                    breakOut = true;
                                }
                            }
                        }
                    }
                }

                //Extremely unlikely
                //Only if there are no available points and we need to stop the loop
                if (pass > boardSize.X + boardSize.Y)
                    return new Point(-1, -1);

                if (breakOut == true)
                    return returnable;
            }
            return returnable;
        }
        #endregion

        //this is just an extra method that will display the values for each spot on the board
        //use this for error trapping
        public void DrawValues(SpriteBatch _spriteBatch, SpriteFont _spriteFont)
        {
            for (int x = 0; x < boardSize.X; x++)
                for (int y = 0; y < boardSize.Y; y++)
                    _spriteBatch.DrawString(_spriteFont, "" + Math.Round(boardValues[x, y], 1), new Vector2(x * 32, y * 32), Color.Black, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
        }
    }
}
