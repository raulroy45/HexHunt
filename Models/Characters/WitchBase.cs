using HexHunt.Managers;
using Microsoft.Xna.Framework;

namespace HexHunt.Models.Characters
{
    public class WitchBase : CharacterBase
    {
        public override void Update()
        {
            _animationManager.Update(InputManager.Direction);
        }

        public override void Draw()
        {
            _animationManager.Draw(_position, InputManager.Direction);
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }
    }
}
