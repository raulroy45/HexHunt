using System.Numerics;
using HexHunt.Content;

namespace HexHunt.Entity.Monsters;

public static class MonsterCatalogue
{
    public static readonly MonsterDefinition Rat = new(
        ArtCatalogue.Rat,
        Speed: 40f,
        FreezeSeconds: 0.75f,
        AttackRadiusPixels: 2,
        Scale: 0.5f,
        DamageSourceOffset:new Vector2(25, -9),
        AttackFrame: 8,
        HitPoints: 10,
        HitboxWidth: 30,
        HitboxHeight: 30,
        BiteBoxWidth: 10,
        BiteBoxHeight: 10,
        DelayDeathSeconds: 0.75f,
        DamageAmount: 2
    );
}