using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class BigSpiderWords : Wordify<BigSpider>
    {
        public static FLabel[] Init(BigSpiderGraphics bigSpiderGraf, CreatureTemplate.Type type)
        {
            var label = new FLabel(Font, type == CreatureTemplate.Type.BigSpider ? "Big Spider" : Unpascal(type.value));
            label.scale = (bigSpiderGraf.bug.bodyChunks[0].rad + bigSpiderGraf.bug.bodyChunks[1].rad + bigSpiderGraf.bug.bodyChunkConnections[0].distance) * 1.5f / TextWidth(label.text);
            if (bigSpiderGraf.bug.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.BigSpider)
                label.scale *= 2.5f / 1.5f;
            label.color = bigSpiderGraf.yellowCol;
            return [label];
        }

        public static void Draw(BigSpiderGraphics bigSpiderGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            var pos = GetPos(bigSpiderGraf.bug.bodyChunks[1], timeStacker);
            label.SetPosition(pos - camPos);
            label.rotation = FixRotation(sLeaser.sprites[bigSpiderGraf.HeadSprite].rotation) + 90f;
        }
    }
}
