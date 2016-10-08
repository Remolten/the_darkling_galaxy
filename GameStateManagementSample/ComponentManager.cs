using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace GameStateManager
{
    class ComponentManager
    {
        Random randomGenerator = new Random();
        Enemy enemy;
        public List<Enemy> enemyList = new List<Enemy>();
        short enemyCounter;

        public Core core = new Core();
        Portal portal = new Portal();

        public List<Bullet> bulletList = new List<Bullet>();
        Bullet bullet = new Bullet();

        public List<Powerup> powerupList = new List<Powerup>();
        Powerup powerup;

        public bool portalActive = false;
        public int level = 1;

        public float shotsTaken = 1;
        public float kills = 1;

        public bool gameOver = false;

        public void LoadContent(Vector2 windowSize, ContentManager Content, Player player)
        {
            core.LoadContent(Content, "core (2)", windowSize);
            portal.LoadContent(Content, "Bigger Portal", windowSize);
            player.Rect.X = (int)windowSize.X / 2;
            player.Rect.Y = (int)windowSize.Y - 40;
            SoundManager.Audio().LoadContent(Content);
            SoundManager.Audio().PlaySong(randomGenerator.Next(0, SoundManager.Audio().songs.Count - 1));
        }

        public void Update(Vector2 windowSize, ContentManager Content, GameTime gameTime, Player player)
        {
            this.Generate(windowSize, Content, player);
            this.UpdateChildren(windowSize, Content, gameTime, player);
        }

        private void Generate(Vector2 windowSize, ContentManager Content, Player player)
        {
            if (enemyCounter >= 40)
            {
                if (randomGenerator.Next(21) == 11)
                {
                    enemy = new Enemy();
                    enemy.LoadContent(Content, "blob", new Vector2(core.Rect.Center.X, core.Rect.Center.Y));
                    enemyList.Add(enemy);
                    enemyCounter = 0;
                    int random = randomGenerator.Next(0, 3);
                    if (random == 1)
                    {
                        enemy.follow = true;
                        enemy.Texture = Content.Load<Texture2D>("Blue Blop");
                    }
                    else if (random == 2)
                    {
                        enemy.Health++;
                        enemy.Texture = Content.Load<Texture2D>("Tanky Blob");
                    }
                }
            }

            if (player.firing && !portalActive)
            {
                SoundManager.Audio().PlayEffect("laser_shooting_sfx");
                bullet = new Bullet();
                bullet.LoadContent(Content, "test_bullet", player);
                bulletList.Add(bullet);
                player.firing = false;
                shotsTaken++;
            }

            enemyCounter++;

        }

        private void UpdateChildren(Vector2 windowSize, ContentManager Content, GameTime gameTime, Player player)
        {
            if (!portalActive)
            {
                foreach (Powerup _powerup in powerupList.Reverse<Powerup>())
                {
                    if (_powerup.Rect.Intersects(player.Rect))
                    {
                        player.health += 20;
                        powerupList.Remove(_powerup);
                    }
                    else
                        powerup.Update(gameTime);
                }

                foreach (Enemy enemy in enemyList.Reverse<Enemy>())
                {
                    if (enemy.Rect.X < 32)
                    {
                        enemy.Speed.X = -enemy.Speed.X;
                    }
                    if (enemy.Rect.X > windowSize.X - 32)
                    {
                        enemy.Speed.X = -enemy.Speed.X;
                    }
                    if (enemy.Rect.Y < 32)
                    {
                        enemy.Speed.Y = -enemy.Speed.Y;
                    }
                    if (enemy.Rect.Y > windowSize.Y - 32)
                    {
                        enemy.Speed.Y = -enemy.Speed.Y;
                    }
                    
                    int distance = 120;
                    if (new Rectangle(enemy.Rect.X - distance, enemy.Rect.Y - distance, enemy.Rect.Width + 2 * distance, enemy.Rect.Height + 2 * distance).Intersects(player.Rect))
                        enemy.follow = true;

                    if (enemy.follow)
                    {
                        enemy.Speed = new Vector2((player.Rect.Center.X - enemy.Rect.Center.X) / 20 * randomGenerator.Next(1, 3), (player.Rect.Center.Y - enemy.Rect.Center.Y) / 20 * randomGenerator.Next(1, 3));

                        int desiredSpeed = 5;
                        int biggerSpeed;
                        float speedMod = 0f;

                        biggerSpeed = Math.Max(Math.Abs((int)enemy.Speed.X), Math.Abs((int)enemy.Speed.Y));

                        if (!(biggerSpeed > desiredSpeed))
                            if (!(biggerSpeed == 0))
                                Math.Abs(speedMod = desiredSpeed / biggerSpeed);
                            else
                                speedMod = 1;

                            if (biggerSpeed == Math.Abs(enemy.Speed.X))
                            {
                                if (enemy.Speed.X > 0)
                                {
                                    enemy.Speed.X = desiredSpeed;
                                    enemy.Speed.Y *= speedMod;
                                }
                                else if (enemy.Speed.X < 0)
                                {
                                    enemy.Speed.X = -desiredSpeed;
                                    enemy.Speed.Y *= speedMod;
                                }
                            }
                            else if (biggerSpeed == Math.Abs(enemy.Speed.Y))
                            {
                                if (enemy.Speed.Y > 0)
                                {
                                    enemy.Speed.Y = desiredSpeed;
                                    enemy.Speed.X *= speedMod;
                                }
                                else if (enemy.Speed.Y < 0)
                                {
                                    enemy.Speed.Y = -desiredSpeed;
                                    enemy.Speed.X *= speedMod;
                                }
                            }
                    }

                    else
                    {
                        if (randomGenerator.Next(0, 51) == 1)
                        {
                            List<int> speedModifier = new List<int>();
                            speedModifier.AddRange(new int[] { -1, 1 });
                            enemy.Speed = new Vector2(randomGenerator.Next(101) * 0.01f * 5 * speedModifier[randomGenerator.Next(0, 2)], randomGenerator.Next(101) * 0.01f * 5 * speedModifier[randomGenerator.Next(0, 2)]);
                        }
                    }

                    if (enemy.Rect.Intersects(player.Rect))
                    {
                        player.health -= 1;
                        if (player.health <= 0)
                        {
                            player.health = 0;
                            gameOver = true;
                        }
                    }

                    foreach (Bullet _bullet in bulletList.Reverse<Bullet>())
                    {
                        if (_bullet.Rect.Intersects(enemy.Rect)) //&& new Rectangle(player.bgRect.X - 32, player.bgRect.Y - 32, player.bgRect.Width + 64, player.bgRect.Height + 64).Intersects(enemy.Rect))
                        {
                            bulletList.Remove(_bullet);
                            enemy.Health -= 1;
                            if (randomGenerator.Next(101) == 1)
                            {
                                powerup = new Powerup();
                                powerup.LoadContent(Content);
                                powerup.Rect.X = enemy.Rect.Center.X;
                                powerup.Rect.Y = enemy.Rect.Center.Y;
                                powerupList.Add(powerup);
                            }
                        }

                        if (enemy.Health <= 0)
                        {
                            kills++;
                            enemyList.Remove(enemy);
                        }

                        if (_bullet.Rect.Intersects(core.Rect) && player.bgRect.Intersects(core.Rect))
                        {
                            core.health -= 1;
                            bulletList.Remove(_bullet);
                        }

                        if (core.health <= 0)
                        {
                            SoundManager.Audio().StopSong();
                            SoundManager.Audio().PlaySong(SoundManager.Audio().songs.Count - 1);
                            portalActive = true;
                            player.portalActive = true;
                            player.Rect.X = (int)windowSize.X / 2;
                            player.Rect.Y = (int)windowSize.Y - 40;
                        }

                        if (_bullet.Rect.X < -32)
                            bulletList.Remove(_bullet);
                        if (_bullet.Rect.X > windowSize.X)
                            bulletList.Remove(_bullet);
                        if (_bullet.Rect.Y < -32)
                            bulletList.Remove(_bullet);
                        if (_bullet.Rect.Y > windowSize.Y)
                            bulletList.Remove(_bullet);
                    }
                }

                foreach (Enemy _enemy in enemyList.Reverse<Enemy>())
                {
                    _enemy.Update(gameTime);
                }

                foreach (Bullet _bullet in bulletList.Reverse<Bullet>())
                {
                    _bullet.Update();
                }

                core.Update(gameTime);
            }

            else
            {
                portal.Update(gameTime);

                if (portal.Collides(player, true))
                {
                    level += 1;
                    portalActive = false;
                    player.portalActive = false;
                    bulletList.Clear();
                    enemyList.Clear();
                    core.LoadContent(Content, "core (2)", windowSize);
                    core.health = core.defaultHealth;
                    SoundManager.Audio().StopSong();
                    SoundManager.Audio().PlaySong(randomGenerator.Next(0, SoundManager.Audio().songs.Count - 1));
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            if (!portalActive)
                this.DrawChildren(spriteBatch, player);
            else
                portal.Draw(spriteBatch);
        }

        private void DrawChildren(SpriteBatch spriteBatch, Player player)
        {
            foreach (Powerup powerup in powerupList)
                powerup.Draw(spriteBatch);

            foreach (Enemy _enemy in enemyList.Reverse<Enemy>())
            {
                //if (_enemy.Rect.Intersects(player.bgRect))
                _enemy.Draw(spriteBatch);
            }

            foreach (Bullet _bullet in bulletList.Reverse<Bullet>())
            {
                //if (_bullet.Rect.Intersects(player.bgRect))
                _bullet.Draw(spriteBatch);
            }

            // Fixes an unknown error
            if (core.Texture != null) //&& core.Rect.Intersects(player.bgRect))
                core.Draw(spriteBatch);
        }
    }
}
