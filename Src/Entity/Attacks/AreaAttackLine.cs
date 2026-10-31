using System;
using System.Collections.Generic;
using HexHunt.Animations;
using HexHunt.Entity.Combat;
using HexHunt.Entity.Lifetime;
using HexHunt.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Entity.Attacks;

public class AreaAttackLine : IEntity
{
    private readonly Animation _areaAttackAnimation;
    private readonly float _bufferBetweenAttackSeconds;
    private readonly Damage _damage;
    private readonly Vector2 _direction;
    private readonly float _distanceBetweenAttacksPixels;

    private readonly IEntitySpawner _entitySpawner;
    private readonly HashSet<IHittable> _hasHit;
    private readonly bool _shouldDespawnAfterAnimation;
    private readonly List<AreaAttack> _spawnedAttacks = [];
    private readonly Team _team;
    private readonly int _totalAttacks;
    private float _totalTimeElapsed;

    public AreaAttackLine(
        IEntitySpawner entitySpawner,
        Vector2 position,
        Vector2 direction,
        Animation areaAttackAnimation,
        int totalAttacks,
        Damage damage,
        Team team,
        float bufferBetweenAttackSeconds = 0.1f,
        float distanceBetweenAttacksPixels = 25f,
        bool shouldDespawnAfterAnimation = true
    )
    {
        _entitySpawner = entitySpawner;
        Position = position;

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
        _hasHit = new HashSet<IHittable>();
        _damage = damage;
        _team = team;
    }

    public ILifetime Lifetime { get; }
    public Vector2 Position { get; }

    public void Update(float dt)
    {
        Lifetime.Update(dt);

        if (HaveAllAttacksSpawned()) return;

        _totalTimeElapsed += dt;

        if (_totalTimeElapsed / _bufferBetweenAttackSeconds >= _spawnedAttacks.Count)
        {
            var entity = CreateAreaAttackEntity();
            _entitySpawner.Spawn(entity);
            _spawnedAttacks.Add(entity);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    private bool HaveAllAttacksSpawned()
    {
        return _spawnedAttacks.Count >= _totalAttacks;
    }

    private AreaAttack CreateAreaAttackEntity()
    {
        return new AreaAttack(
            Position + _direction * _distanceBetweenAttacksPixels * (_spawnedAttacks.Count + 1),
            _direction,
            _areaAttackAnimation,
            _damage,
            _team,
            _shouldDespawnAfterAnimation,
            _hasHit
        );
    }

    private class AttackLineLifetime(Func<bool> allAttacksFinished) : ILifetime
    {
        public bool IsExpired { get; private set; }

        public void Update(float elapsedTime)
        {
            if (allAttacksFinished()) IsExpired = true;
        }
    }
}