using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGame
{
    public class FadeAnimation : Animation
    {
        bool increase;
        float fadeSpeed;
        TimeSpan defaultTime, timer;
        //bool startTimer;
        float activateValue;
        bool stopUpdating;
        float defaultAlpha;

        public TimeSpan Timer
        {
            get { return timer; }
            set 
            { 
                defaultTime = value; 
                timer = defaultTime; 
            }
        }
        public float FadeSpeed
        {
            get { return fadeSpeed; }
            set { fadeSpeed = value; }
        }

        public override float Alpha 
        {
             get
            { return alpha; }
            set
            {
                alpha = value;
                if (alpha == 1.0f)
                    increase = false;
                else if (alpha == 0.0f)
                    increase = true;
            }
        }

        public float ActivateValue
        {
            get { return activateValue; }
            set { activateValue = value; }
        }

        public bool Increase
        {
            set
            {
                increase = value;
            }
        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            increase = false;
            fadeSpeed = 1.0f;
            defaultTime = new TimeSpan(0, 0, 1);
            timer = defaultTime;
            activateValue = 0.0f;
            stopUpdating = false;
            defaultAlpha = alpha;
        }

        public override void Update(GameTime gametime)
        {
            if (isActive)
            {
                if (!stopUpdating) 
                { 
                    if (!increase)
                        alpha -= fadeSpeed * (float)gametime.ElapsedGameTime.TotalSeconds;
                    else
                        alpha += fadeSpeed * (float)gametime.ElapsedGameTime.TotalSeconds;
                    if (alpha <= 0.0f)
                    {
                        alpha = 0.0f;
                        increase = true;
                    }
                        
                    else if (alpha >= 1.0f)
                    {
                        alpha = 1.0f;
                        increase = false;
                    }
                        
                }
                
                if(alpha == activateValue)
                {
                    stopUpdating = true;
                    timer -= gametime.ElapsedGameTime;
                    if(timer.TotalSeconds <= 0)
                    {
                        //not needed anymore [SplashScreen 2]
                        //increase = !increase;
                        timer = defaultTime;
                        stopUpdating = false;
                    }
                }
            }
            else
            {
                alpha = defaultAlpha;
                stopUpdating = false;
            }
        }
    }
}
