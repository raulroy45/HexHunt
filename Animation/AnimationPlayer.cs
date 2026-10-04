using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Animation;

public class AnimationPlayer
{
    private Animation _currentAnimation;
    private int _currentFrameIndex;
    private float _timerLeft;
    
    public bool IsFinished { get; private set; }

    public void Play(Animation animation)
    {
        if (_currentAnimation == animation)
            return;
        
        _currentAnimation = animation;
        _currentFrameIndex = 0;
        _timerLeft = _currentAnimation.GetFrameDuration();
        IsFinished = false;
    }

    public void Update(float elapsedTime)
    {
        _timerLeft -= elapsedTime;
        
        if (!(_timerLeft <= 0) || IsFinished) return;
        
        if (_currentFrameIndex >= _currentAnimation.GetFrameCount() - 1)
        {
            if (_currentAnimation.IsLooping())
                _currentFrameIndex = 0;
            else
                IsFinished = true;
        }
        else
            _currentFrameIndex++;
            
        _timerLeft = _currentAnimation.GetFrameDuration();
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Facing facing, float scale)
    {
        var origin = _currentAnimation.GetOrigin(facing);
        var effects = facing == Facing.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        spriteBatch.Draw(
            _currentAnimation.GetTexture(),
            position,
            _currentAnimation.GetSourceRectangle(_currentFrameIndex),
            Color.White,
            0f,
            origin,
            scale,
            effects,
            0f
        );
    }
}

public enum Facing
{
    Left,
    Right
}