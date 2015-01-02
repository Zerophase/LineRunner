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

namespace Utilities
{
    public class DrawableAnimatedSprite : SpriteCharacter
    {
        protected SpriteAnimationAdapter spriteAnimationAdapter;

        protected Texture2D currentTexture;

        public DrawableAnimatedSprite(Game game)
            : base(game)
        {
            spriteAnimationAdapter = new SpriteAnimationAdapter(game);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = spriteAnimationAdapter.CurrentTexture;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            lastUpdateTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            SpriteEffects = SpriteEffects.None;
            this.spriteTexture = this.spriteAnimationAdapter.CurrentTexture;

            Rectangle currentTextureRect = spriteAnimationAdapter.GetCurrentDrawRect(lastUpdateTime);

            this.locationRect = new Rectangle((int)Location.X - (int)this.Origin.X,
                (int)Location.Y - (int)this.Origin.Y,
                currentTextureRect.Width,
                currentTextureRect.Height);

            base.Update(gameTime);
        }

        protected override void SetTransformAndRect()
        {
            try
            {
                spriteTransform =
                    Matrix.CreateTranslation(new Vector3(this.Origin, 0.0f)) *
                    Matrix.CreateScale(this.Scale) *
                    Matrix.CreateRotationZ(0.0f) *
                    Matrix.CreateTranslation(new Vector3(this.Location, 0.0f));

                this.locationRect = CalculateBoundingRectangle(
                         new Rectangle(0, 0, this.spriteAnimationAdapter.CurrentTexture.Width,
                             this.spriteAnimationAdapter.CurrentTexture.Height),
                         spriteTransform);
            }
            catch (NullReferenceException nu)
            {
                //nothing
                if (this.spriteTexture == null)
                {
                    //first time this will fail because load content hasn't been called yet

                }
                else
                {
                    throw nu;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle currentTextureRect = spriteAnimationAdapter.GetCurrentDrawRect(lastUpdateTime);

            this.locationRect = new Rectangle((int)Location.X - (int)this.Origin.X,
                (int)Location.Y - (int)this.Origin.Y,
                currentTextureRect.Width,
                currentTextureRect.Height);

            spriteBatch.Begin();
            spriteBatch.Draw(spriteAnimationAdapter.CurrentTexture,
                new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    currentTextureRect.Width * (int)this.Scale,
                    currentTextureRect.Height * (int)this.Scale),
                    currentTextureRect,
                    Color.White,
                    0.0f,//Rotation
                    this.Origin,
                    SpriteEffects,
                    0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    public class SpriteAnimationAdapter
    {
        List<SpriteAnimation> spriteAnimations;
        protected SpriteAnimation currentAnimation;
        protected CellAnimationManager cellAnimationmanager;


        public Rectangle CurrentLocationRect
        {
            get
            {
                return this.GetCurrentDrawRect();  
            }
        }

        public CellAnimationManager CellAnimationManager { get { return cellAnimationmanager; } }
        public SpriteAnimation CurrentAnimation
        {
            get { return currentAnimation; }
            set
            {
                if (!(spriteAnimations.Contains(value)))
                {
                    this.spriteAnimations.Add(value);
                }
                else
                    this.currentAnimation = value;
            }
        }

        public SpriteAnimationAdapter(Game game)
        {
            spriteAnimations = new List<SpriteAnimation>();

            cellAnimationmanager = (CellAnimationManager)game.Services.GetService(typeof(ICellAnimationManager));
            if (cellAnimationmanager == null)
            {
                throw new Exception("To use a DrawableAnimatedSprite you must add CellAnimationManger to the game as a service.");
            }
        }

        public Texture2D CurrentTexture
        {
            get { return cellAnimationmanager.GetTexture(currentAnimation.TextureName); }
        }

        public void AddAnimation(SpriteAnimation sprite)
        {
            this.spriteAnimations.Add(sprite);
            this.cellAnimationmanager.AddAnimation(sprite.AnimationName, sprite.TextureName, 
                sprite.CellSize, sprite.FPS);
            this.cellAnimationmanager.ToggleAnimation(sprite.AnimationName, false);//false = playing
            if (spriteAnimations.Count == 1)
                currentAnimation = sprite;
        }

        public void ResetAnimation(SpriteAnimation sprite)
        {
            this.cellAnimationmanager.ResetAnimation(sprite.AnimationName);
        }

        public void RemoveAnimation(SpriteAnimation sprite)
        {
            this.spriteAnimations.Remove(sprite);
            this.cellAnimationmanager.Animations.Remove(sprite.AnimationName);
        }

        public void PauseAnimation(SpriteAnimation sprite)
        {
            this.cellAnimationmanager.ToggleAnimation(sprite.AnimationName, true);
        }

        public void GoToFram(SpriteAnimation sprite, int frame)
        { 
            //TODO
        }
        public void ResumeAnimation(SpriteAnimation sprite)
        {
            this.cellAnimationmanager.ToggleAnimation(sprite.AnimationName, false);
        }

        public Rectangle GetCurrentDrawRect()
        {
            return GetCurrentDrawRect(0.0f);
        }

        public Rectangle GetCurrentDrawRect(float elapsedTime)
        {
            return this.CellAnimationManager.GetCurrentDrawRect(elapsedTime, currentAnimation.AnimationName);
        }

        public int GetLoopCount()
        { 
            return this.cellAnimationmanager.Animations[currentAnimation.AnimationName].LoopCount;
        }
    }

    public class SpriteAnimation
    {
        public string AnimationName;
        public int FPS;
        public string TextureName;
        public CellCount CellSize;

        protected bool isPaused;
        public bool IsPaused { get { return isPaused; } set { isPaused = value; } }
        public SpriteAnimation(string animationName, string textureName,
            int fps, int columns, int rows)
        {
            this.AnimationName = animationName;
            this.FPS = fps;
            this.TextureName = textureName;
            this.CellSize = new CellCount(columns, rows);
            isPaused = true;
        }
    }
}
