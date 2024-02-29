using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    internal static class POWords
    {
        public static FLabel[] Init(PhysicalObject item, string name)
        {
            var label = new FLabel(Font, name)
            {
                scale = item.firstChunk.rad * 3f / FontSize
            };

            if (item is PlayerCarryableItem)
                label.color = (item as PlayerCarryableItem).color;

            return [label];
        }

        public static void Draw(PhysicalObject obj, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(obj.firstChunk, timeStacker) - camPos);
        }
    }
}
