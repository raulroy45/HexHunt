using HexHunt.Animations;
using HexHunt.Content;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Monsters;

public class Rat : IEntity, IHittable, IDamageSource
{
    public Hitbox.Hitbox Hitbox { get; }
    public Team Team { get; init; } = Team.Foe;
    public DamageSource DamageSource { get; }
    
    private const float Speed = 80f;
    private const float FreezeSeconds = 0.75f;
    private const int BiteRadiusPixels = 3;
    private const float Scale = 1f;
    private const float DamageOffsetX = 25;
    private const float DamageOffsetY = 9;
    private const int AttackFrame = 8;
    
    private Vector2 _position;
    private EntityFacing _facing =  EntityFacing.Right;
    private MonsterAction _currentState =  MonsterAction.Idling;
    private float _freezeTimeLeft = 0;
    private readonly HitPoints _hitPoints;

    private readonly AnimationSet<MonsterAction> _animationSet;
    private readonly AnimationPlayer _animationPlayer;
    private readonly IPositioned _targetPosition;
    private readonly KnockbackMotion _knockbackMotion = new();



    public Rat(Vector2 position, AnimationSet<MonsterAction> animationSet, IPositioned targetPosition)
    {
        _position = position;
        _animationSet = animationSet;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(animationSet.Animations[MonsterAction.Idling]);
        _targetPosition = targetPosition;

        Hitbox = new Hitbox.Hitbox(
            _position,
            Scale,
            30,
            30
        );
        _hitPoints = new HitPoints(10);

        Lifetime = new DelayedExpirationLifetime(
            () => _currentState == MonsterAction.Death && _animationPlayer.IsFinished, 0.75f);
        
        DamageSource = new DamageSource(
            GetDamageSourcePosition(),
            Scale, 
            10, 
            10, 
            new Damage
            {
                DamageAmount = 2
            },
            team: Team.Foe,
            isActive: false);
    }

    public ILifetime Lifetime { get; }

    // The Rat monster chases the target and then tries to bite her,
    // freezes after its bite and the chases are again.
    public void Update(float timeElapsed)
    {
        var direction = GetDirection();

        if (_currentState == MonsterAction.Death)
        { } // Let the death animation play
        else if (_currentState == MonsterAction.TakeDamage)
        {
            if (_animationPlayer.IsFinished)
            {
                _freezeTimeLeft = FreezeSeconds;
                SetState(MonsterAction.Idling);
            }
        }
        else if (_freezeTimeLeft > 0)
        {
            _freezeTimeLeft -= timeElapsed;
        } 
        else if (_currentState == MonsterAction.Attacking)
        {
            if (_animationPlayer.CurrentFrameIndex >= AttackFrame)
            {
                DamageSource.Activate();
            }
            
            if (_animationPlayer.IsFinished)
            {
                _freezeTimeLeft = FreezeSeconds;
                SetState(MonsterAction.Idling);
            }
        } 
        else if (direction == Vector2.Zero)
        {
            SetState(MonsterAction.Attacking);
        }
        else
        {
            _position += direction * Speed * timeElapsed;
            SetState(MonsterAction.Running);
        }

        // Only switch face when running/chasing
        if (_currentState == MonsterAction.Running)
        {
            _facing = (_targetPosition.Position.X - _position.X) switch
            {
                < 0 => EntityFacing.Left,
                > 0 => EntityFacing.Right,
                _ => _facing
            };
        }
        
        _position += _knockbackMotion.Update(timeElapsed);
        _animationPlayer.Update(timeElapsed);
        DamageSource.Update(GetDamageSourcePosition());
        Hitbox.Update(_position);
        Lifetime.Update(timeElapsed);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(spriteBatch, _position, _facing, Scale);
    }

    private Vector2 GetDirection()
    {
        var direction = _targetPosition.Position - GetDamageSourcePosition();

        if (direction.Length() < BiteRadiusPixels)
            return Vector2.Zero;

        if (direction != Vector2.Zero)
            direction.Normalize();
        
        return direction;
    }

    private Vector2 GetDamageSourcePosition()
    {
        return _facing == EntityFacing.Right
            ? new Vector2(_position.X + DamageOffsetX, _position.Y - DamageOffsetY)
            : new Vector2(_position.X - DamageOffsetX, _position.Y - DamageOffsetY);
    }

    private void SetState(MonsterAction action)
    {
        if (_currentState == MonsterAction.Attacking && action !=  MonsterAction.Attacking)
        {
            DamageSource.Deactivate();
            DamageSource.ClearEarlierHits();
        }
        _currentState = action;
        _animationPlayer.Play(_animationSet.Animations[action]);
    }

    public bool OnHit(Damage damage)
    {
        if (_hitPoints.IsDead) return false;
        
        _hitPoints.TakeDamage(damage.DamageAmount);
        SetState(_hitPoints.IsDead ? MonsterAction.Death : MonsterAction.TakeDamage);
        if (damage.Knockback is not null)
            _knockbackMotion.Start(damage.Knockback);
        
        return true;
    }
}