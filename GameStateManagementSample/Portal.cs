using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    class Portal
    {
        public Texture2D Texture;
        public Rectangle Rect;
        public short health = 10;
        public short defaultHealth = 10;
        Random random = new Random();

        Rectangle sourceRect;
        float timer = 0f;
        float interval = 1000f / 10f;
        int frameRows = 1;
        int frameColumns = 4;
        int currentColumn = 0;
        int currentRow = 0;
        int spriteWidth = 256;
        int spriteHeight = 256;

        public void LoadContent(ContentManager Content, String texture, Vector2 windowSize)
        {
            Texture = Content.Load<Texture2D>(texture);
            Rect = new Rectangle((int)windowSize.X / 2 - 128, 0, 256, 256);
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

        public bool Collides(Player player, bool calcPerPixel)
        {
            // Get dimensions of texture
            int widthOther = player.Texture.Width;
            int heightOther = player.Texture.Height;
            int widthMe = Texture.Width;
            int heightMe = Texture.Height;

            if (calcPerPixel &&                                // if we need per pixel
                ((Math.Min(widthOther, heightOther) > 100) ||  // at least avoid doing it
                (Math.Min(widthMe, heightMe) > 100)))          // for small sizes (nobody will notice :P)
            {
                return Rect.Intersects(player.Rect) // If simple intersection fails, don't even bother with per-pixel
                    && PerPixelCollision(this, player);
            }

            return Rect.Intersects(player.Rect);
        }

        static bool PerPixelCollision(Portal a, Player b)
        {
            // Get Color data of each Texture
            Color[] bitsA = new Color[a.Texture.Width * a.Texture.Height];
            a.Texture.GetData(bitsA);
            Color[] bitsB = new Color[b.Texture.Width * b.Texture.Height];
            b.Texture.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = Math.Max(a.Rect.X, b.Rect.X);
            int x2 = Math.Min(a.Rect.X + a.Rect.Width, b.Rect.X + b.Rect.Width);

            int y1 = Math.Max(a.Rect.Y, b.Rect.Y);
            int y2 = Math.Min(a.Rect.Y + a.Rect.Height, b.Rect.Y + b.Rect.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color A = bitsA[(x - a.Rect.X) + (y - a.Rect.Y) * a.Texture.Width];
                    Color B = bitsB[(x - b.Rect.X) + (y - b.Rect.Y) * b.Texture.Width];

                    if (A.A != 0 && B.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }
            // If no collision occurred by now, we're clear.
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, sourceRect, Color.White);
        }
    }
}
