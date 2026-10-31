using HexHunt.Animations;
using HexHunt.Entity.Combat;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Characters;

public abstract class Character : IEntity, IHittable
{
    public Vector2 Position { get; protected set; }
    public ILifetime Lifetime { get; }
    public Hitbox Hitbox { get; }
    public abstract Team Team { get; }
    
    protected AnimationPlayer AnimationPlayer { get; }
    protected AnimationSet<CharacterAction> AnimationSet { get; }
    protected CharacterAction Action { get; private set; }
    protected HitPoints HitPoints { get; set; }
    protected EntityFacing EntityFacing { get; set; } = EntityFacing.Right;
    protected readonly KnockbackMotion KnockbackMotion = new();
    
    private float Scale { get; set; } = 1f;

    protected Character(
        Vector2 position,
        AnimationSet<CharacterAction> animationSet,
        HitPoints hitPoints,
        Hitbox hitbox,
        float scale = 1f,
        float delayDeathSeconds = 0.2f)
    {
        Position = position;
        AnimationSet = animationSet;
        HitPoints = hitPoints;
        Hitbox = hitbox;
        Scale = scale;

        AnimationPlayer = new AnimationPlayer();
        Lifetime = new DelayedExpirationLifetime(
            () => Action == CharacterAction.Death && AnimationPlayer.IsFinished,
            delayDeathSeconds);
        Action = CharacterAction.Idling;
        
        AnimationPlayer.Play(AnimationSet.Animations[Action]);
    }


    public void Update(float dt)
    {
        if (Action == CharacterAction.Death)
        {
        }
        // If taking damage, freeze the character so that the animation first finishes
        else if (Action is CharacterAction.TakeDamage)
        {
            if (AnimationPlayer.IsFinished) OnHurtFinished();
        }
        else UpdateBehaviors(dt);
        
        Position += KnockbackMotion.Update(dt);
        AnimationPlayer.Update(dt);
        Hitbox.Update(Position);
        Lifetime.Update(dt);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        AnimationPlayer.Draw(
            spriteBatch,
            Position,
            EntityFacing,
            Scale
        );
    }
    
    public bool OnHit(Damage damage)
    {
        if (HitPoints.IsDead) return false;

        HitPoints.TakeDamage(damage.DamageAmount);
        SetState(HitPoints.IsDead ? CharacterAction.Death : CharacterAction.TakeDamage);
        if (damage.Knockback is not null)
            KnockbackMotion.Start(damage.Knockback);

        return true;
    }

    protected virtual void SetState(CharacterAction action)
    {
        Action = action;
        AnimationPlayer.Play(AnimationSet.Animations[action]);
    }

    protected abstract void UpdateBehaviors(float dt);
    protected abstract void OnHurtFinished();
}