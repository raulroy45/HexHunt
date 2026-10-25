using System.Collections.Generic;
using HexHunt.Assets;
using HexHunt.Assets.Characters;
using HexHunt.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexHunt;

public class HexHunt : Game, IEntitySpawner
{
    private const float FrameDuration = 0.1f;

    private List<IEntity> _entities = [];
    private List<IEntity> _queuedSpawnEntities = [];
    private List<IEntity> _queuedDespawnEntities = [];

    private GraphicsDeviceManager _graphics;

    private Witch _player;
    private KeyboardState _previousKeyboardState;
    private SpriteBatch _spriteBatch;

    public HexHunt()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _player = new Witch(
            new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f),
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
        
        _entities.Add(_player);
    }

    protected override void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var keyboardState = Keyboard.GetState();

        var characterInput = new WitchInput();

        if (WasJustPressed(keyboardState, _previousKeyboardState, Keys.J))
            characterInput.AttackButtonPressed = true;

        characterInput.Direction = GetDirection(keyboardState);

        _player.SetInput(characterInput);

        HandleQueuedEntities();
        foreach (var entity in _entities)
            entity.Update(elapsedTime);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);

        _previousKeyboardState = keyboardState;
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

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var entity in _entities)
            entity.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
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

    public void Spawn(IEntity entity)
    {
        _queuedSpawnEntities.Add(entity);
    }

    public void Despawn(IEntity entity)
    {
        _queuedDespawnEntities.Add(entity);
    }
}