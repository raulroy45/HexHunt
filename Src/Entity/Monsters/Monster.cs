using HexHunt.Animations;
using HexHunt.Entity.Characters;
using HexHunt.Entity.Combat;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity.Monsters;

public class Monster : Character, IDamageSource
{
    public override Team Team => Team.Foe;
    public DamageSource DamageSource { get; }
    
    private float _freezeTimeLeft;
    private readonly IPositioned _targetPosition;
    private readonly MonsterDefinition _monsterDefinition;

    public Monster(
        MonsterDefinition monsterDefinition,
        Vector2 position,
        AnimationSet<CharacterAction> animationSet,
        IPositioned targetPosition)
        : base(position,
            animationSet,
            new HitPoints(monsterDefinition.HitPoints),
            new Hitbox(
                position,
                monsterDefinition.Scale,
                monsterDefinition.HitboxWidth,
                monsterDefinition.HitboxHeight
            ),
            monsterDefinition.Scale,
            delayDeathSeconds: monsterDefinition.DelayDeathSeconds)
    {
        _monsterDefinition = monsterDefinition;
        _targetPosition = targetPosition;
        DamageSource = new DamageSource(
            position: GetDamageSourcePosition(),
            scale: monsterDefinition.Scale,
            width: monsterDefinition.BiteBoxWidth,
            height: monsterDefinition.BiteBoxHeight,
            damage: new Damage
            {
                DamageAmount = monsterDefinition.DamageAmount
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
            if (AnimationPlayer.CurrentFrameIndex >= _monsterDefinition.AttackFrame) DamageSource.Activate();

            if (AnimationPlayer.IsFinished)
            {
                _freezeTimeLeft = _monsterDefinition.FreezeSeconds;
                SetState(CharacterAction.Idling);
            }
        }
        else if (direction == Vector2.Zero)
        {
            SetState(CharacterAction.Attacking);
        }
        else
        {
            Position += direction * _monsterDefinition.Speed * dt;
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
        _freezeTimeLeft = _monsterDefinition.FreezeSeconds;
        SetState(CharacterAction.Idling);
    }

    private Vector2 GetDirection()
    {
        var direction = _targetPosition.Position - GetDamageSourcePosition();

        if (direction.Length() < _monsterDefinition.AttackRadiusPixels)
            return Vector2.Zero;

        if (direction != Vector2.Zero)
            direction.Normalize();

        return direction;
    }

    private Vector2 GetDamageSourcePosition()
    {
        var offset = _monsterDefinition.DamageSourceOffset * _monsterDefinition.Scale;

        if (EntityFacing == EntityFacing.Left)
            offset.X = -offset.X;

        return Position + offset;
    }

    protected override void SetState(CharacterAction nextAction)
    {
        var isLeavingAttack = (Action == CharacterAction.Attacking && nextAction != CharacterAction.Attacking);
        
        base.SetState(nextAction);

        if (!isLeavingAttack) return;
        
        DamageSource.Deactivate();
        DamageSource.ClearEarlierHits();
    }
}