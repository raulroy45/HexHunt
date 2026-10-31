namespace HexHunt.Entity.Combat;

public interface IHittable
{
    public Hitbox Hitbox { get; }
    public Team Team { get; }
    public bool OnHit(Damage damage);
}