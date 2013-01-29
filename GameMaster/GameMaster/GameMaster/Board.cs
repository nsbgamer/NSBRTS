using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaster.AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GameMaster
{
    class Board
    {
        #region Members

        private AStar aStar;
        private Point size;
        private Cell[,] cells;
        private int selected; 
        
        private AiControlled player;
        private AiControlled enemy;

        private Texture2D currTexture;
        private Texture2D emptyTexture;
        private Texture2D enemyTexture;
        private Texture2D playerTexture;
        private Texture2D wallTexture;

        private MouseState mouseCurr, mousePrev;
        private KeyboardState keyboardCurr, keyboardPrev;

        private SpriteFont font;

        private bool chase;

        #endregion

        public Board(Point _size)
        {
            size = _size;
            aStar = new AStar(size);
            cells = new Cell[size.X, size.Y];
            selected = 0;
            chase = false;
        }

        public void LoadContent(ContentManager _content)
        {
            font = _content.Load<SpriteFont>("Arial");
            emptyTexture = _content.Load<Texture2D>("Sprites/Empty");
            wallTexture = _content.Load<Texture2D>("Sprites/Wall");
            playerTexture = _content.Load<Texture2D>("Sprites/PlayerTest");
            enemyTexture = _content.Load<Texture2D>("Sprites/EnemyTest");

            for(int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                {
                    cells[x, y] = new Cell(new Rectangle(x * 32, y * 32, 32, 32));//, 2, new Point(0, 0));
                    cells[x, y].LoadContent(_content);
                }

            enemy = new AiControlled(enemyTexture, new Rectangle(0, 0, 32, 32), 2, new Point(0, 0));
            player = new AiControlled(playerTexture, new Rectangle(32, 0, 32, 32), 2, new Point(1, 0));
            Show();
        }

        public void Update()
        {
            player.Update();
            enemy.Update();

            mouseCurr = Mouse.GetState();
            keyboardCurr = Keyboard.GetState();

            if (keyboardCurr.IsKeyDown(Keys.Space) && keyboardPrev.IsKeyUp(Keys.Space))
            {
                if (chase == true)
                    chase = false;
                else
                    chase = true;
            }

            if (player.Moved == true && chase == true)
                enemy.UpdatePath(aStar.FindPath(Compose(), enemy.CellCurr, player.CellCurr));
            if (keyboardCurr.IsKeyDown(Keys.R) && keyboardPrev.IsKeyDown(Keys.R))
                Reset();
            if (mouseCurr.ScrollWheelValue > mousePrev.ScrollWheelValue)
            {
                selected++;
                Show();
            }
            if (mouseCurr.ScrollWheelValue < mousePrev.ScrollWheelValue)
            {
                selected--;
                Show();
            }
            if (mouseCurr.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                Place();

            mousePrev = mouseCurr;
            keyboardPrev = keyboardCurr;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                    cells[x, y].Draw(_spriteBatch);
            enemy.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            _spriteBatch.Draw(currTexture, new Rectangle(mouseCurr.X - 16, mouseCurr.Y - 16, 32, 32), Color.White);

            aStar.DrawValues(_spriteBatch, font);

            _spriteBatch.DrawString(font, "Left click to move\nRight click to place\nScroll wheel to select\nPress R to reset board\nPress Space to start chase", new Vector2(500, 20), Color.Black);
        }

        public void Reset()
        {
            enemy = new AiControlled(enemyTexture, new Rectangle(0, 0, 32, 32), 1, new Point(0, 0));
            player = new AiControlled(playerTexture, new Rectangle(32, 0, 32, 32), 2, new Point(1, 0));
            aStar.ClearBoard();

            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                    cells[x, y].Reset();
        }

        public void Show()
        {
            if (selected > 3)
                selected = 0;
            if (selected < 0)
                selected = 3;

            switch (selected)
            {
                case 0: currTexture = emptyTexture;
                    break;
                case 1: currTexture = wallTexture;
                    break;
                case 2: currTexture = playerTexture;
                    break;
                case 3: currTexture = enemyTexture;
                    break;
            }
        }

        public void Place()
        {
            for(int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                {
                    if (cells[x, y].Rect.Contains(new Point(mouseCurr.X, mouseCurr.Y)))
                    {
                        switch (selected)
                        {
                            case 0: cells[x, y].Contain = Container.Empty;
                                break;
                            case 1: cells[x, y].Contain = Container.Wall;
                                break;
                            case 2: player.Position = new Vector2(cells[x, y].Rect.X, cells[x, y].Rect.Y);
                                player.ClearPath(new Point(x, y));
                                break;
                            case 3: enemy.Position = new Vector2(cells[x, y].Rect.X, cells[x, y].Rect.Y);
                                enemy.ClearPath(new Point(x, y));
                                break;
                        }
                    }
                }
        }

        public Container[,] Compose()
        {
            Container[,] returnable = new Container[size.X, size.Y];

            for(int x = 0; x < size.X; x++)
                for(int y = 0; y < size.Y; y++)
                    returnable[x,y] = cells[x,y].Contain;

            return returnable;
        }

        public Point Discover(Point _point)
        {
            for(int x = 0; x < size.X; x++)
                for(int y = 0; y < size.Y; y++)
                {
                    if(cells[x,y].Rect.Contains(_point))
                        return new Point(x,y);
                }
            return new Point(-1, -1);
        }

        public void UserControl()
        {
        }

        private bool SquareOpen(int _x, int _y)
        {
            if(ValidCoordinates(_x, _y))
            {
                switch (cells[_x, _y].Contain)
                {
                    case Container.Empty:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        public bool ValidCoordinates(int _x, int _y)
        {
            if (_x < 0)
                return false;
            if (_y < 0)
                return false;
            if(_x > size.X - 1)
                return false;
            return true;
        }

    }
}
