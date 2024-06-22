using System.Linq;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.MoreSlugcats
{
    public class YeekWords : Wordify<Yeek>
    {
        public static FLabel[] Init(YeekGraphics yeekGraf, CreatureTemplate.Type type)
        {
            var label = new FLabel(Font, type.value);
            label.scale = yeekGraf.myYeek.bodyChunks[0].rad * 4f / TextWidth(label.text);
            label.color = yeekGraf.furColor;
            return [label];
        }

        public static void Draw(YeekGraphics yeekGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(yeekGraf.myYeek.mainBodyChunk, timeStacker) - camPos);
            labels[0].rotation = Custom.VecToDeg(Vector2.Lerp(yeekGraf.lastHeadDrawDirection, yeekGraf.headDrawDirection, timeStacker));
            // labels[0].rotation = sLeaser.sprites[yeekGraf.HeadSpritesStart + 2].rotation;
        }
    }
}
