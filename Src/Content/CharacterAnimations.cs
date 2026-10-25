using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HexHunt.Content;

public record CharacterAnimations<TAction>(IReadOnlyDictionary<TAction, Animation.Animation> Animations)
    where TAction : struct, Enum;
    
public static class AnimationLoader
{
    public static CharacterAnimations<TAction> LoadCharacterAnimations<TAction>(
        ContentManager content,
        CharacterArt<TAction> art,
        float frameDuration
    ) where TAction : struct, Enum
    {
        var characterAnimations = new Dictionary<TAction, Animation.Animation>();
        foreach (var (action, sheet) in art.Sheets)
        {
            characterAnimations.Add(
                action,
                LoadAnimation(content, sheet, frameDuration)
            );
        }
        return new CharacterAnimations<TAction>(characterAnimations);
    }

    public static Animation.Animation LoadAnimation(ContentManager content, SheetArt sheet, float frameDuration)
    {
        return new Animation.Animation(
            texture: content.Load<Texture2D>(sheet.Path),
            frameWidth: sheet.FrameWidth,
            frameHeight: sheet.FrameHeight,
            origin: sheet.Origin,
            frameDuration: frameDuration,
            isLooping: sheet.IsLooping
        );
    }
}