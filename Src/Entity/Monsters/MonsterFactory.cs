using System.Collections.Generic;
using HexHunt.Animations;
using HexHunt.Content;
using HexHunt.Entity.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HexHunt.Entity.Monsters;

public class MonsterFactory(ContentManager content, float frameDuration)
{
    private readonly Dictionary<MonsterDefinition, AnimationSet<CharacterAction>> _monsterAnimationsCache = new();
    
    public Monster Create(MonsterDefinition monsterDefinition, Vector2 position, IEntity entity)
    {
        if (!_monsterAnimationsCache.TryGetValue(monsterDefinition, out var animationSet))
        {
            animationSet = AnimationLoader.LoadAnimations(content, monsterDefinition.ArtSet, frameDuration);
            _monsterAnimationsCache.Add(monsterDefinition, animationSet);
        }
        return new Monster(monsterDefinition, position, animationSet, entity);
    }
}