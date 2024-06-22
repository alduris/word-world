using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class FlareBombWords : Wordify<FlareBomb>
    {
        public static FLabel[] Init(FlareBomb flare)
        {
            var label = new FLabel(Font, "F")
            {
                scale = flare.firstChunk.rad * 4f / FontSize,
                color = Color.Lerp(flare.color, new(1f, 1f, 1f), 0.9f)
            };
            return [label];
        }

        public static void Draw(FlareBomb flare, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(flare.firstChunk, timeStacker) - camPos);
            sLeaser.sprites[2].isVisible = true;
        }
    }
}
