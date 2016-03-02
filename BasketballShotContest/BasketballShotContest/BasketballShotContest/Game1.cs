using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame3
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region data
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D ballTex;
        Texture2D basketTex;
        Texture2D arrowTex;
        Texture2D strength;
        Texture2D bg;
        Vector2 pos1 = new Vector2(1500, 700);
        Vector2 pos2 = new Vector2(100, 700);
        SpriteFont font;
        int screenWidth;
        int screenHeight;
        float rotation2 = -0.785f;
        float rotation1 = -2.356f;
        int strength1 = 0;
        int strength2 = 0;
        float vx1 = 0;
        float vx2 = 0;
        float vy1 = 0;
        float vy2 = 0;
        int score1 = 0;
        int score2 = 0;
        bool isfly1 = false;
        bool isfly2 = false;
        bool ismade1 = false;
        bool ismade2 = false;
        static public KeyboardState keyboardState;
        static public KeyboardState previousState;
        Random rnd = new Random();
        #endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1700;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            font = Content.Load<SpriteFont>("font");

            ballTex = Content.Load<Texture2D>("Images/ball");
            basketTex = Content.Load<Texture2D>("Images/basket");
            arrowTex = Content.Load<Texture2D>("Images/arrow");
            strength = Content.Load<Texture2D>("Images/strength");
            bg = Content.Load<Texture2D>("Images/background");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardThings();
            if (isfly1)
            {
                if (pos1.Y <= 900 && pos1.X >= 600)
                {
                    vy1 += 0.2f;
                    pos1.X += vx1;
                    pos1.Y += vy1;
                }
                else
                {
                    isfly1 = false;
                    pos1 = new Vector2(rnd.Next(1000,1600), rnd.Next(500, 700));
                    rotation1 = -2.356f;
                    strength1 = 0;
                    ismade1 = false;
                }
            }
            if (isfly2)
            {
                if (pos2.Y <= 900 && pos2.X <= 1000)
                {
                    vy2 += 0.2f;
                    pos2.X += vx2;
                    pos2.Y += vy2;
                }
                else
                {
                    isfly2 = false;
                    pos2 = new Vector2(rnd.Next(0, 600), rnd.Next(500, 700));
                    rotation2 = -0.785f;
                    strength2 = 0;
                    ismade2 = false;
                }
            }
            made1();
            made2();
            Window.Title = "y1:" + ((int)pos1.Y).ToString() + "   y2:" + ((int)pos2.Y).ToString();

            base.Update(gameTime);
        }
        public void KeyboardThings()
        {
            previousState = keyboardState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D) && rotation2 < -0.2f)
                rotation2 += 0.02f;
            if (keyboardState.IsKeyDown(Keys.A) && rotation2 > -1.5f)
                rotation2 -= 0.02f;
            if (keyboardState.IsKeyDown(Keys.Right) && rotation1 < -1.6f)
                rotation1 += 0.02f;
            if (keyboardState.IsKeyDown(Keys.Left) && rotation1 > -3f)
                rotation1 -= 0.02f;
            if (keyboardState.IsKeyDown(Keys.Up) && strength1 < 299)
                strength1 += 1;
            if (keyboardState.IsKeyDown(Keys.Down) && strength1 > 0)
                strength1 -= 1;
            if (keyboardState.IsKeyDown(Keys.W) && strength2 < 299)
                strength2 += 1;
            if (keyboardState.IsKeyDown(Keys.S) && strength2 > 0)
                strength2 -= 1;
            if (keyboardState.IsKeyDown(Keys.Enter) && previousState.IsKeyUp(Keys.Enter) && !isfly1)
                fly1();
            if (keyboardState.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space) && !isfly2)
                fly2();
        }
        public void fly1()
        {
            isfly1 = true;
            vx1 = strength1 * (float)Math.Cos(rotation1) / 8;
            vy1 = strength1 * (float)Math.Sin(rotation1) / 8;
        }
        public void fly2()
        {
            isfly2 = true;
            vx2 = strength2 * (float)Math.Cos(rotation2) / 8;
            vy2 = strength2 * (float)Math.Sin(rotation2) / 8;
        }
        public void made1()
        {
            float distanceX = Math.Abs(pos1.X - 850);
            float distance = (float)Math.Sqrt((Math.Pow((double)distanceX, 2) + Math.Pow((double)pos1.Y - 100, 2)));
            float angle = (float)Math.Atan((double)vx1 / vy1);
            if (distance < 70 && !ismade1 && angle < -0.5235f)
            {
                ismade1 = true;
                vx1 = 0;
                vy1 = 0;
                score1++;
            }
        }
        public void made2()
        {
            float distanceX = Math.Abs(pos2.X - 850);
            float distance = (float)Math.Sqrt((Math.Pow((double)distanceX, 2) + Math.Pow((double)pos2.Y - 100, 2)));
            float angle = (float)Math.Atan((double)vx2 / vy2);
            if (distance < 70 && !ismade2 && angle > -2.617f)
            {
                ismade2 = true;
                vx2 = 0;
                vy2 = 0;
                score2++;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            Rectangle rec1 = new Rectangle((int)pos2.X+50, (int)pos2.Y+50, 250, 50);
            Rectangle rec2 = new Rectangle((int)pos1.X + 50, (int)pos1.Y + 50, 250, 50);
            Rectangle rec3 = new Rectangle((int)pos2.X + 50, (int)pos2.Y + 50, 250 * strength2 / 299, 50);
            Rectangle rec4 = new Rectangle((int)pos1.X + 50, (int)pos1.Y + 50, 250 * strength1 / 299, 50);
            spriteBatch.Draw(bg, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.Draw(basketTex, new Rectangle(screenWidth / 2 - 125, 100, 250, 250), Color.White);
            spriteBatch.DrawString(font, "Score1:" + score1, new Vector2(900, 50), Color.Red, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Score2:" + score2, new Vector2(700, 50), Color.Red, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ballTex, new Rectangle((int)pos2.X, (int)pos2.Y, 100, 100), Color.White);
            spriteBatch.Draw(ballTex, new Rectangle((int)pos1.X, (int)pos1.Y, 100, 100), Color.White);
            if (!isfly2)
            {
                spriteBatch.Draw(arrowTex, rec1, new Rectangle(0, 0, 299, 228), Color.White, rotation2, new Vector2(-50, 50), SpriteEffects.None, 0f);
                spriteBatch.Draw(strength, rec3, new Rectangle(0, 0, strength2, 228), Color.White, rotation2, new Vector2(-50, 50), SpriteEffects.None, 0f);
            }
            if (!isfly1)
            {
                spriteBatch.Draw(arrowTex, rec2, new Rectangle(0, 0, 299, 228), Color.White, rotation1, new Vector2(-50, 50), SpriteEffects.None, 0f);
                spriteBatch.Draw(strength, rec4, new Rectangle(0, 0, strength1, 228), Color.White, rotation1, new Vector2(-50, 50), SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
