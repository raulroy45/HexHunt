using HexHunt.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexHunt
{
    public class HexHunt : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _previousKeyboardState;
        
        private Witch _witch;

        public HexHunt()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _witch = new Witch();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _witch.Initialize(new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var origin = new Vector2(26, 42);
            var stateTextures = GetWitchTextures(Content);
            _witch.Load(stateTextures, origin);
        }

        private static CharacterStateTextures GetWitchTextures(ContentManager content)
        {
            return new CharacterStateTextures
            {
                Idle = content.Load<Texture2D>("Characters/Blue Witch/B_witch_idle"),
                Move = content.Load<Texture2D>("Characters/Blue Witch/B_witch_run"),
                Attack = content.Load<Texture2D>("Characters/Blue Witch/B_witch_attack")
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            var characterInput = new CharacterInput();
            
            if (WasJustPressed(keyboardState, _previousKeyboardState, Keys.J))
                characterInput.AttackButtonPressed = true;
            
            characterInput.Direction = GetDirection(keyboardState);
            
            _witch.Update(elapsedTime, characterInput);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
            
            _previousKeyboardState = keyboardState;
        }

        private static bool WasJustPressed(KeyboardState currKeyboardState, KeyboardState prevKeyboardState, Keys key)
        {
            return currKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp); 
            _witch.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private static Vector2 GetDirection(KeyboardState keyboardState)
        {
            var direction = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1f;

            if (keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1f;

            if (keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1f;

            if (keyboardState.IsKeyDown(Keys.D))
                direction.X += 1f;
            
            if (direction != Vector2.Zero)
                direction.Normalize();
            
            return direction;
        }
    }
}
