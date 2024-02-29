using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class OverseerWords
    {
        public static FLabel[] Init(OverseerGraphics overseerGraf, CreatureTemplate.Type type)
        {
            return [new FLabel(Font, Unpascal(type)) { color = overseerGraf.MainColor }];
        }

        public static void Draw(OverseerGraphics overseerGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].isVisible = overseerGraf.overseer.room != null;

            // Fixes a crash moving rooms in safari, which was where I discovered it. No clue if it existed in normal gameplay, didn't happen in arena.
            if (overseerGraf.overseer.room != null)
            {
                labels[0].SetPosition(AvgVectors(overseerGraf.DrawPosOfSegment(0f, timeStacker), overseerGraf.DrawPosOfSegment(1f, timeStacker)) - camPos);
                labels[0].rotation = AngleBtwn(overseerGraf.DrawPosOfSegment(0f, timeStacker), overseerGraf.DrawPosOfSegment(1f, timeStacker)) + 90f;
                labels[0].scale = (overseerGraf.DrawPosOfSegment(0f, timeStacker) - overseerGraf.DrawPosOfSegment(1f, timeStacker)).magnitude / TextWidth(labels[0].text);
            }
        }
    }
}
