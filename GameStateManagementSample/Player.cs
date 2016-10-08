using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameStateManagement
{
    class Player
    {
        public Texture2D Texture;
        //Texture2D bgTexture;
        public Rectangle bgRect;
        Texture2D target_img;
        Rectangle sourceRect;
        public Rectangle Rect;
        short Speed = 5;
        public short health = 100;
        
        float timer = 0f;
        float interval = 1000f / 10f;
        //int frameRows = 12;
        //int frameColumns = 4;
        int currentColumn = 0;
        int currentRow = 0;
        int spriteWidth = 32;
        int spriteHeight = 32;
        public bool up, down, left, right, firing, useMouse = false;
        public Vector2 targetPosition;
        public bool portalActive;

        public void LoadContent(ContentManager Content, string texture, Vector2 position, Vector2 size)
        {
            Texture = Content.Load<Texture2D>(texture);
            //bgTexture = Content.Load<Texture2D>("bg2");
            target_img = Content.Load<Texture2D>("targeting");
            Rect = new Rectangle((int)position.X - (int)size.X / 2, (int)position.Y - (int)size.Y, (int)size.X, (int)size.Y);
            bgRect = new Rectangle(Rect.X - 200, Rect.Y - 200, Rect.Width + 200 * 2, Rect.Height + 200 * 2);
        }

        public void Update(GameTime gameTime, ContentManager Content, KeyboardState currentState, KeyboardState previousState, MouseState _currentState, MouseState _previousState, 
            GamePadState currentPadState, GamePadState previousPadState, Vector2 windowSize)
        {
            up = false;
            down = false;
            left = false;
            right = false;

            // Mouse and keyboard
            if (currentState.IsKeyDown(Keys.W) && Rect.Top >= 0)
            {
                Rect.Y -= Speed;
                up = true;
            }
            if (currentState.IsKeyDown(Keys.A) && Rect.Left >= 0)
            {
                Rect.X -= Speed;
                left = true;
            }
            if (currentState.IsKeyDown(Keys.S) && Rect.Bottom <= windowSize.Y)
            {
                Rect.Y += Speed;
                down = true;
            }
            if (currentState.IsKeyDown(Keys.D) && Rect.Right <= windowSize.X)
            {
                Rect.X += Speed;
                right = true;
            }
            if (_currentState.LeftButton == ButtonState.Pressed && _previousState.LeftButton == ButtonState.Released)
            {
                firing = true;
                useMouse = true;
            }

            // Controller
            if (currentPadState.ThumbSticks.Left.Y > 0.0f && Rect.Top >= 0)
            {
                Rect.Y -= Speed;
                up = true;
            }
            if (currentPadState.ThumbSticks.Left.X < 0.0f && Rect.Left >= 0)
            {
                Rect.X -= Speed;
                left = true;
            }
            if (currentPadState.ThumbSticks.Left.Y < 0.0f && Rect.Bottom <= windowSize.Y)
            {
                Rect.Y += Speed;
                down = true;
            }
            if (currentPadState.ThumbSticks.Left.X > 0.0f && Rect.Right <= windowSize.X)
            {
                Rect.X += Speed;
                right = true;
            }
            if (currentPadState.Triggers.Right > 0.5f && previousPadState.Triggers.Right < 0.5f)
            {
                firing = true;
                useMouse = false;
            }

            //Display spritesheet
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer += deltaTime;
            if (timer > interval && (up || down || right || left))
            {
                currentColumn++;

                /*if (currentColumn > frameRows - 1)
                {
                    //currentRow++;
                    currentColumn = 0;
                    up = false;
                    down = false;
                    right = false;
                    left = false;
                }*/

                /*if (currentRow > frameRows - 1)
                {
                    currentColumn = 0;
                    currentRow = 0;
                }*/

                if (up)
                {
                    if (currentColumn == 6)
                        currentColumn = 3;
                    else if (currentColumn < 3 || currentColumn > 6)
                        currentColumn = 3;
                    //currentRow = 0;
                    left = false;
                    right = false;
                }
                else if (down)
                {
                    if (currentColumn == 3)
                        currentColumn = 0;
                    else if (currentColumn > 3)
                        currentColumn = 0;
                    //currentRow = 2;
                    left = false;
                    right = false;
                }
                if (right)
                {
                    if (currentColumn == 12)
                        currentColumn = 9;
                    else if (currentColumn < 9)
                        currentColumn = 9;
                    //currentRow = 1;
                }
                else if (left)
                {
                    if (currentColumn == 9)
                        currentColumn = 6;
                    else if (currentColumn < 6 || currentColumn > 9)
                        currentColumn = 6;
                    //currentRow = 3;
                }

                timer = 0f;
            }

            sourceRect = new Rectangle(currentColumn * spriteWidth, currentRow * spriteHeight, spriteWidth, spriteHeight);

            bgRect.X = Rect.X - 86;
            bgRect.Y = Rect.Y - 86;

            // Set where our target should be drawn and where bullets fire
            int radius = 50;
            if (!useMouse)
            {
                double angle = Math.Atan2(currentPadState.ThumbSticks.Right.X, currentPadState.ThumbSticks.Right.Y) + 3 * Math.PI / 2;
                double x_coord = radius * Math.Cos(angle);
                double y_coord = radius * Math.Sin(angle);
                targetPosition = new Vector2(Rect.Center.X + (float)x_coord, Rect.Center.Y + (float)y_coord);
            }
            else
            {
                double angle = Math.Atan2(_currentState.X - Rect.Center.X, _currentState.Y - Rect.Center.Y) +  Math.PI / 2;
                double x_coord = -(radius * Math.Cos(angle));
                double y_coord = radius * Math.Sin(angle);
                targetPosition = new Vector2(Rect.Center.X + (float)x_coord, Rect.Center.Y + (float)y_coord);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!portalActive)
            {
                //spriteBatch.Draw(bgTexture, new Rectangle(bgRect.X - 32, bgRect.Y - 32, bgRect.Width + 64, bgRect.Height + 64), Color.White);
                spriteBatch.Draw(target_img, targetPosition, Color.White);
            }
            
            spriteBatch.Draw(Texture, Rect, sourceRect, Color.White);
        }
    }
}
