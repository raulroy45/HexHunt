using HexHunt.Animations;
using HexHunt.Content;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Monsters;

public class Rat : IEntity
{
    private const float Speed = 80f;
    private const float FreezeAfterBiteSeconds = 0.75f;
    private const int BiteRadiusPixels = 20;
    
    private Vector2 _position;
    private EntityFacing _facing =  EntityFacing.Right;
    private MonsterAction _currentState =  MonsterAction.Idling;
    private float _freezeTimeLeft = 0;

    private readonly AnimationSet<MonsterAction> _animationSet;
    private readonly AnimationPlayer _animationPlayer;
    private readonly IPositioned _targetPosition;

    public Rat(Vector2 position, AnimationSet<MonsterAction> animationSet, IPositioned targetPosition)
    {
        _position = position;
        _animationSet = animationSet;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(animationSet.Animations[MonsterAction.Idling]);
        _targetPosition = targetPosition;
    }

    public ILifetime Lifetime { get; } = new InfiniteLifetime();

    // The Rat monster chases the Witch and then tries to bite her,
    // freezes after its bite and the chases are again.
    public void Update(float timeElapsed)
    {
        
        var direction = GetDirection();
        
        if (_freezeTimeLeft > 0)
        {
            _freezeTimeLeft -= timeElapsed;
        } 
        else if (_currentState == MonsterAction.Attacking)
        {
            if (_animationPlayer.IsFinished)
            {
                _freezeTimeLeft = FreezeAfterBiteSeconds;
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
            _facing = direction.X switch
            {
                < 0 => EntityFacing.Left,
                > 0 => EntityFacing.Right,
                _ => _facing
            };
        }

        _animationPlayer.Update(timeElapsed);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(spriteBatch, _position, _facing, 1f);
    }

    private Vector2 GetDirection()
    {
        var direction = _targetPosition.Position - _position;

        if (direction.Length() < BiteRadiusPixels)
            return Vector2.Zero;

        if (direction != Vector2.Zero)
            direction.Normalize();
        
        return direction;
    }

    private void SetState(MonsterAction action)
    {
        _currentState = action;
        _animationPlayer.Play(_animationSet.Animations[action]);
    }
}