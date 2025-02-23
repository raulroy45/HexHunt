using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HexHunt.Managers
{
    public class AnimationManager
    {
        private readonly Dictionary<Vector2, Animation> _animations = new();
        private Vector2 _lastKey = Vector2.Zero; // Default to (0,0)

        public void AddAnimation(Vector2 key, Animation animation)
        {
            if (!_animations.ContainsKey(key))
            {
                _animations[key] = animation;
                _lastKey = key;
            }
        }

        public void AddAnimationForRestPosition(Animation animation)
        {
            AddAnimation(Vector2.Zero, animation);
        }

        public void AddAnimationForFrontMovePositions(Animation animation)
        {
            Vector2[] moveKeys =
            {
                new(0, 1), new(-1, 0), new(1, 0), new(0, -1),
                new(-1, 1), new(-1, -1), new(1, 1), new(1, -1)
            };

            foreach (var key in moveKeys)
                AddAnimation(key, animation);
        }
        public void AddAnimationForBackMovePositions(Animation animation)
        {
            Vector2[] moveKeys =
            {
                new(0, 1), new(-1, 0), new(1, 0), new(0, -1),
                new(-1, 1), new(-1, -1), new(1, 1), new(1, -1)
            };

            foreach (var key in moveKeys)
                AddAnimation(key, animation);
        }

        public void Update(Vector2 key)
        {
            if (_animations.TryGetValue(key, out var newAnimation))
            {
                if (_lastKey != key) // Only switch if the key is different
                {
                    _animations[_lastKey].Stop();
                    newAnimation.Start();
                    _lastKey = key;
                }

                newAnimation.Update();
            }
            else
            {
                _animations[_lastKey].Update(); // Keep playing last valid animation
            }
        }

        public void Draw(Vector2 position, Vector2 direction)
        {
            _animations[_lastKey].Draw(position, InputManager.Direction);
        }
    }
}

