using System;
using HexHunt.Animation;
using HexHunt.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Assets.Characters;

public class Witch : IEntity
{
    private WitchAction _witchAction = WitchAction.Idling;
    private EntityFacing _entityFacing = EntityFacing.Right;
    
    private const float Speed = 90f;
    
    private readonly AnimationPlayer _animationPlayer;
    private readonly CharacterAnimations<WitchAction> _characterAnimationses;
    
    private Vector2 _position;
    private WitchInput _input;

    public Witch(Vector2 position, CharacterAnimations<WitchAction> characterAnimationses)
    {
        _position = position;
        _animationPlayer = new AnimationPlayer();
        _characterAnimationses = characterAnimationses;
        SetState(WitchAction.Idling);
    }

    public void SetInput(WitchInput input)
    {
        _input = input;
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
    
    private void SetState(WitchAction action)
    {
        _witchAction = action;
        _animationPlayer.Play(_characterAnimationses.Animations[_witchAction]);
    }
    
    private void HandleInput(WitchInput input, float elapsedTime)
    {
        // Set state to either attacking, moving, or idling and if needed update position.
        if (input.AttackButtonPressed)
        {
            SetState(WitchAction.Attacking);
            // TODO: CreateAttackObject(_position, _entityFacing);
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

    // TODO: need to implement this
    // private void CreateAttackObject(Vector2 position, EntityFacing entityFacing)
    // {
    //     throw new NotImplementedException();
    // }
}

