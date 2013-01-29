using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameMaster.AI
{
    class AiControlled
    {
        #region Members

        private float speed;
        private Vector2 position;
        private bool moved;
        Point cellCurr;
        Point cellPrev;
        private Rectangle rect;
        private Texture2D texture;
        private List<Point> pathway;

        #endregion

        #region Accessors

        public float Speed { get { return speed; } set { speed = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public bool Moved { get { return moved; } set { moved = value; } }
        public Point CellCurr { get { return cellCurr; } }
        public Point CellPrev { get { return cellPrev; } }

        #endregion

        public AiControlled(Texture2D _texture, Rectangle _rect, float _speed, Point _start)
        {
            cellPrev = _start;
            cellCurr = _start;
            position = new Vector2(_rect.X, _rect.Y);
            pathway = new List<Point>();
            texture = _texture;
            rect = _rect;
            speed = _speed;
        }

        public void Update()
        {
            if (pathway.Count > 0)
            {
                if (Vector2.Distance(Position, new Vector2(pathway[0].X * (int)GridStats.WIDTH, pathway[0].Y * (int)GridStats.HEIGHT)) > Speed)
                {
                    position += (Vector2.Normalize(new Vector2(pathway[0].X * (int)GridStats.WIDTH, pathway[0].Y * (int)GridStats.HEIGHT) - Position));
                    Moved = false;

                    if (CellCurr != pathway[0])
                    {
                        cellPrev = CellCurr;
                        cellCurr = pathway[0];
                        Moved = true;
                    }
                }
                else
                    NextPoint();

            }
        }

        public void UpdatePath(List<Point> _path)
        {
            pathway = _path;
        }

        public void UpdatePath(Point _point)
        {
            pathway = new List<Point>();
            pathway.Add(_point);
        }

        public void ClearPath(Point _point)
        {
            cellPrev = cellCurr;
            cellCurr = _point;
            pathway.Clear();
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            _spriteBatch.Draw(texture, rect, Color.White);
        }

        public void NextPoint()
        {
            Position = new Vector2(pathway[0].X * (int)GridStats.WIDTH, pathway[0].Y * (int)GridStats.HEIGHT);
            pathway.RemoveAt(0);
        }

    }
}
