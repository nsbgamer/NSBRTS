using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameMaster
{
    class Cell
    {
        #region Members

        private Container contain;
        private Texture2D emptyTexture;
        private Texture2D wallTexture;
        private Texture2D outline;
        private Rectangle rect;

        #endregion

        #region Accessors

        public Rectangle Rect { get { return rect; } }
        public Container Contain { get { return contain; } set { contain = value; } }

        #endregion

        public Cell(Rectangle _rect)
        {
            rect = _rect;
            contain = Container.Empty;
        }

        public void LoadContent(ContentManager _content)
        {
            emptyTexture = _content.Load<Texture2D>("Sprites/Empty");
            wallTexture = _content.Load<Texture2D>("Sprites/Wall");
            outline = _content.Load<Texture2D>("Sprites/Outline");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (contain == Container.Empty)
                _spriteBatch.Draw(emptyTexture, rect, Color.White);
            else
                _spriteBatch.Draw(wallTexture, rect, Color.White);
            _spriteBatch.Draw(outline, rect, Color.White);
        }

        public void Reset()
        {
            contain = Container.Empty;
        }
    }
}
