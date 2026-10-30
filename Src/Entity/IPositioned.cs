using Microsoft.Xna.Framework;

namespace HexHunt.Entity;

public interface IPositioned
{
    public Vector2 Position { get; }
}