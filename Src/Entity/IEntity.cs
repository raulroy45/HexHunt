using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity;

public interface IEntity : IPositioned
{
    public ILifetime Lifetime { get; }

    public void Update(float dt);

    public void Draw(SpriteBatch spriteBatch);
}