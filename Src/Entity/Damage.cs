namespace HexHunt.Entity;

public class Damage
{
    public int DamageAmount { get; init; }
    
    public Knockback? Knockback { get; init; } = null;
}