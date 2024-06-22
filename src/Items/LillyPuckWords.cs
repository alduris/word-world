using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class LillyPuckWords : Wordify<LillyPuck>
    {
        public static FLabel[] Init(LillyPuck lillyPuck)
        {
            var label = new FLabel(Font, "Lillypuck")
            {
                scale = lillyPuck.firstChunk.rad * 3f / FontSize,
                // scale = sLeaser.sprites[0].element.sourcePixelSize.y / TextWidth("Lillypuck"),
                color = lillyPuck.flowerColor
            };
            return [label];
        }

        public static void Draw(LillyPuck lillyPuck, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(lillyPuck.firstChunk, timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[0].rotation) - 90f;
        }
    }
}
