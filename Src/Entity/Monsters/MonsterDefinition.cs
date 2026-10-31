using HexHunt.Content;
using HexHunt.Entity.Characters;
using Microsoft.Xna.Framework;

namespace HexHunt.Entity.Monsters;

public record MonsterDefinition(
    ArtSet<CharacterAction> ArtSet,
    float Speed,
    float Scale,
    Vector2 DamageSourceOffset,
    int AttackRadiusPixels,
    int AttackFrame,
    float FreezeSeconds, 
    int HitPoints,
    int HitboxWidth,
    int HitboxHeight,
    float DelayDeathSeconds,
    int BiteBoxWidth,
    int BiteBoxHeight,
    int DamageAmount
);