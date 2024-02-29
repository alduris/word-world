using System;
using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public static class GlowWeedWords
    {
        public static FLabel[] Init(GlowWeed obj) => POWords.Init(obj, $"Glow{Environment.NewLine}Weed");

        public static void Draw(GlowWeed obj, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(obj, labels, timeStacker, camPos);
    }
}
