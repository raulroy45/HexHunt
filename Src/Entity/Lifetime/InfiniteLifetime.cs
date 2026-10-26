namespace HexHunt.Entity.Lifetime;

public class InfiniteLifetime : ILifetime
{
    public bool IsExpired => false;

    public void Update(float elapsedTime) { }
}