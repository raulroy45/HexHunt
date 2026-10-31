using System;

namespace HexHunt.Entity.Lifetime;

public class DelayedExpirationLifetime(
    Func<bool> isFinalAnimationFinished,
    float delayExpirationSeconds = 0.2f
) : ILifetime
{
    private float _delayExpirationSecondsLeft = delayExpirationSeconds;
    public bool IsExpired { get; private set; }

    public void Update(float elapsedTime)
    {
        if (!isFinalAnimationFinished()) return;
        _delayExpirationSecondsLeft -= elapsedTime;
        if (_delayExpirationSecondsLeft <= 0) IsExpired = true;
    }
}