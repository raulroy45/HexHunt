using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Assets;

public interface IEntity
{
    public void Update(float timeElapsed);
    
    public void Draw(SpriteBatch spriteBatch);
}