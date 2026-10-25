using System;
using System.Collections.Generic;
using HexHunt.Animation;
using HexHunt.Assets.Effects.Attack;
using HexHunt.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Assets.Characters;

public class Witch : IEntity
{
    private const float Speed = 90f;

    private readonly AnimationPlayer _animationPlayer;
    private readonly AnimationSet<WitchAction> _animationSet;
    private readonly AnimationSet<WitchEffect> _effectAnimations;

    private Vector2 _position;
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
        // Set state to either attacking, moving, or idling and if needed update position.
        if (input.AttackButtonPressed)
        {
            SetState(WitchAction.Attacking);
            SpawnObject(WitchEffect.IceSpike);
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

    private void SpawnObject(WitchEffect effect)
    {
        if (_input.Direction != Vector2.Zero)
            _input.Direction.Normalize();
        
        _entitySpawner.Spawn(
            new AttackObject(
                _position + (_input.Direction * 25),
                _input.Direction,
                _effectAnimations.Animations[effect]
            )
        );
    }
}