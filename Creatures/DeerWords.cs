using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class DeerWords
    {
        public static FLabel[] Init(DeerGraphics deerGraf, CreatureTemplate.Type type)
        {
            var bodyChunks = deerGraf.deer.bodyChunks;
            var bodyLabel = new FLabel(Font, Unpascal(type))
            {
                scale = (bodyChunks[1].rad + bodyChunks[2].rad + bodyChunks[3].rad + bodyChunks[4].rad) / 2f / FontSize,
                color = deerGraf.bodyColor
            };
            var antlerLabel = new FLabel(Font, "Antlers")
            {
                scale = bodyChunks[5].rad / FontSize,
                color = deerGraf.bodyColor
            };

            // TODO: legs

            return [bodyLabel, antlerLabel];
        }

        public static void Draw(DeerGraphics deerGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var bodyChunks = deerGraf.deer.bodyChunks;
            labels[0].SetPosition(
                AvgVectors(
                    AvgBodyChunkPos(bodyChunks[1], bodyChunks[2], timeStacker),
                    AvgBodyChunkPos(bodyChunks[3], bodyChunks[4], timeStacker)
                ) - camPos
            );
            labels[1].SetPosition(GetPos(bodyChunks[5], timeStacker));
            labels[1].rotation = Custom.VecToDeg(deerGraf.deer.HeadDir);
        }
    }
}
