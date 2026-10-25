using HexHunt.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Assets.Effects.Attack;

public class AttackObject : IEntity
{
    private readonly AnimationPlayer _animationPlayer;

    private readonly Vector2 _position;
    private readonly EntityFacing _facing;

    public AttackObject(
        Vector2 position,
        EntityFacing facing,
        Animation.Animation attackObjectAnimation
    )
    {
        _position = position;
        _facing = facing;
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
            _facing,
            1.5f
        );
    }
}