using System.Collections.Generic;
using HexHunt.Entity.Characters;
using HexHunt.Entity.Monsters;
using Microsoft.Xna.Framework;

namespace HexHunt.Content;

public static class ArtCatalogue
{
    private static readonly SheetArt BlueWitchBase = new(
        "",
        111,
        48,
        new Vector2(26, 42),
        true
    );

    public static readonly ArtSet<WitchAction> BlueWitch = new(
        new Dictionary<WitchAction, SheetArt>
        {
            { WitchAction.Idling, BlueWitchBase with { Path = "Characters/Blue Witch/B_witch_idle" } },
            { WitchAction.Moving, BlueWitchBase with { Path = "Characters/Blue Witch/B_witch_run" } },
            {
                WitchAction.Attacking,
                BlueWitchBase with { Path = "Characters/Blue Witch/B_witch_attack", IsLooping = false }
            }
        }
    );

    public static readonly ArtSet<WitchEffect> WitchEffects =
        new(new Dictionary<WitchEffect, SheetArt>
        {
            {
                WitchEffect.IceSpike, new SheetArt(
                    "Effects/Magic/B_witch_ice_spike",
                    19,
                    38,
                    new Vector2(4, 33),
                    false
                )
            }
        }
    );
    
    private static readonly SheetArt RatBase = new(
        "",
        70,
        70,
        new Vector2(35, 44),
        true
    );

    public static readonly ArtSet<MonsterAction> Rat = new(
        new Dictionary<MonsterAction, SheetArt>
        {
            { MonsterAction.Idling, RatBase with { Path = "Monsters/Rat/idle" } },
            { MonsterAction.Running, RatBase with { Path = "Monsters/Rat/run" } },
            { MonsterAction.Attacking, RatBase with { Path = "Monsters/Rat/attack_bite", IsLooping = false} }
        }
    );
}