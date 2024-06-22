using System;
using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class GooieDuckWords() : POWordify<GooieDuck>($"Gooie{Environment.NewLine}duck")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            Label.scale = Drawable.firstChunk.rad * 3f / WordUtil.FontSize + Mathf.Sin(Drawable.PulserA) / 6f;
            Label.color = Drawable.blink > 1 ? Drawable.blinkColor : Drawable.CoreColor;
        }
    }
}
