using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class OverseerWords : CreatureWordify<OverseerGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // TODO: is there any way I can make it go along the curve?
            labels.Add(new FLabel(Font, Unpascal(Type)));
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].isVisible = Drawable.overseer.room != null;

            // Fixes a crash moving rooms in safari, which was where I discovered it. No clue if it existed in normal gameplay, didn't happen in arena.
            if (Drawable.overseer.room != null)
            {
                labels[0].SetPosition(AvgVectors(Drawable.DrawPosOfSegment(0f, timeStacker), Drawable.DrawPosOfSegment(1f, timeStacker)) - camPos);
                labels[0].rotation = AngleBtwn(Drawable.DrawPosOfSegment(0f, timeStacker), Drawable.DrawPosOfSegment(1f, timeStacker)) + 90f;
                labels[0].scale = (Drawable.DrawPosOfSegment(0f, timeStacker) - Drawable.DrawPosOfSegment(1f, timeStacker)).magnitude / TextWidth(labels[0].text);
                labels[0].color = Drawable.MainColor; // fixes arena mode inconsistency
            }
        }
    }
}
