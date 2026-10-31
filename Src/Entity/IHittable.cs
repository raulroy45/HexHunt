namespace HexHunt.Entity;

public interface IHittable
{
    public Hitbox.Hitbox Hitbox { get; }
    public Team Team { get; init; }
    public bool OnHit(Damage damage);
}