using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class GhostWords : Wordify<Ghost>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new(Font, "Echo")
            {
                scale = 8f * Drawable.scale,
                color = Drawable.goldColor
            };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var startPos = Vector2.Lerp(Drawable.spine[0].lastPos, Drawable.spine[0].pos, timeStacker);
            var endPos = Vector2.Lerp(Drawable.spine[Drawable.spine.Length - 1].lastPos, Drawable.spine[Drawable.spine.Length - 1].pos, timeStacker);
            label.SetPosition(AvgVectors(startPos, endPos) - camPos);
            label.rotation = AngleBtwn(startPos, endPos);
            sLeaser.sprites[Drawable.DistortionSprite].isVisible = true;
            sLeaser.sprites[Drawable.LightSprite].isVisible = true;
        }

        protected override void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser, FContainer container)
        {
            base.AddToContainer(rCam, sLeaser, rCam.ReturnFContainer("Items"));
        }
    }
}
