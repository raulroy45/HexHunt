using HexHunt.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Models.Characters
{
    public abstract class CharacterBase : GameObject
    {
        protected readonly float _speed = 20f;
        protected readonly AnimationManager _animationManager = new();

        public abstract void Update();

        public abstract void Draw();
    }
}
