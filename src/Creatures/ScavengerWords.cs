using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class ScavengerWords : Wordify<Scavenger>
    {
        public static FLabel[] Init(ScavengerGraphics scavGraf, CreatureTemplate.Type type)
        {
            var name = Unpascal(type);
            return [
                new(Font, name)
                {
                    scale = (scavGraf.scavenger.bodyChunks[0].rad + scavGraf.scavenger.bodyChunks[1].rad) * 3f / TextWidth(name),
                    color = scavGraf.bodyColor.rgb
                },
                new(Font, "Head")
                {
                    color = scavGraf.headColor.rgb
                },
                new(Font, "Mask")
                {
                    scale = 17.5f / FontSize / TextWidth("Mask")
                }
            ];
        }

        public static void Draw(ScavengerGraphics scavGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body chunk guide: 0 -> body, 1 -> hips, 2 -> head
            var chunks = scavGraf.scavenger.bodyChunks;
            labels[0].SetPosition(GetPos(chunks[0], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(chunks[1], chunks[0], timeStacker);

            var eyesPop = Mathf.Lerp(scavGraf.lastEyesPop, scavGraf.eyesPop, timeStacker);
            labels[1].SetPosition(GetPos(chunks[2], timeStacker) - camPos);
            labels[1].rotation = FixRotation(AngleBtwn(GetPos(chunks[2], timeStacker), scavGraf.lookPoint)) - 90f;
            labels[1].scale = chunks[2].rad * Mathf.Lerp(4f, 8f, eyesPop) / TextWidth("Head");
            labels[1].color = Color.Lerp(scavGraf.headColor.rgb, scavGraf.eyeColor.rgb, eyesPop);

            var hasMask = scavGraf.maskGfx != null && !scavGraf.scavenger.readyToReleaseMask;
            labels[2].isVisible = hasMask;
            if (hasMask)
            {
                labels[2].SetPosition(labels[1].GetPosition());
                labels[2].rotation = sLeaser.sprites[scavGraf.MaskSprite].rotation + 90f;
            }
        }
    }
}
