using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HexHunt
{
    public class Animation
    {
        private readonly Texture2D _texture;
        private readonly List<Rectangle> _sourceRectangles = new();
        private readonly int _frames;
        private int _frame;
        private readonly float _frameTime;
        private float _frameTimeLeft;
        private bool _active = true;
        private bool _shouldFlipHorizontally = false;
        private Vector2 _scale = Vector2.One;

        public Animation(
            Texture2D texture, 
            int framesX, 
            int framesY, 
            float frameTime, 
            int textureIndex = 0, 
            bool isVertical = false,
            Vector2 scale = default)
        {
            _texture = texture;
            _frameTime = _frameTimeLeft = frameTime;
            var frameWidth = _texture.Width / framesX;
            var frameHeight = _texture.Height / framesY;

            if (isVertical)
            {
                _frames = framesY;

                for (var i = 0; i < _frames; i++)
                    _sourceRectangles.Add(new Rectangle(textureIndex * frameWidth, i * frameHeight, frameWidth, frameHeight));
            }
            else
            {
                _frames = framesX;

                for (var i = 0; i < _frames; i++)
                    _sourceRectangles.Add(new Rectangle(textureIndex * frameWidth, i * frameHeight, frameWidth, frameHeight));
            }

            if (scale != default)
                _scale = scale;
        }

        public void Stop()
        {
            _active = false;
        }

        public void Start()
        {
            _active = true;
        }

        public void Reset()
        {
            _frame = 0;
            _frameTimeLeft = _frameTime;
        }

        public void Update()
        {
            if (!_active) return;

            _frameTimeLeft -= (float)Globals.TotalSecondsElapsed;

            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;
                _frame = (_frame + 1) % _frames;
            }
        }

        public void Draw(Vector2 pos, Vector2 direction)
        {
            var spriteEffects = SpriteEffects.None;
            if (direction.X < 0)
                _shouldFlipHorizontally = true;
            else if (direction.X > 0)
                _shouldFlipHorizontally = false;

            if (_shouldFlipHorizontally)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Globals.SpriteBatch.Draw(_texture, pos, _sourceRectangles[_frame], Color.White, 0, Vector2.Zero, _scale, spriteEffects, 1);
        }
    }
}
