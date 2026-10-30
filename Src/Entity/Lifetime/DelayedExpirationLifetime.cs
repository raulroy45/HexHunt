using System;

namespace HexHunt.Entity.Lifetime;

public class DelayedExpirationLifetime(
    Func<bool> isFinalAnimationFinished,
    float delayExpirationSeconds = 0.2f
) : ILifetime
{
    public bool IsExpired { get; private set; } = false;
    private float _delayExpirationSecondsLeft = delayExpirationSeconds;

    public void Update(float elapsedTime)
    {
        if (!isFinalAnimationFinished()) return;
        _delayExpirationSecondsLeft -= elapsedTime;
        if (_delayExpirationSecondsLeft <= 0) IsExpired = true;
    }
}