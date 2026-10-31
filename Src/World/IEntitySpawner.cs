using HexHunt.Entity;

namespace HexHunt.World;

public interface IEntitySpawner
{
    void Spawn(IEntity entity);

    void Despawn(IEntity entity);
}