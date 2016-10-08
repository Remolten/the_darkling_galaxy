using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameStateManagement
{
    class Bullet
    {
        public Texture2D Texture;
        public Rectangle Rect;
        public Vector2 Speed;
        int spriteWidth = 2;
        int spriteHeight = 2;
        //string dir;

        public void LoadContent(ContentManager Content, String texture, Player player)
        {
            Texture = Content.Load<Texture2D>(texture);
            Rect = new Rectangle(player.Rect.Center.X, player.Rect.Center.Y, spriteWidth, spriteHeight);
            Speed.X = (player.targetPosition.X - player.Rect.Center.X) / 5;
            Speed.Y = (player.targetPosition.Y - player.Rect.Center.Y) / 5;
            /*if (player.up)
            {
                Rect = new Rectangle(player.Rect.X + player.Rect.Width / 2 - spriteWidth / 2, player.Rect.Top, spriteWidth, spriteHeight);
                dir = "up";
            }
            else if (player.down)
            {
                Rect = new Rectangle(player.Rect.X + player.Rect.Width / 2 - spriteWidth / 2, player.Rect.Bottom + 2, spriteWidth, spriteHeight);
                dir = "down";
            }
            else if (player.left)
            {
                Rect = new Rectangle(player.Rect.Left, player.Rect.Y + player.Rect.Height / 2 - spriteHeight / 2, spriteWidth, spriteHeight);
                dir = "left";
            }
            else if (player.right)
            {
                Rect = new Rectangle(player.Rect.Right + 2, player.Rect.Y + player.Rect.Height / 2 - spriteHeight / 2, spriteWidth, spriteHeight);
                dir = "right";
            }*/
        }

        public void Update()
        {
            /*switch (dir)
            {
                case "up":
                    Rect.Y -= Speed;
                    break;
                case "down":
                    Rect.Y += Speed;
                    break;
                case "left":
                    Rect.X -= Speed;
                    break;
                case "right":
                    Rect.X += Speed;
                    break;
            }*/
            Rect.X += (int)Speed.X;
            Rect.Y += (int)Speed.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color.White);
        }
    }
}
