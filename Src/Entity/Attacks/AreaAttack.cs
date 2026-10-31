using System.Collections.Generic;
using HexHunt.Animations;
using HexHunt.Entity.Combat;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Attacks;

public class AreaAttack : IEntity, IDamageSource
{
    private const float Scale = 1f;
    private const int StartDamageFrame = 4;

    private readonly AnimationPlayer _animationPlayer;
    private readonly Vector2 _direction;

    public AreaAttack(
        Vector2 position,
        Vector2 direction,
        Animation attackObjectAnimation,
        Damage damage,
        Team team,
        bool shouldDespawnAfterAnimation = true,
        HashSet<IHittable>? hasHit = null
    )
    {
        Position = position;
        _direction = direction;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(attackObjectAnimation);
        Lifetime = new DelayedExpirationLifetime(() => shouldDespawnAfterAnimation && _animationPlayer.IsFinished);
        DamageSource = new DamageSource(Position, Scale, 10, 10, damage, team, hasHit: hasHit);
    }

    public DamageSource DamageSource { get; }

    public ILifetime Lifetime { get; }

    public Vector2 Position { get; }


    public void Update(float dt)
    {
        _animationPlayer.Update(dt);

        if (_animationPlayer.CurrentFrameIndex >= StartDamageFrame)
            DamageSource.Activate();

        Lifetime.Update(dt);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            Position,
            GetFacingFromDirection(_direction),
            Scale
        );
    }


    private static EntityFacing GetFacingFromDirection(Vector2 direction)
    {
        return direction.X >= 0 ? EntityFacing.Right : EntityFacing.Left;
    }
}