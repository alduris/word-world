using MoreSlugcats;
using RWCustom;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class SingularityBombWords() : POWordify<SingularityBomb>("S")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink < 1) Label.color = Custom.HSL2RGB(0.6638889f, 1f, 0.35f);
        }
    }
}
