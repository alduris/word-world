using System;
using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class GooieDuckWords : Wordify<GooieDuck>
    {
        public static FLabel[] Init(GooieDuck gooieduck)
        {
            var labels = POWords.Init(gooieduck, $"Gooie{Environment.NewLine}duck");
            labels[0].color = gooieduck.CoreColor;
            return labels;
        }

        public static void Draw(GooieDuck gooieduck, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(gooieduck, labels, timeStacker, camPos);
            labels[0].scale = gooieduck.firstChunk.rad * 3f / WordUtil.FontSize + Mathf.Sin(gooieduck.PulserA) / 6f;
        }
    }
}
