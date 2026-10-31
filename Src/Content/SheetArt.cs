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