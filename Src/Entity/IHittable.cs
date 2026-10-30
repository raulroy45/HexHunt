namespace HexHunt.Entity;

public interface IHittable
{
    public Hitbox.Hitbox Hitbox { get; }
    public bool OnHit(Damage damage);
}