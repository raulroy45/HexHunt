using System.Collections.Generic;
using HexHunt.Animations;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Effects.Attack;

public class AreaAttack : IEntity, IDamageSource
{
    private const float Scale = 1.5f;
    private const int StartDamageFrame = 4;
    
    public ILifetime Lifetime { get; }
    public DamageSource DamageSource { get; }
    
    private readonly AnimationPlayer _animationPlayer;
    
    private readonly Vector2 _position;
    private readonly Vector2 _direction;

    public AreaAttack(
        Vector2 position,
        Vector2 direction,
        Animation attackObjectAnimation,
        Damage damage,
        bool shouldDespawnAfterAnimation = true,
        HashSet<IHittable>? hasHit = null
    )
    {
        _position = position;
        _direction = direction;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(attackObjectAnimation);
        Lifetime = new DelayedExpirationLifetime(() => shouldDespawnAfterAnimation && _animationPlayer.IsFinished);
        DamageSource = new DamageSource(_position, Scale, 10, 10, damage, hasHit: hasHit);
    }


    public void Update(float elapsedTime)
    {
        _animationPlayer.Update(elapsedTime);
        
        if (_animationPlayer.CurrentFrameIndex >= StartDamageFrame)
            DamageSource.Activate();
        
        Lifetime.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            _position,
            GetFacingFromDirection(_direction),
            Scale
        );
    }


    private static EntityFacing GetFacingFromDirection(Vector2 direction)
    {
        return direction.X >= 0 ? EntityFacing.Right : EntityFacing.Left;
    }
}