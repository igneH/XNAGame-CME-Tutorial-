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
    public class TitleScreen : GameScreen
    {
        //KeyboardState keyState;
        SpriteFont spriteFont;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (spriteFont == null)
                spriteFont = content.Load<SpriteFont>("SpriteFont1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            if (inputManager.KeyPressed(Keys.Enter))
                ScreenManager.Instance.AddScreen(new SplashScreen(), inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "TitleScreen", new Vector2(100, 100), Color.Black);
        }
    }
}
