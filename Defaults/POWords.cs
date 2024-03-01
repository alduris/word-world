using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    internal static class POWords
    {
        public static FLabel[] Init(PhysicalObject obj, string name)
        {
            var label = new FLabel(Font, name)
            {
                scale = obj.firstChunk.rad * 3f / FontSize
            };

            if (obj is PlayerCarryableItem)
                label.color = (obj as PlayerCarryableItem).color;

            return [label];
        }

        public static void Draw(PhysicalObject obj, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(obj.firstChunk, timeStacker) - camPos);

            if (obj is PlayerCarryableItem item)
                labels[0].color = item.blink > 1 && Random.value > 0.5f ? item.blinkColor : item.color;
        }
    }
}
