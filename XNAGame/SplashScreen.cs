using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAGame
{
    //A screen where f.e. the prodcution studios name shows up when starting the game
    public class SplashScreen : GameScreen
    {
        KeyboardState keyState;
        SpriteFont spriteFont;

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            if (spriteFont == null)
                spriteFont = content.Load<SpriteFont>("SpriteFont1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Z))
                ScreenManager.Instance.AddScreen(new TitleScreen());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "SplashScreen", new Vector2(100, 100), Color.Black);
        }
    }
}
