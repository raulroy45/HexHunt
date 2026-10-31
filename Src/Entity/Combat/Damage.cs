namespace HexHunt.Entity.Combat;

public class Damage
{
    public int DamageAmount { get; init; }

    public Knockback? Knockback { get; init; } = null;
}