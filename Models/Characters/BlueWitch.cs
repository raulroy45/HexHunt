using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Models.Characters
{
    public class BlueWitch : WitchBase
    {
        private Texture2D _idleTexture;
        private Texture2D _runTexture;
        private Vector2 _scale = Vector2.One * 2; // scale texture by two times
        private const string AssetDirectoryPath = "Characters/Blue Witch";

        public BlueWitch(Vector2 position = default)
        {
            LoadTextures();

            _animationManager.AddAnimationForRestPosition(new(_idleTexture, 1, 6, _defaultFrameRate, isVertical: true, scale: _scale));
            _animationManager.AddAnimationForFrontMovePositions(new(_runTexture, 1, 8, _defaultFrameRate, isVertical: true, scale: _scale));
            _animationManager.AddAnimationForBackMovePositions(new(_runTexture, 1, 8, _defaultFrameRate, isVertical: true, scale: _scale));
        }

        private void LoadTextures()
        {
            _idleTexture = Globals.Content.Load<Texture2D>($"{AssetDirectoryPath}/B_witch_idle");
            _runTexture = Globals.Content.Load<Texture2D>($"{AssetDirectoryPath}/B_witch_run");
        }
    }
}
