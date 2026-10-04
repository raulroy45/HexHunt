using System;
using HexHunt.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexHunt
{
    public class HexHunt : Game
    {
        private enum SpriteState
        {
            Idling,
            Moving,
            Attacking
        }

        private const int SingleFrameWidth = 111;
        private const int SingleFrameHeight = 48;
        private const float FrameDuration = 0.1f;
        private const float Speed = 90f;
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly AnimationPlayer _animationPlayer;
        
        private Animation.Animation _blueWitchIdle;
        private Animation.Animation _blueWitchRun;
        private Animation.Animation _blueWitchAttack;
        private SpriteState _currentSpriteState = SpriteState.Idling;
        
        private Vector2 _blueWitchPosition;
        
        private KeyboardState _previousKeyboardState;
        
        private Facing _facing = Facing.Right;

        public HexHunt()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _animationPlayer =  new AnimationPlayer();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _blueWitchPosition = new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
            SetState(SpriteState.Idling);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var origin = new Vector2(26, 42);
            var blueWitchIdleTexture = Content.Load<Texture2D>("Characters/Blue Witch/B_witch_idle");
            _blueWitchIdle = new Animation.Animation(
                blueWitchIdleTexture,
                SingleFrameWidth,
                SingleFrameHeight,
                FrameDuration,
                origin: origin
            );
            var blueWitchRunTexture =  Content.Load<Texture2D>("Characters/Blue Witch/B_witch_run");
            _blueWitchRun = new Animation.Animation(
                blueWitchRunTexture,
                SingleFrameWidth,
                SingleFrameHeight,
                FrameDuration,
                origin: origin
            );
            
            var blueWitchAttackTexture = Content.Load<Texture2D>("Characters/Blue Witch/B_witch_attack");
            _blueWitchAttack = new Animation.Animation(
                blueWitchAttackTexture,
                SingleFrameWidth,
                SingleFrameHeight,
                FrameDuration,
                isLooping: false,
                origin: origin
            );
        }

        protected override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            if (_currentSpriteState == SpriteState.Attacking)
            {
                if (_animationPlayer.IsFinished)
                    SetState(SpriteState.Idling);
            }
            else if (WasJustPressed(keyboardState, _previousKeyboardState, Keys.J))
            {
                SetState(SpriteState.Attacking);
            }
            else
            {
                // We can only move when not attacking
                UpdatePosition(keyboardState, elapsedTime);
            }
            
            _animationPlayer.Update(elapsedTime);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
            
            _previousKeyboardState = keyboardState;
        }

        private static bool WasJustPressed(KeyboardState currKeyboardState, KeyboardState prevKeyboardState, Keys key)
        {
            return currKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }

        private Animation.Animation GetAnimationForState(SpriteState state)
        {
            return state switch
            {
                SpriteState.Attacking => _blueWitchAttack,
                SpriteState.Idling => _blueWitchIdle,
                SpriteState.Moving => _blueWitchRun,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp); 
            _animationPlayer.Draw(
                _spriteBatch,
                _blueWitchPosition,
                _facing,
                1.5f
            );
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void SetState(SpriteState newState)
        {
            _currentSpriteState = newState;
            var animation = GetAnimationForState(newState);
            _animationPlayer.Play(animation);
        }

        private void UpdatePosition(KeyboardState keyboardState, float elapsedTime)
        {
            var direction = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                direction.Y -= 1f;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                direction.Y += 1f;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                direction.X -= 1f;
                _facing = Facing.Left;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                direction.X += 1f;
                _facing = Facing.Right;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                SetState(SpriteState.Moving);
            } else
            {
                SetState(SpriteState.Idling);
            }

            _blueWitchPosition += direction * Speed * elapsedTime;
        }
    }
}
