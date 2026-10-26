using System;
using HexHunt.Animation;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Effects.Attack;

public class AreaAttack : IEntity
{
    public ILifetime Lifetime { get; }
    
    private readonly AnimationPlayer _animationPlayer;
    
    private readonly Vector2 _position;
    private readonly Vector2 _direction;

    private class AttackAreaLifetime(bool shouldDespawnAfterAnimation,
        Func<bool> isAnimationFinished,
        float delayExpirationSeconds = 0.2f
    ) : ILifetime
    {
        public bool IsExpired { get; private set; } = false;
        private float _delayExpirationSecondsLeft = delayExpirationSeconds;

        public void Update(float elapsedTime)
        {
            if (!shouldDespawnAfterAnimation || !isAnimationFinished()) return;
            
            _delayExpirationSecondsLeft -= elapsedTime;
            if (_delayExpirationSecondsLeft <= 0) IsExpired = true;
        }
    }

    public AreaAttack(
        Vector2 position,
        Vector2 direction,
        Animation.Animation attackObjectAnimation,
        bool shouldDespawnAfterAnimation = true
    )
    {
        _position = position;
        _direction = direction;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(attackObjectAnimation);
        Lifetime = new AttackAreaLifetime(shouldDespawnAfterAnimation, () => _animationPlayer.IsFinished);
    }


    public void Update(float elapsedTime)
    {
        _animationPlayer.Update(elapsedTime);
        Lifetime.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            _position,
            GetFacingFromDirection(_direction),
            1.5f
        );
    }

    private static EntityFacing GetFacingFromDirection(Vector2 direction)
    {
        return direction.X >= 0 ? EntityFacing.Right : EntityFacing.Left;
    }
}