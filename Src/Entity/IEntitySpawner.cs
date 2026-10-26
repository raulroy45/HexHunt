namespace HexHunt.Entity;

public interface IEntitySpawner
{
    void Spawn(IEntity entity);
    
    void Despawn(IEntity entity);
}