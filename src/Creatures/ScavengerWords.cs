using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class ScavengerWords : CreatureWordify<ScavengerGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var name = Unpascal(Type);
            labels.AddRange([
                new(Font, name)
                {
                    scale = (Drawable.scavenger.bodyChunks[0].rad + Drawable.scavenger.bodyChunks[1].rad) * 3f / TextWidth(name),
                    color = Drawable.bodyColor.rgb
                },
                new(Font, "Head")
                {
                    color = Drawable.headColor.rgb
                },
                new(Font, "Mask")
                {
                    scale = 17.5f / FontSize / TextWidth("Mask")
                }
            ]);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body chunk guide: 0 -> body, 1 -> hips, 2 -> head
            var chunks = Drawable.scavenger.bodyChunks;
            labels[0].SetPosition(GetPos(chunks[0], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(chunks[1], chunks[0], timeStacker);

            var eyesPop = Mathf.Lerp(Drawable.lastEyesPop, Drawable.eyesPop, timeStacker);
            labels[1].SetPosition(GetPos(chunks[2], timeStacker) - camPos);
            labels[1].rotation = FixRotation(AngleBtwn(GetPos(chunks[2], timeStacker), Drawable.lookPoint)) - 90f;
            labels[1].scale = chunks[2].rad * Mathf.Lerp(4f, 8f, eyesPop) / TextWidth("Head");
            labels[1].color = Color.Lerp(Drawable.headColor.rgb, Drawable.eyeColor.rgb, eyesPop);

            var hasMask = Drawable.maskGfx != null && !Drawable.scavenger.readyToReleaseMask;
            labels[2].isVisible = hasMask;
            if (hasMask)
            {
                labels[2].SetPosition(labels[1].GetPosition());
                labels[2].rotation = sLeaser.sprites[Drawable.MaskSprite].rotation + 90f;
            }
        }
    }
}
