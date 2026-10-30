using Microsoft.Xna.Framework;

namespace HexHunt.Entity.Hitbox;

public class Hitbox
{
    public Rectangle HitboxRectangle { get; private set; }
    private readonly float _scaledWidth;
    private readonly float _scaledHeight;

    public Hitbox(Vector2 position, float scale, float width, float height)
    {
        _scaledWidth = width * scale;
        _scaledHeight = height * scale;
        
        Update(position);
    }

    public void Update(Vector2 position)
    {
        HitboxRectangle = new Rectangle(
            (int)(position.X - _scaledWidth / 2),
            (int)(position.Y - _scaledHeight / 2),
            (int)_scaledWidth,
            (int)_scaledHeight
        );
    }

    public bool IsHit(Hitbox other)
    {
        return this.HitboxRectangle.Intersects(other.HitboxRectangle);
    }
}