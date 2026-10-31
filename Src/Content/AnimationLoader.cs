using System;
using System.Collections.Generic;
using HexHunt.Animations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Content;

public static class AnimationLoader
{
    public static AnimationSet<TKey> LoadAnimations<TKey>(
        ContentManager content,
        ArtSet<TKey> artSet,
        float frameDuration
    ) where TKey : struct, Enum
    {
        var animationSet = new Dictionary<TKey, Animation>();
        foreach (var (action, sheet) in artSet.Sheets)
            animationSet.Add(
                action,
                LoadAnimation(content, sheet, frameDuration)
            );
        return new AnimationSet<TKey>(animationSet);
    }

    public static Animation LoadAnimation(ContentManager content, SheetArt sheet, float frameDuration)
    {
        return new Animation(
            content.Load<Texture2D>(sheet.Path),
            sheet.FrameWidth,
            sheet.FrameHeight,
            origin: sheet.Origin,
            frameDuration: frameDuration,
            isLooping: sheet.IsLooping
        );
    }
}