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

namespace FinalProject.Components.Rects
{
    public class VerticalRect : RectParent
    {
        public VerticalRect(Game game, int rectCount)
            : base(game)
        {
            SpriteTexture = content.Load<Texture2D>("YellowTile");
            rectName = name.WALL;
            Width = 100;
            Height = 200;
            Y = 468;

            base.Shape(rectCount);
            //shapeRect = new Rectangle((int)x, (int)y, width, height);
            //this.locationRect = shapeRect;
        }
    }
}
