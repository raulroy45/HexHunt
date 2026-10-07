using System;
using HexHunt.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Characters;

public class Witch
{
    private Vector2 _position;
    private CharacterState _characterState = CharacterState.Idling;
    private CharacterFacing _characterFacing = CharacterFacing.Right;
    
    private const int SingleFrameWidth = 111;
    private const int SingleFrameHeight = 48;
    private const float FrameDuration = 0.1f;
    private const float Speed = 90f;
    
    private Animation.Animation _blueWitchIdle;
    private Animation.Animation _blueWitchRun;
    private Animation.Animation _blueWitchAttack;
    
    private AnimationPlayer _animationPlayer;

    public void Initialize(Vector2 position)
    {
        _position = position;
        _animationPlayer = new AnimationPlayer();
        SetState(CharacterState.Idling);
    }

    public void Load(CharacterStateTextures textures, Vector2 origin)
    {
        _blueWitchIdle = new Animation.Animation(
            textures.Idle,
            SingleFrameWidth,
            SingleFrameHeight,
            FrameDuration,
            origin: origin
        );
        _blueWitchRun = new Animation.Animation(
            textures.Move,
            SingleFrameWidth,
            SingleFrameHeight,
            FrameDuration,
            origin: origin
        );
            
        _blueWitchAttack = new Animation.Animation(
            textures.Attack,
            SingleFrameWidth,
            SingleFrameHeight,
            FrameDuration,
            isLooping: false,
            origin: origin
        );
    }

    public void Update(float elapsedTime, CharacterInput input)
    {
        // If attacking, freeze the character so that the animation first finishes
        if (_characterState == CharacterState.Attacking)
        {
            if (_animationPlayer.IsFinished)
                SetState(CharacterState.Idling);
        } 
        else
        {
            HandleInput(input, elapsedTime);
        }
        
        _animationPlayer.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            _position,
            _characterFacing,
            1.5f
        );
    }
    
    private void SetState(CharacterState state)
    {
        _characterState = state;
        var animation = GetAnimationForState(state);
        _animationPlayer.Play(animation);
    }
    
    private void HandleInput(CharacterInput input, float elapsedTime)
    {
        // Set state to either attacking, moving, or idling and if needed update position.
        if (input.AttackButtonPressed)
        {
            SetState(CharacterState.Attacking);
        }
        else
        {
            _position += input.Direction * Speed * elapsedTime;
            SetState(input.Direction != Vector2.Zero ? CharacterState.Moving : CharacterState.Idling);
        }
        
        // Set orientation
        _characterFacing = input.Direction.X switch
        {
            < 0 => CharacterFacing.Left,
            > 0 => CharacterFacing.Right,
            _ => _characterFacing
        };
    }
    
    private Animation.Animation GetAnimationForState(CharacterState state)
    {
        return state switch
        {
            CharacterState.Attacking => _blueWitchAttack,
            CharacterState.Idling => _blueWitchIdle,
            CharacterState.Moving => _blueWitchRun,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

