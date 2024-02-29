using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class LeechWords
    {
        public static FLabel[] Init(LeechGraphics leechGraf, CreatureTemplate.Type type)
        {
            var label = new FLabel(Font, PascalRegex.Replace(type.value, " "))
            {
                scale = leechGraf.leech.mainBodyChunk.rad * 4f / FontSize
            };
            return [label];
        }

        public static void Draw(LeechGraphics leechGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].color = sLeaser.sprites[0].color;
            labels[0].SetPosition(GetPos(leechGraf.leech.mainBodyChunk, timeStacker) - camPos);
            labels[0].rotation = AngleBtwnParts(leechGraf.body[0], leechGraf.body[leechGraf.body.Length - 1], timeStacker) + 90f;
        }
    }
}
