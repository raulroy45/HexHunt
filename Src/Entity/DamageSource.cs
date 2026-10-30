using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity;

public class DamageSource
{
    public readonly Hitbox.Hitbox Hitbox;
    public bool IsActive { get; private set; }
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
    private HashSet<IHittable> _hasHit;
    private readonly Damage _damage;

    public DamageSource(
        Vector2 position, 
        float scale, 
        float width, 
        float height, 
        Damage damage,
        bool isActive = false,
        HashSet<IHittable>? hasHit = null
    )
    {
        Hitbox = new Hitbox.Hitbox(position, scale, width, height);
        _hasHit = hasHit ?? new HashSet<IHittable>();
        _damage = damage;
        IsActive = isActive;
    }

    public void Update(Vector2 position)
    {
        Hitbox.Update(position);
    }

    public bool TryHit(IHittable hittable)
    {
        if (!IsActive)  return false;
        if (_hasHit.Contains(hittable)) return false;
        if (!Hitbox.IsHit(hittable.Hitbox)) return false;
        if (!hittable.OnHit(_damage)) return false;
        
        _hasHit.Add(hittable);
        return true;
    }
}