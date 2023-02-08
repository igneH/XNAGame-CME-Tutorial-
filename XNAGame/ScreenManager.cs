﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAGame
{
    public class ScreenManager
    {
        #region Variable

        /// <summary>
        /// Creating custom Contentmanager
        /// </summary>
        ContentManager content;

        GameScreen currentScreen;

        GameScreen newScreen;

        /// <summary>
        /// ScreenManager Instance
        /// </summary>
        private static ScreenManager instance;

        /// <summary>
        /// Screenstack
        /// </summary>
        Stack<GameScreen> screenStack = new Stack<GameScreen>();

        /// <summary>
        /// Screens width and height
        /// </summary>
        Vector2 dimensions;

        /// <summary>
        /// Let's us know if we should transition or not
        /// </summary>
        
        bool transition;

        FadeAnimation fade;

        Texture2D fadeTexture;

        #endregion

        #region Properties

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }
        
        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }
        #endregion

        #region Main Methods

        //Let me load & unload whichever screens that I need
        public void AddScreen(GameScreen screen)
        {
            transition = true;
            //newScreen not rlly needed, but if a Screen gets deleted from the stack it's needed
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateValue = 1.0f;
        }

        /*
         * Why Initialize, not a constructor?
         * Initzialize can be called any amount of time we want to
         * Constructor gets only called when the Object is created/ not more then once!
         */
        public void Initialize()
        {
            currentScreen = new SplashScreen();
            fade = new FadeAnimation();
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content);

            fadeTexture = content.Load<Texture2D>("blackdot");
            fade.LoadContent(content, fadeTexture, "", Vector2.Zero);
            fade.Scale = dimensions.X;
        }

        public void Update(GameTime gameTime)
        {
            if (!transition)
                currentScreen.Update(gameTime);
            else
                Transition(gameTime);
            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (transition)
                fade.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods
        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime);
            if(fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content);
            }
            else if ( fade.Alpha == 0.0f) 
            {
                transition = false;
                fade.IsActive = false;
            }
        }
        #endregion
    }
}