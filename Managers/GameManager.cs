using HexHunt.Models.Characters;
using Microsoft.Xna.Framework;

namespace HexHunt.Managers
{
    public class GameManager
    {
        private BlueWitch _witch;

        public void Initialize() 
        {
            var viewport = Globals.Graphics.GraphicsDevice.Viewport;
            _witch = new BlueWitch(new Vector2(viewport.Width / 2f, viewport.Height / 2f));
        }

        public void Update() 
        {
            InputManager.Update();
            _witch.Update();
        }

        public void Draw()
        {
            _witch.Draw();
        }
    }
}
