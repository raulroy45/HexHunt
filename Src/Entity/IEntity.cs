using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity;

public interface IEntity : IPositioned
{
    public ILifetime Lifetime { get; }
    
    public void Update(float timeElapsed);

    public void Draw(SpriteBatch spriteBatch);
}