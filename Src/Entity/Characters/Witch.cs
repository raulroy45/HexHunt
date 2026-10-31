using HexHunt.Animations;
using HexHunt.Entity.Attacks;
using HexHunt.Entity.Combat;
using HexHunt.Input;
using HexHunt.World;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity.Characters;

public class Witch(
    Vector2 position,
    AnimationSet<CharacterAction> animationSet,
    AnimationSet<WitchEffect> effectAnimations,
    IEntitySpawner entitySpawner)
    : Character(position, animationSet, new HitPoints(20), new Hitbox(position, Scale, 2, 2), scale: Scale, delayDeathSeconds: 2)
{
    public override Team Team => Team.Friend;

    private const float Speed = 45f;
    private const float Scale = 1f;

    private WitchInput _input;
    private Vector2 _lastAimDirection = Vector2.UnitX;

    protected override void UpdateBehaviors(float dt)
    {
        if (Action is CharacterAction.Attacking)
        {
            if (AnimationPlayer.IsFinished) SetState(CharacterAction.Idling);
        }
        else HandleInput(_input, dt);
    }

    protected override void OnHurtFinished()
    {
        SetState(CharacterAction.Idling);
    }

    public void SetInput(WitchInput input)
    {
        _input = input;
    }

    private void HandleInput(WitchInput input, float elapsedTime)
    {
        var aim = input.AimTarget - Position;
        if (aim != Vector2.Zero) _lastAimDirection = Vector2.Normalize(aim);

        // Set state to either attacking, moving, or idling and if needed update position.
        if (input.AttackButtonPressed)
        {
            SetState(CharacterAction.Attacking);
            SpawnAreaAttackLine(WitchEffect.IceSpike);
        }
        else
        {
            Position += input.MoveDirection * Speed * elapsedTime;
            SetState(input.MoveDirection != Vector2.Zero ? CharacterAction.Moving : CharacterAction.Idling);
        }

        // Set orientation
        var directionFacingAdheres =
            Action == CharacterAction.Attacking ? _lastAimDirection.X : input.MoveDirection.X;

        EntityFacing = directionFacingAdheres switch
        {
            < 0 => EntityFacing.Left,
            > 0 => EntityFacing.Right,
            _ => EntityFacing
        };
    }

    private void SpawnAreaAttackLine(WitchEffect effect)
    {
        entitySpawner.Spawn(
            new AreaAttackLine(
                entitySpawner: entitySpawner,
                position: Position,
                direction: _lastAimDirection,
                areaAttackAnimation: effectAnimations.Animations[effect],
                totalAttacks: 4,
                damage: new Damage
                {
                    DamageAmount = 5,
                    Knockback = new Knockback
                    {
                        Direction = _lastAimDirection,
                        Distance = 50,
                        Duration = 0.15f
                    }
                },
                team: Team.Friend,
                bufferBetweenAttackSeconds: 0.08f,
                distanceBetweenAttacksPixels: 12
            )
        );
    }
}