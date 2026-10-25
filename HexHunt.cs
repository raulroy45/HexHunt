using System.Collections.Generic;
using HexHunt.Assets;
using HexHunt.Assets.Characters;
using HexHunt.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexHunt
{
    public class HexHunt : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _previousKeyboardState;
        
        private const float FrameDuration = 0.1f;

        private List<IEntity> _entities = [];
        
        private Witch _player;
        
        public HexHunt()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player = new Witch(
                new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f),
                AnimationLoader.LoadCharacterAnimations<WitchAction>(
                    Content,
                    ArtCatalogue.BlueWitch,
                    FrameDuration
                )
            );
        }

        protected override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            var characterInput = new WitchInput();
            
            if (WasJustPressed(keyboardState, _previousKeyboardState, Keys.J))
                characterInput.AttackButtonPressed = true;
            
            characterInput.Direction = GetDirection(keyboardState);
            
            _player.SetInput(characterInput);
            _player.Update(elapsedTime);
            
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
            _player.Draw(_spriteBatch);
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
