using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HexHunt.Managers
{
    public static class InputManager
    {
        private static KeyboardState _previousKeyboardState;

        private static Vector2 _direction;
        public static Vector2 Direction => _direction;
        public static bool Moving => _direction != Vector2.Zero;

        public static bool Cycle { get; private set; }

        public static void Update()
        {
            _direction = Vector2.Zero;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
            if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;
            if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
            if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;

            Cycle = keyboardState.IsKeyDown(Keys.X) && _previousKeyboardState.IsKeyUp(Keys.X);

            // Store current keyboard state for next frame
            _previousKeyboardState = keyboardState;
        }
    }
}

