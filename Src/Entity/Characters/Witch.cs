using HexHunt.Animations;
using HexHunt.Content;
using HexHunt.Entity.Effects.Attack;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Characters;

public class Witch : IEntity
{
    private const float Speed = 90f;
    
    // The Witch should never despawn
    public ILifetime Lifetime { get; } = new InfiniteLifetime();
    
    private readonly AnimationPlayer _animationPlayer;
    private readonly AnimationSet<WitchAction> _animationSet;
    private readonly AnimationSet<WitchEffect> _effectAnimations;

    private Vector2 _position;
    private Vector2 _lastNonZeroDirection;
    private WitchInput _input;
    private WitchAction _witchAction = WitchAction.Idling;
    private EntityFacing _entityFacing = EntityFacing.Right;

    private readonly IEntitySpawner _entitySpawner;

    public Witch(
        Vector2 position,
        AnimationSet<WitchAction> animationSet,
        AnimationSet<WitchEffect> effectAnimations,
        IEntitySpawner entitySpawner
    )
    {
        _position = position;
        _lastNonZeroDirection = Vector2.UnitX;
        _animationPlayer = new AnimationPlayer();
        _animationSet = animationSet;
        _effectAnimations = effectAnimations;
        _entitySpawner = entitySpawner;
        SetState(WitchAction.Idling);
    }
    
    public void Update(float elapsedTime)
    {
        // If attacking, freeze the character so that the animation first finishes
        if (_witchAction == WitchAction.Attacking)
        {
            if (_animationPlayer.IsFinished)
                SetState(WitchAction.Idling);
        }
        else
        {
            HandleInput(_input, elapsedTime);
        }

        _animationPlayer.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            _position,
            _entityFacing,
            1.5f
        );
    }

    public void SetInput(WitchInput input)
    {
        _input = input;
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
            _position += input.Direction * Speed * elapsedTime;
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
                _position,
                _lastNonZeroDirection,
                _effectAnimations.Animations[effect],
                4,
                bufferBetweenAttackSeconds: 0.08f,
                distanceBetweenAttacksPixels: 25,
                shouldDespawnAfterAnimation: true
            )
        );
    }
}