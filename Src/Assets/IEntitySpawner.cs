namespace HexHunt.Assets;

public interface IEntitySpawner
{
    void Spawn(IEntity entity);
    
    void Despawn(IEntity entity);
}