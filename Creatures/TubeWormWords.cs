using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class TubeWormWords
    {
        public static FLabel[] Init(TubeWormGraphics wormGraf, CreatureTemplate.Type type)
        {
            var text = Unpascal(type);
            return [new FLabel(Font, text)
            {
                scale = (wormGraf.worm.bodyChunks[0].rad + wormGraf.worm.bodyChunks[1].rad + wormGraf.worm.bodyChunkConnections[0].distance) * 2f / TextWidth(text),
                color = wormGraf.color
            }];
        }

        public static void Draw(TubeWormGraphics wormGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(AvgBodyChunkPos(wormGraf.worm.bodyChunks[0], wormGraf.worm.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(wormGraf.worm.bodyChunks[0], wormGraf.worm.bodyChunks[1], timeStacker)) - 90f;
        }
    }
}
