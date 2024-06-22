using System.Linq;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.MoreSlugcats
{
    public class YeekWords : CreatureWordify<YeekGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, Type.value);
            label.scale = Drawable.myYeek.bodyChunks[0].rad * 4f / TextWidth(label.text);
            label.color = Drawable.furColor;
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(Drawable.myYeek.mainBodyChunk, timeStacker) - camPos);
            labels[0].rotation = Custom.VecToDeg(Vector2.Lerp(Drawable.lastHeadDrawDirection, Drawable.headDrawDirection, timeStacker));
        }
    }
}
