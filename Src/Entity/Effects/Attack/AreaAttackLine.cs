using System;
using HexHunt.Animations;
using HexHunt.Entity.Lifetime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Effects.Attack;

public class AreaAttackLine : IEntity
{
    public ILifetime Lifetime { get; }
    
    private readonly IEntitySpawner _entitySpawner;
    private readonly int _totalAttacks;
    private readonly float _bufferBetweenAttackSeconds;
    private readonly float _distanceBetweenAttacksPixels;
    private readonly bool _shouldDespawnAfterAnimation;
    private readonly Animation _areaAttackAnimation;
    private readonly Vector2 _direction;
    private readonly Vector2 _position;
    private float _totalTimeElapsed = 0f;
    private int _spawnedAttacks = 0;

    public AreaAttackLine(
        IEntitySpawner entitySpawner,
        Vector2 position,
        Vector2 direction,
        Animation areaAttackAnimation,
        int totalAttacks,
        float bufferBetweenAttackSeconds = 0.1f,
        float distanceBetweenAttacksPixels = 25f,
        bool shouldDespawnAfterAnimation = true
    )
    {
        _entitySpawner = entitySpawner;
        _position = position;
        
        if (direction != Vector2.Zero) direction.Normalize();
        _direction = direction;
        
        _areaAttackAnimation = areaAttackAnimation;
        _totalAttacks = totalAttacks > 0
            ? totalAttacks
            : throw new ArgumentException("Total number of attacks cannot be less than or equal to 0");
        
        _bufferBetweenAttackSeconds = bufferBetweenAttackSeconds;
        _distanceBetweenAttacksPixels = distanceBetweenAttacksPixels;
        _shouldDespawnAfterAnimation = shouldDespawnAfterAnimation;
        
        Lifetime = new AttackLineLifetime(HaveAllAttacksSpawned);
    }
    
    public void Update(float timeElapsed)
    {
        Lifetime.Update(timeElapsed);

        if (HaveAllAttacksSpawned()) return;
        
        _totalTimeElapsed += timeElapsed;

        if (_totalTimeElapsed / _bufferBetweenAttackSeconds >= _spawnedAttacks)
        {
            _entitySpawner.Spawn(CreateAreaAttack());
            _spawnedAttacks++;
        }
    }

    private bool HaveAllAttacksSpawned()
    {
        return _spawnedAttacks >= _totalAttacks;
    }

    public void Draw(SpriteBatch spriteBatch) { }

    private AreaAttack CreateAreaAttack()
    {
        return new AreaAttack(
            _position + (_direction * _distanceBetweenAttacksPixels * (_spawnedAttacks + 1)),
            _direction,
            _areaAttackAnimation,
            shouldDespawnAfterAnimation: _shouldDespawnAfterAnimation
        );
    }

    private class AttackLineLifetime(Func<bool> allAttacksFinished) : ILifetime
    {
        public bool IsExpired { get; private set; } = false;
        public void Update(float elapsedTime)
        {
            if (allAttacksFinished()) IsExpired = true;
        }
    }
}