using System.Collections.Generic;
using HexHunt.Assets.Characters;
using Microsoft.Xna.Framework;

namespace HexHunt.Content;

public static class ArtCatalogue
{
    private static readonly SheetArt BlueWitchBase = new(
        Path: "",
        FrameWidth: 111,
        FrameHeight: 48,
        Origin: new Vector2(26, 42),
        IsLooping: true
    );

    public static readonly CharacterArt<WitchAction> BlueWitch =
        new(new Dictionary<WitchAction, SheetArt>
        {
            { WitchAction.Idling, BlueWitchBase with { Path = "Characters/Blue Witch/B_witch_idle" } },
            { WitchAction.Moving, BlueWitchBase with { Path = "Characters/Blue Witch/B_witch_run" } },
            {
                WitchAction.Attacking,
                BlueWitchBase with { Path = "Characters/Blue Witch/B_witch_attack", IsLooping = false }
            }
        });

    public static readonly SheetArt AttackEffectIceSpike =
        new(
            Path: "Effects/Magic/B_witch_ice_spike",
            FrameWidth: 19,
            FrameHeight: 38,
            Origin: new Vector2(4, 33),
            IsLooping: false
        );
}