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
    public class BlankRect : RectParent
    {
        public BlankRect(Game game, int rectCount)
            : base(game)
        {
            SpriteTexture = content.Load<Texture2D>("BlankTexture");
            rectName = name.PIT;
            Width = 100;
            Height = 100;
            Y = 668;
            X = 100;
            base.Shape(rectCount);
        }
    }
}
