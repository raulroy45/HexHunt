using System;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity;

public class Knockback
{
    public Vector2 Direction { get; init; }
    public float Distance { get; init; }

    public float Duration { get; init; } = 3f;
}

public class KnockbackMotion
{
    private Knockback? _knockback;
    private float _timeLeft;

    public void Start(Knockback knockback)
    {
        _knockback =  knockback;
        _timeLeft = knockback.Duration;
    }

    public Vector2 Update(float timeElapsed)
    {
        if (_timeLeft <= 0 || _knockback is null) return Vector2.Zero;
        
        var step = Math.Min(timeElapsed, _timeLeft);
        var positionDrift = _knockback.Direction * _knockback.Distance * step / _knockback.Duration;
        _timeLeft -= step;
        return positionDrift;
    }

}