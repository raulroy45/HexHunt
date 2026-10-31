using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity;

public class DamageSource(
    Vector2 position,
    float scale,
    float width,
    float height,
    Damage damage,
    Team team,
    bool isActive = false,
    HashSet<IHittable>? hasHit = null)
{
    public readonly Hitbox.Hitbox Hitbox = new(position, scale, width, height);
    public Team Team { get; } = team;
    public bool IsActive { get; private set; } = isActive;
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
    private readonly HashSet<IHittable> _hasHit = hasHit ?? [];

    public void Update(Vector2 position)
    {
        Hitbox.Update(position);
    }

    public bool TryHit(IHittable hittable)
    {
        if (!IsActive)  return false;
        if (hittable.Team == Team) return false;
        if (_hasHit.Contains(hittable)) return false;
        if (!Hitbox.IsHit(hittable.Hitbox)) return false;
        if (!hittable.OnHit(damage)) return false;
        
        _hasHit.Add(hittable);
        return true;
    }

    public void ClearEarlierHits()
    {
        _hasHit.Clear();
    }
}