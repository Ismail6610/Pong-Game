using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Win32;

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
            //commits
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

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Rectangle player1Rect = new Rectangle((int)player1Position.X, (int)player1Position.Y, (int)player1width, (int)player1height);
            Rectangle player2Rect = new Rectangle((int)player2Position.X, (int)player2Position.Y, (int)player2width, (int)player2height);

            GraphicsDevice.Clear(new Color(34,34,34));
            _spriteBatch.Begin();
            _spriteBatch.Draw(player1Texture, player1Rect, Color.White);
            _spriteBatch.Draw(player2Texture, player2Rect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
