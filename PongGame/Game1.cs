using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Win32;
using System;

namespace PongGame
{
    public class Game1 : Game
    {
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
        Vector2 ballVelocity = new Vector2(300f, 300f); // X and Y speeds



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
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player1Texture = new Texture2D(GraphicsDevice, 1,1);
            player1Texture.SetData(new[] { Color.White });
            player1Position = new Vector2(100,600);


            player2Texture = new Texture2D(GraphicsDevice, 1, 1);
            player2Texture.SetData(new[] { Color.White });
            player2Position = new Vector2(1100, 600);


            ballTexture = new Texture2D(GraphicsDevice, 1, 1);
            ballTexture.SetData(new[] { Color.White }); 
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - ballSize / 2, _graphics.PreferredBackBufferHeight / 2 - ballSize / 2);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W))
            {
                player1Position.Y -= 500f * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                player1Position.Y += 500f * deltaTime;
            }

            //Player 2 controls
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                player2Position.Y -= 500f * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                player2Position.Y += 500f * deltaTime;
            }
            player1Position.Y = Math.Clamp(player1Position.Y, 0, _graphics.PreferredBackBufferHeight - player1height);
            player2Position.Y = Math.Clamp(player2Position.Y, 0, _graphics.PreferredBackBufferHeight - player2height);

            ballPosition += ballVelocity* deltaTime;

            Rectangle ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)ballSize, (int)ballSize);
            Rectangle player1Rect = new Rectangle((int)player1Position.X, (int)player1Position.Y, (int)player1width, (int)player1height);
            Rectangle player2Rect = new Rectangle((int)player2Position.X, (int)player2Position.Y, (int)player2width, (int)player2height);

            if(ballRect.Intersects(player1Rect) || ballRect.Intersects(player2Rect))
            {
                ballVelocity.X = -ballVelocity.X;
            }

            if (ballPosition.Y <= 0 || ballPosition.Y + ballSize >= _graphics.PreferredBackBufferHeight)
            {
                ballVelocity.Y *= -1; 
            }




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Rectangle player1Rect = new Rectangle((int)player1Position.X, (int)player1Position.Y, (int)player1width, (int)player1height);
            Rectangle player2Rect = new Rectangle((int)player2Position.X, (int)player2Position.Y, (int)player2width, (int)player2height);
            Rectangle ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)ballSize, (int)ballSize);


            GraphicsDevice.Clear(new Color(34,34,34));
            _spriteBatch.Begin();
            _spriteBatch.Draw(player1Texture, player1Rect, Color.White);
            _spriteBatch.Draw(player2Texture, player2Rect, Color.White);
            _spriteBatch.Draw(ballTexture, ballRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
