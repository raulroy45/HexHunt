namespace HexHunt.Entity;

public interface IHittable
{
    public Hitbox.Hitbox Hitbox { get; }
    public Team Team { get; }
    public bool OnHit(Damage damage);
}