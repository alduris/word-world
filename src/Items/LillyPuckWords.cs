using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class LillyPuckWords : Wordify<LillyPuck>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "Lillypuck")
            {
                scale = Drawable.firstChunk.rad * 3f / FontSize,
                // scale = sLeaser.sprites[0].element.sourcePixelSize.y / TextWidth("Lillypuck"),
                color = Drawable.flowerColor
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            label.rotation = FixRotation(sLeaser.sprites[0].rotation) - 90f;
        }
    }
}
