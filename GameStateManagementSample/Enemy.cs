using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameStateManagement
{
    class Enemy
    {
        public Texture2D Texture;
        public Rectangle Rect;
        public Vector2 Speed;
        public short Health = 1;
        List<int> speedModifier = new List<int>();
        Random random = new Random();
        public bool follow = false;

        Rectangle sourceRect;
        float timer = 0f;
        float interval = 1000f / 10f;
        int frameRows = 1;
        int frameColumns = 14;
        int currentColumn = 0;
        int currentRow = 0;
        int spriteWidth = 32;
        int spriteHeight = 32;

        public void LoadContent(ContentManager Content, String texture, Vector2 position)
        {
            Texture = Content.Load<Texture2D>(texture);
            Rect = new Rectangle((int)position.X, (int)position.Y, 64, 64);
            speedModifier.AddRange(new int[] {-1, 1});
            Speed = new Vector2(random.Next(101) * 0.01f * 5 * speedModifier[random.Next(0, 2)], random.Next(101) * 0.01f * 5 * speedModifier[random.Next(0, 2)]);
            if (Speed.X < 1f)
                Speed.X = 1f * speedModifier[random.Next(0, 2)];
            if (Speed.Y < 1f)
                Speed.Y = 1f * speedModifier[random.Next(0, 2)];
            if (Speed.X < 0 && Speed.X > Speed.Y)
            {
                currentColumn = 6;
                frameColumns = 11;
            }
            else if (Speed.X > 0 && Speed.X > Speed.Y)
            {
                currentColumn = 3;
                frameColumns = 6;
            }
            else if (Speed.Y < 0)
            {
                frameColumns = 3;
            }
            else if (Speed.Y > 0)
            {
                currentColumn = 11;
                frameColumns = 14;
            }
        }

        public void Update(GameTime gameTime)
        {
            //Display spritesheet
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer += deltaTime;
            if (timer > interval)
            {
                currentColumn++;
                if (currentColumn > frameColumns - 1)
                {
                    currentRow++;
                    currentColumn = 0;
                }

                if (currentRow > frameRows - 1)
                {
                    if (Speed.X < 0 && Speed.X > Speed.Y)
                    {
                        currentColumn = 6;
                        frameColumns = 11;
                    }
                    else if (Speed.X > 0 && Speed.X > Speed.Y)
                    {
                        currentColumn = 3;
                        frameColumns = 6;
                    }
                    else if (Speed.Y < 0)
                    {
                        frameColumns = 3;
                    }
                    else if (Speed.Y > 0)
                    {
                        currentColumn = 11;
                        frameColumns = 14;
                    }
                    currentRow = 0;
                }

                timer = 0f;
            }

            sourceRect = new Rectangle(currentColumn * spriteWidth, currentRow * spriteHeight, spriteWidth, spriteHeight);

            Rect.X += (int)Speed.X;
            Rect.Y += (int)Speed.Y;
        }

        /*public bool InLOS(float AngleDistance, float PositionDistance, Vector2 PositionA, Vector2 PositionB, float AngleB)
        {
            float AngleBetween = (float)Math.Atan2((PositionA.Y - PositionB.Y), (PositionA.X - PositionB.X));

            if ((AngleBetween <= (AngleB + (AngleDistance / 2f / 100f))) && (AngleBetween >= (AngleB - (AngleDistance / 2f / 100f))) && (Vector2.Distance(PositionA, PositionB) <= PositionDistance))
                return true;
            else
                return false;
        }*/

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, sourceRect, Color.White);
        }
    }
}
