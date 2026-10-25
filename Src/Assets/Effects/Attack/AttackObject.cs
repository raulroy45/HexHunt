using HexHunt.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Assets.Effects.Attack;

public class AttackObject : IEntity
{
    private readonly AnimationPlayer _animationPlayer;
    
    private readonly Vector2 _position;
    private readonly Vector2 _direction;

    public AttackObject(
        Vector2 position,
        Vector2 direction,
        Animation.Animation attackObjectAnimation
    )
    {
        _position = position;
        _direction = direction;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(attackObjectAnimation);
    }

    public void Update(float elapsedTime)
    {
        _animationPlayer.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(
            spriteBatch,
            _position,
            GetFacingFromDirection(_direction),
            1.5f
        );
    }

    private static EntityFacing GetFacingFromDirection(Vector2 direction)
    {
        return direction.X >= 0 ? EntityFacing.Right : EntityFacing.Left;
    }
}