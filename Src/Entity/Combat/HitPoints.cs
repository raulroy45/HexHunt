namespace HexHunt.Entity.Combat;

public class HitPoints(int totalHitPoints)
{
    private int _remainingHitPoints = totalHitPoints;

    public bool IsDead => _remainingHitPoints <= 0;

    public void TakeDamage(int damage)
    {
        if (_remainingHitPoints <= damage) _remainingHitPoints = 0;
        else _remainingHitPoints -= damage;
    }

    public void Heal(int heal)
    {
        if (_remainingHitPoints + heal >= totalHitPoints) _remainingHitPoints = totalHitPoints;
        else _remainingHitPoints += heal;
    }
}