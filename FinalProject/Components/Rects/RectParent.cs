using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace FinalProject.Components
{
    public enum name { GROUND, WALL, PIT };

    public class RectParent : SpriteCharacter
    {
        protected int height;
        protected int Height { get { return height; } set { height = value; } }

        protected int width;
        protected int Width { get { return width; } set { width = value; } }

        

        protected name rectName;
        public name RectName { get { return rectName; } }

        public RectParent(Game game)
            : base(game)
        {
            
        }

        public void Shape(int rectCount)
        {
            x = (width * rectCount);
            shapeRect = new Rectangle((int)x, (int)y, width, height);
            this.locationRect = shapeRect;
        }
    }
}
