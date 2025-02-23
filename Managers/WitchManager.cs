using HexHunt.Models.Characters;
using Microsoft.Xna.Framework;

namespace HexHunt.Managers
{
    public class WitchManager
    {
        private Vector2 _position;

        private WitchBase[] _witches;
        private int _currentWitchIndex;

        public WitchManager(Vector2 position)
        {
            _currentWitchIndex = 0;
            _position = position;

            _witches = new WitchBase[3] {
                new BlueWitch(_position),
                new RedWitch(_position),
                new WhiteWitch(_position)
            };
        }

        public void Update()
        {
            if (InputManager.Cycle)
                _currentWitchIndex = (_currentWitchIndex + 1) % _witches.Length;

            if (_currentWitchIndex >= _witches.Length)
                _currentWitchIndex = 0;

            _witches[_currentWitchIndex].SetPosition(_position);
            _witches[_currentWitchIndex].Update();
        }

        public void Draw() 
        {
            _witches[_currentWitchIndex].Draw();
        }

        public WitchBase GetCurrentWitch()
        {
            return _witches[_currentWitchIndex];
        }
    }
}
