using HexHunt.Animations;
using HexHunt.Entity.Characters;
using HexHunt.Entity.Combat;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity.Monsters;

public class Rat : Character, IDamageSource
{
    public override Team Team => Team.Foe;
    public DamageSource DamageSource { get; }
    
    private const float Speed = 40f;
    private const float FreezeSeconds = 0.75f;
    private const int BiteRadiusPixels = 2;
    private const float Scale = 0.5f;
    private const float DamageOffsetX = 12.5f;
    private const float DamageOffsetY = 4.5f;
    private const int AttackFrame = 8;
    
    private float _freezeTimeLeft;
    private readonly IPositioned _targetPosition;
    
    public Rat(
        Vector2 position,
        AnimationSet<CharacterAction> animationSet,
        IPositioned targetPosition)
        : base(position,
            animationSet,
            new HitPoints(10),
            new Hitbox(position, Scale, 30, 30),
            Scale,
            delayDeathSeconds: 0.75f)
    {
        _targetPosition = targetPosition;
        DamageSource = new DamageSource(
            position: GetDamageSourcePosition(),
            scale: Scale,
            width: 10,
            height: 10,
            damage: new Damage
            {
                DamageAmount = 2
            },
            team: Team.Foe);
    }

    protected override void UpdateBehaviors(float dt)
    {
        var direction = GetDirection();

        if (_freezeTimeLeft > 0)
        {
            _freezeTimeLeft -= dt;
        }
        else if (Action == CharacterAction.Attacking)
        {
            if (AnimationPlayer.CurrentFrameIndex >= AttackFrame) DamageSource.Activate();

            if (AnimationPlayer.IsFinished)
            {
                _freezeTimeLeft = FreezeSeconds;
                SetState(CharacterAction.Idling);
            }
        }
        else if (direction == Vector2.Zero)
        {
            SetState(CharacterAction.Attacking);
        }
        else
        {
            Position += direction * Speed * dt;
            SetState(CharacterAction.Moving);
        }

        // Only switch face when running/chasing
        if (Action == CharacterAction.Moving)
            EntityFacing = (_targetPosition.Position.X - Position.X) switch
            {
                < 0 => EntityFacing.Left,
                > 0 => EntityFacing.Right,
                _ => EntityFacing
            };

        DamageSource.Update(GetDamageSourcePosition());
    }

    protected override void OnHurtFinished()
    {
        _freezeTimeLeft = FreezeSeconds;
        SetState(CharacterAction.Idling);
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
        return EntityFacing == EntityFacing.Right
            ? new Vector2(Position.X + DamageOffsetX, Position.Y - DamageOffsetY)
            : new Vector2(Position.X - DamageOffsetX, Position.Y - DamageOffsetY);
    }

    protected override void SetState(CharacterAction nextAction)
    {
        var hasStartedAttacking = (Action != CharacterAction.Attacking || nextAction == CharacterAction.Attacking);
        
        base.SetState(nextAction);

        if (hasStartedAttacking) return;
        
        DamageSource.Deactivate();
        DamageSource.ClearEarlierHits();
    }
}