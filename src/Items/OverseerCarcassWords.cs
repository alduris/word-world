using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class OverseerCarcassWords() : POWordify<OverseerCarcass>(null)
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var abstr = (Drawable.abstractPhysicalObject as OverseerCarcass.AbstractOverseerCarcass);
            var big = ModManager.MSC && abstr.InspectorMode;
            text = big ? "Eye" : "O";
            base.Init(sLeaser);
            if (!big)
            {
                Label.scale /= 1.5f;
            }
            Label.color = abstr.color;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink <= 1) Label.color = sLeaser.sprites[5].color;
        }
    }
}
