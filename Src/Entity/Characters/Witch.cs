using HexHunt.Animations;
using HexHunt.Content;
using HexHunt.Entity.Effects.Attack;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Characters;

public class Witch : IEntity, IPositioned, IHittable
{
    private const float Speed = 90f;
    public Hitbox.Hitbox Hitbox { get; }
    public Team Team { get; init; } = Team.Friend;

    // The Witch should never despawn
    public ILifetime Lifetime { get; }
    public Vector2 Position { get; private set; }

    private readonly HitPoints _hitPoints = new(20);
    
    private readonly AnimationPlayer _animationPlayer;
    private readonly AnimationSet<WitchAction> _animationSet;
    private readonly AnimationSet<WitchEffect> _effectAnimations;

    private Vector2 _lastNonZeroDirection;
    private WitchInput _input;
    private WitchAction _witchAction = WitchAction.Idling;
    private EntityFacing _entityFacing = EntityFacing.Right;

    private readonly IEntitySpawner _entitySpawner;
    private const float Scale = 1.5f;

    public Witch(
        Vector2 position,
        AnimationSet<WitchAction> animationSet,
        AnimationSet<WitchEffect> effectAnimations,
        IEntitySpawner entitySpawner
    )
    {
        Position = position;
        _lastNonZeroDirection = Vector2.UnitX;
        _animationPlayer = new AnimationPlayer();
        _animationSet = animationSet;
        _effectAnimations = effectAnimations;
        _entitySpawner = entitySpawner;
        Hitbox = new Hitbox.Hitbox(Position, Scale, 2, 2);
        Lifetime = new DelayedExpirationLifetime(
            () => _witchAction == WitchAction.Death && _animationPlayer.IsFinished,
            2
        );

        SetState(WitchAction.Idling);
    }
    
    public void Update(float elapsedTime)
    {
        if (_witchAction == WitchAction.Death) { }
        // If attacking, freeze the character so that the animation first finishes
        else if (_witchAction is WitchAction.Attacking or WitchAction.TakeDamage)
        {
            if (_animationPlayer.IsFinished)
                SetState(WitchAction.Idling);
        }
        else
        {
            HandleInput(_input, elapsedTime);
        }

        _animationPlayer.Update(elapsedTime);
        Hitbox.Update(Position);
        Lifetime.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            Position,
            _entityFacing,
            Scale
        );
    }

    public void SetInput(WitchInput input)
    {
        _input = input;
    }
    
    public bool OnHit(Damage damage)
    {
        if (_hitPoints.IsDead) return false;
        
        _hitPoints.TakeDamage(damage.DamageAmount);
        SetState(_hitPoints.IsDead ? WitchAction.Death : WitchAction.TakeDamage);
        
        return true;
    }

    private void SetState(WitchAction action)
    {
        _witchAction = action;
        _animationPlayer.Play(_animationSet.Animations[_witchAction]);
    }

    private void HandleInput(WitchInput input, float elapsedTime)
    {
        if (input.Direction != Vector2.Zero) _lastNonZeroDirection = input.Direction;
        
        // Set state to either attacking, moving, or idling and if needed update position.
        if (input.AttackButtonPressed)
        {
            SetState(WitchAction.Attacking);
            SpawnAreaAttackLine(WitchEffect.IceSpike);
        }
        else
        {
            Position += input.Direction * Speed * elapsedTime;
            SetState(input.Direction != Vector2.Zero ? WitchAction.Moving : WitchAction.Idling);
        }

        // Set orientation
        _entityFacing = input.Direction.X switch
        {
            < 0 => EntityFacing.Left,
            > 0 => EntityFacing.Right,
            _ => _entityFacing
        };
    }

    private void SpawnAreaAttackLine(WitchEffect effect)
    {
        _entitySpawner.Spawn(
            new AreaAttackLine(
                _entitySpawner,
                Position,
                _lastNonZeroDirection,
                _effectAnimations.Animations[effect],
                4,
                new Damage
                {
                    DamageAmount = 5, 
                    Knockback = new Knockback
                    {
                        Direction = _lastNonZeroDirection,
                        Distance = 100,
                        Duration = 0.15f
                    }
                },
                team: Team.Friend,
                bufferBetweenAttackSeconds: 0.08f,
                distanceBetweenAttacksPixels: 25,
                shouldDespawnAfterAnimation: true
            )
        );
    }
}