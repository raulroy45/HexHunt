namespace HexHunt.Entity.Lifetime;

public interface ILifetime
{
    public bool IsExpired { get; }
    public void Update(float elapsedTime);
}