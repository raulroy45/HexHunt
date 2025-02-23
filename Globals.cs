using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HexHunt
{
    public static class Globals
    {
        public static ContentManager Content {  get; set; } 
        public static GraphicsDeviceManager Graphics { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static double TotalSecondsElapsed { get; set; }

        public static void Update(GameTime gt)
        {
            TotalSecondsElapsed = gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
