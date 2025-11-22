using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace PongGame
{
    public class Game1 : Game
    {
        enum GameState
        {
            MainMenu,
            Playing
        }
        GameState currentGameState = GameState.MainMenu;

        MouseState previousMouseState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D player1Texture;
        Vector2 player1Position;
        float player1width = 20f;
        float player1height = 100f;

        Texture2D player2Texture;
        Vector2 player2Position;
        float player2width = 20f;
        float player2height = 100f;

        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSize = 20f;
        Vector2 ballVelocity = new Vector2(300f, 300f); 

        SpriteFont font;
        Rectangle startButtonRect = Rectangle.Empty;
        Color startButtonColor = Color.White;
        string title = "PONG GAME";

        int scorePlayer1 = 0;
        int scorePlayer2 = 0;

        float baseBallSpeed = 300f;
        float speedIncreaseTimer = 0f;
        float speedIncreaseInterval = 5f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            int screenWidth = 1200;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            previousMouseState = Mouse.GetState(); 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            player1Texture = new Texture2D(GraphicsDevice, 1, 1);
            player1Texture.SetData(new[] { Color.White });
            player1Position = new Vector2(100, 600);

            player2Texture = new Texture2D(GraphicsDevice, 1, 1);
            player2Texture.SetData(new[] { Color.White });
            player2Position = new Vector2(1100, 600);

            ballTexture = new Texture2D(GraphicsDevice, 1, 1);
            ballTexture.SetData(new[] { Color.White });
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - ballSize / 2,
                                       _graphics.PreferredBackBufferHeight / 2 - ballSize / 2);

           
            font = Content.Load<SpriteFont>("MenuFont");
        }

        public void ResetBall()
        {
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - ballSize / 2,
                                       _graphics.PreferredBackBufferHeight / 2 - ballSize / 2);
            Random rand = new Random();
            float dirX = rand.Next(0, 2) == 0 ? -1f : 1f;
            float dirY = rand.Next(0, 2) == 0 ? -1f : 1f;
            ballVelocity = new Vector2(baseBallSpeed * dirX, baseBallSpeed * dirY);
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            MouseState mouseState = Mouse.GetState();
            bool clicked = mouseState.LeftButton == ButtonState.Pressed &&
                           previousMouseState.LeftButton == ButtonState.Released;

            
            int screenW = _graphics.PreferredBackBufferWidth;
            int screenH = _graphics.PreferredBackBufferHeight;
            int btnW = (int)(screenW * 0.35f);  
            int btnH = (int)(screenH * 0.09f);   
            startButtonRect = new Rectangle((screenW - btnW) / 2, (int)(screenH * 0.55f), btnW, btnH);

            if (currentGameState == GameState.MainMenu)
            {
               
                if (startButtonRect.Contains(mouseState.Position))
                    startButtonColor = Color.LightGray;
                else
                    startButtonColor = Color.White;

                
                if (clicked && startButtonRect.Contains(mouseState.Position))
                {
                    currentGameState = GameState.Playing;
                }

                previousMouseState = mouseState;
                return; 
            }


            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
                player1Position.Y -= 500f * deltaTime;
            if (keyboardState.IsKeyDown(Keys.S))
                player1Position.Y += 500f * deltaTime;

            if (keyboardState.IsKeyDown(Keys.Up))
                player2Position.Y -= 500f * deltaTime;
            if (keyboardState.IsKeyDown(Keys.Down))
                player2Position.Y += 500f * deltaTime;

            player1Position.Y = Math.Clamp(player1Position.Y, 0, _graphics.PreferredBackBufferHeight - player1height);
            player2Position.Y = Math.Clamp(player2Position.Y, 0, _graphics.PreferredBackBufferHeight - player2height);

            ballPosition += ballVelocity * deltaTime;

            if (ballPosition.Y <= 0)
            {
                ballPosition.Y = 0;
                ballVelocity.Y *= -1;
            }
            else if (ballPosition.Y + ballSize >= _graphics.PreferredBackBufferHeight)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballSize;
                ballVelocity.Y *= -1;
            }

            speedIncreaseTimer += deltaTime;
            if(speedIncreaseTimer >= speedIncreaseInterval)
            {
                speedIncreaseTimer = 0f;
                ballVelocity *= 1.05f;
            }

            Rectangle ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)ballSize, (int)ballSize);
            Rectangle player1Rect = new Rectangle((int)player1Position.X, (int)player1Position.Y, (int)player1width, (int)player1height);
            Rectangle player2Rect = new Rectangle((int)player2Position.X, (int)player2Position.Y, (int)player2width, (int)player2height);

            if (ballRect.Intersects(player1Rect))
            {
                ballPosition.X = player1Position.X + player1width;
                ballVelocity.X *= -1;
                float totalSpeed = ballVelocity.Length();

                float paddleCenter = player1Position.Y + player1height / 2;
                float ballCenter = ballPosition.Y + ballSize / 2;
                float normalizedDiff = (ballCenter - paddleCenter) / (player1height / 2);
                Vector2 direction = new Vector2(Math.Sign(ballVelocity.X), normalizedDiff);
                direction.Normalize();
                ballVelocity=direction*totalSpeed;
            }
            else if (ballRect.Intersects(player2Rect))
            {   
                float totalSpeed = ballVelocity.Length();
                ballPosition.X = player2Position.X - ballSize;
                ballVelocity.X *= -1;
                float paddleCenter = player2Position.Y + player2height / 2;
                float ballCenter = ballPosition.Y + ballSize / 2;
                float normalizedDiff = (ballCenter - paddleCenter) / (player2height / 2);
                Vector2 direction = new Vector2(Math.Sign(ballVelocity.X), normalizedDiff);
                direction.Normalize();
                ballVelocity=direction*totalSpeed;
            }

            if(ballPosition.X < -ballSize)
            {
                scorePlayer2++;
                ResetBall();
            }
            else if(ballPosition.X > _graphics.PreferredBackBufferWidth)
            {
                scorePlayer1++;
                ResetBall();
            }
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Rectangle player1Rect = new Rectangle((int)player1Position.X, (int)player1Position.Y, (int)player1width, (int)player1height);
            Rectangle player2Rect = new Rectangle((int)player2Position.X, (int)player2Position.Y, (int)player2width, (int)player2height);
            Rectangle ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)ballSize, (int)ballSize);

            GraphicsDevice.Clear(new Color(34, 34, 34));

            _spriteBatch.Begin();

            if (currentGameState == GameState.MainMenu)
            {
                
                Vector2 titleSize = font.MeasureString(title);
                Vector2 titlePos = new Vector2((_graphics.PreferredBackBufferWidth - titleSize.X) / 2f,
                                               _graphics.PreferredBackBufferHeight * 0.18f);
                _spriteBatch.DrawString(font, title, titlePos, Color.White);

                
                _spriteBatch.Draw(player1Texture, startButtonRect, startButtonColor);

                
                string startText = "START";
                Vector2 startTextSize = font.MeasureString(startText);
                Vector2 startTextPos = new Vector2(
                    startButtonRect.X + (startButtonRect.Width - startTextSize.X) / 2f,
                    startButtonRect.Y + (startButtonRect.Height - startTextSize.Y) / 2f
                );
                _spriteBatch.DrawString(font, startText, startTextPos, Color.Black);

                _spriteBatch.End();
                return;
            }

            
            _spriteBatch.Draw(player1Texture, player1Rect, Color.White);
            _spriteBatch.Draw(player2Texture, player2Rect, Color.White);
            _spriteBatch.DrawString(font, $"{scorePlayer1}", new Vector2(500, 50), Color.White);
            _spriteBatch.DrawString(font, $"{scorePlayer2}", new Vector2(650, 50), Color.White);

            _spriteBatch.Draw(ballTexture, ballRect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
