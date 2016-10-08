using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameStateManagement
{
    class Core
    {
        public Texture2D Texture;
        public Rectangle Rect;
        public short health = 20;
        public short defaultHealth = 20;
        Random random = new Random();

        Rectangle sourceRect;
        float timer = 0f;
        float interval = 1000f / 10f;
        int frameRows = 1;
        int frameColumns = 3;
        int currentColumn = 0;
        int currentRow = 0;
        int spriteWidth = 128;
        int spriteHeight = 128;

        public void LoadContent(ContentManager Content, String texture, Vector2 windowSize)
        {
            Texture = Content.Load<Texture2D>(texture);
            Rect = new Rectangle(random.Next(0, (int)windowSize.X - 128), random.Next(0, (int)windowSize.Y - 128), 128, 128);
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
                    currentColumn = 0;
                    currentRow = 0;
                }

                timer = 0f;
            }

            sourceRect = new Rectangle(currentColumn * spriteWidth, currentRow * spriteHeight, spriteWidth, spriteHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, sourceRect, Color.White);
        }
    }
}