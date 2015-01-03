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
using Utilities;

namespace FinalProject.Components
{
    public class BackgroundSprite : SpriteCharacter
    {

        private float time;

        public BackgroundSprite(Game game)
            : base(game)
        {
            this.SpriteTexture = content.Load<Texture2D>("PalmAndSand");

            this.Direction = new Vector2(0, 0);

            this.Location = new Vector2(0, 0);
            this.accel = 20;
            speedMax = 100f;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
