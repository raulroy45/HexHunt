using System;
using System.Collections.Generic;
using System.Linq;

namespace HexHunt.Content;

public record ArtSet<TKey> where TKey : struct, Enum
{
    public ArtSet(IReadOnlyDictionary<TKey, SheetArt> sheets)
    {
        // Validate that all actions in TAction enum have been mapped to a sheet
        var allActions = Enum.GetValues<TKey>();
        var missingSheets = allActions.Where(x => !sheets.ContainsKey(x)).ToArray();
        if (missingSheets.Length != 0)
            throw new ArgumentException(
                $"Missing sheets for {typeof(TKey).Name} members: {string.Join(", ", missingSheets)}");

        Sheets = sheets;
    }

    public IReadOnlyDictionary<TKey, SheetArt> Sheets { get; }
}