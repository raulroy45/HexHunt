using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Animation;

public class Animation
{
    private readonly Texture2D _texture;
    private readonly int _frameHeight;
    private readonly int _frameWidth;
    private int _frameCount;
    private float _frameDuration;
    private bool _isLooping;
    private Vector2 _origin;

    /// <summary>
    /// Animation class for sprites
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="frameWidth"></param>
    /// <param name="frameHeight"></param>
    /// <param name="frameDuration"></param>
    /// <param name="isLooping"></param>
    /// <param name="origin"></param>
    public Animation(
        Texture2D texture,
        int frameWidth,
        int frameHeight,
        float frameDuration,
        bool isLooping = true,
        Vector2? origin = null
    )
    {
        _texture = texture;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameCount = _texture.Width / _frameWidth;
        _frameDuration = frameDuration;
        _isLooping = isLooping;
        _origin = origin ?? Vector2.Zero;
    }
    
    public Texture2D GetTexture()
    {
        return _texture;
    }
    
    public float GetFrameDuration()
    {
        return _frameDuration;
    }

    public int GetFrameCount()
    {
        return _frameCount;
    }

    public Rectangle GetSourceRectangle(int frameIndex)
    {
        return new Rectangle(_frameWidth * frameIndex, 0, _frameWidth, _frameHeight);
    }

    public bool IsLooping()
    {
        return _isLooping;
    }

    public int GetFrameWidth()
    {
        return _frameWidth;
    }

    public int GetFrameHeight()
    {
        return _frameHeight;
    }

    public Vector2 GetOrigin(Facing facing)
    {
        return facing == Facing.Left ? new Vector2(_frameWidth - _origin.X, _origin.Y) : _origin;
    }
}