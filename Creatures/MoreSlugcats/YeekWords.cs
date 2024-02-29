using System.Linq;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.MoreSlugcats
{
    public static class YeekWords
    {
        public static FLabel[] Init(YeekGraphics yeekGraf, CreatureTemplate.Type type)
        {
            var label = new FLabel(Font, type.value);
            label.scale = yeekGraf.myYeek.bodyChunks.Sum(c => c.rad) * 2f / TextWidth(label.text);
            label.color = yeekGraf.tailHighlightColor;
            return [label];
        }

        public static void Draw(YeekGraphics yeekGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(yeekGraf.myYeek.mainBodyChunk, timeStacker) - camPos);
            // labels[0].rotation = Custom.VecToDeg(yeekGraf.myYeek.bodyDirection);
            labels[0].rotation = sLeaser.sprites[yeekGraf.HeadSpritesStart + 2].rotation;
        }
    }
}
