using HexHunt.Models.Characters;
using Microsoft.Xna.Framework;

namespace HexHunt.Managers
{
    public class GameManager
    {
        private WitchManager _witchManager;
        private WitchBase _witch; 

        public void Initialize() 
        {
            var viewport = Globals.Graphics.GraphicsDevice.Viewport;
            _witchManager = new WitchManager(new Vector2(viewport.Width / 2f, viewport.Height / 2f));

            _witch = _witchManager.GetCurrentWitch();
        }

        public void Update() 
        {
            InputManager.Update();
            _witchManager.Update();
        }

        public void Draw()
        {
            _witchManager.Draw();
        }
    }
}
