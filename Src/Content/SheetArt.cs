using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace HexHunt.Content;

public record SheetArt(
    string Path,
    int FrameWidth,
    int FrameHeight,
    Vector2 Origin,
    bool IsLooping
);

public record CharacterArt<TAction> where TAction : struct, Enum 
{
    public IReadOnlyDictionary<TAction, SheetArt> Sheets { get; }

    public CharacterArt(IReadOnlyDictionary<TAction, SheetArt> sheets)
    {
        // Validate that all actions in TAction enum have been mapped to a sheet
        var allActions = Enum.GetValues<TAction>();
        var missingSheets = allActions.Where(x => !sheets.ContainsKey(x)).ToArray();
        if (missingSheets.Length != 0)
            throw new ArgumentException(
                $"Missing sheets for {typeof(TAction).Name} actions: {string.Join(", ", missingSheets)}");
        
        Sheets = sheets;
    }

}