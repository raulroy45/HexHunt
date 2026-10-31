using System;
using System.Collections.Generic;
using System.Linq;
using HexHunt.Entity;
using HexHunt.Content;
using HexHunt.Entity.Characters;
using HexHunt.Entity.Monsters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexHunt;

public class HexHunt : Game, IEntitySpawner
{
    private const float WorldScale = 2f;
    private const float FrameDuration = 0.1f;
    private float _boundaryMinX;
    private float _boundaryMaxX;
    private float _boundaryMinY;
    private float _boundaryMaxY;

    private readonly List<IEntity> _entities = [];
    private readonly List<IEntity> _queuedSpawnEntities = [];
    private readonly List<IEntity> _queuedDespawnEntities = [];

    private GraphicsDeviceManager _graphics;

    private Witch _player;
    private KeyboardState _previousKeyboardState;
    private SpriteBatch _spriteBatch;
    
    // debugging
    private Texture2D _debugBox;
    private bool _isDebugMode = false;

    public HexHunt()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _boundaryMinX = GraphicsDevice.Viewport.Bounds.Left / WorldScale;
        _boundaryMaxX = GraphicsDevice.Viewport.Bounds.Right / WorldScale;
        _boundaryMinY = GraphicsDevice.Viewport.Bounds.Top / WorldScale;
        _boundaryMaxY = GraphicsDevice.Viewport.Bounds.Bottom / WorldScale;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _player = new Witch(
            new Vector2(
                GraphicsDevice.Viewport.Width / (2f * WorldScale),
                GraphicsDevice.Viewport.Height / (2f * WorldScale)),
            AnimationLoader.LoadAnimations(
                Content,
                ArtCatalogue.BlueWitch,
                FrameDuration
            ),
            AnimationLoader.LoadAnimations(
                Content,
                ArtCatalogue.WitchEffects,
                FrameDuration
            ),
            this // Give reference to this Game's IEntitySpawner
        );

        SpawnMonsters(
            AnimationLoader.LoadAnimations(
                Content,
                ArtCatalogue.Rat,
                FrameDuration
            )
        );
        
        _debugBox = new Texture2D(GraphicsDevice, 1, 1);
        _debugBox.SetData([Color.White]);
        
        _entities.Add(_player);
    }

    protected override void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var keyboardState = Keyboard.GetState();

        var characterInput = new WitchInput();

        if (WasJustPressed(keyboardState, _previousKeyboardState, Keys.J))
            characterInput.AttackButtonPressed = true;
        
        if (WasJustPressed(keyboardState, _previousKeyboardState, Keys.F1))
            _isDebugMode = !_isDebugMode;

        characterInput.Direction = GetDirection(keyboardState);

        _player.SetInput(characterInput);

        HandleQueuedEntities();
        foreach (var entity in _entities)
        {
            entity.Update(elapsedTime);
            
            // Despawn entity in the next update
            if (entity.Lifetime.IsExpired) Despawn(entity);
        }
        HandleCollisions();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Escape) || _player.Lifetime.IsExpired)
            Exit();

        base.Update(gameTime);
        _previousKeyboardState = keyboardState;
    }

    private void HandleCollisions()
    {
        var damageSources = _entities.OfType<IDamageSource>();
        var hittables = _entities.OfType<IHittable>().ToList();
        
        foreach (var entity in damageSources)
            foreach (var hittable in hittables)
                entity.DamageSource.TryHit(hittable);
    }

    private void HandleQueuedEntities()
    {
        _entities.RemoveAll(e => _queuedDespawnEntities.Contains(e));
        _entities.AddRange(_queuedSpawnEntities);
        
        _queuedDespawnEntities.Clear();
        _queuedSpawnEntities.Clear();
    }

    private static bool WasJustPressed(KeyboardState currKeyboardState, KeyboardState prevKeyboardState, Keys key)
    {
        return currKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: Matrix.CreateScale(WorldScale)
        );
        foreach (var entity in _entities.OrderBy(e => e.Position.Y))
            entity.Draw(_spriteBatch);
        if (_isDebugMode) DrawDebuggingElements();
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawDebuggingElements()
    {
        var hittables = _entities.OfType<IHittable>();
        var activeDamageSources = _entities.OfType<IDamageSource>().Where(x => x.DamageSource.IsActive);
        foreach (var entity in hittables)
            _spriteBatch.Draw(_debugBox, entity.Hitbox.HitboxRectangle, Color.Red * 0.4f);
        foreach (var entity in activeDamageSources)
            _spriteBatch.Draw(_debugBox, entity.DamageSource.Hitbox.HitboxRectangle, Color.Yellow * 0.4f);
    }

    private static Vector2 GetDirection(KeyboardState keyboardState)
    {
        var direction = Vector2.Zero;
        if (keyboardState.IsKeyDown(Keys.W))
            direction.Y -= 1f;

        if (keyboardState.IsKeyDown(Keys.S))
            direction.Y += 1f;

        if (keyboardState.IsKeyDown(Keys.A))
            direction.X -= 1f;

        if (keyboardState.IsKeyDown(Keys.D))
            direction.X += 1f;

        if (direction != Vector2.Zero)
            direction.Normalize();

        return direction;
    }
    
    private void SpawnMonsters(AnimationSet<MonsterAction> monsterAnimation)
    {
        var rand = new Random();
        var randomPosition = new Vector2(
            _boundaryMinX + (float)rand.NextDouble() * (_boundaryMaxX - _boundaryMinX),
            _boundaryMinY + (float)rand.NextDouble() * (_boundaryMaxY - _boundaryMinY)
        );
        Spawn(new Rat(randomPosition, monsterAnimation, _player));
    }

    public void Spawn(IEntity entity)
    {
        _queuedSpawnEntities.Add(entity);
    }

    public void Despawn(IEntity entity)
    {
        _queuedDespawnEntities.Add(entity);
    }
}