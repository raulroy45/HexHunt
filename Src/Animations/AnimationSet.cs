using System;
using System.Collections.Generic;

namespace HexHunt.Animations;

public record AnimationSet<TKey>(IReadOnlyDictionary<TKey, Animation> Animations)
    where TKey : struct, Enum;