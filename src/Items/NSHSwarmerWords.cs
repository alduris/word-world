using RWCustom;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class NSHSwarmerWords() : POWordify<NSHSwarmer>("N") // why doesn't NSHSwarmer extend OracleSwarmer :(
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            Label.color = Drawable.myColor;
            Label.scale *= 1.5f;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            Label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);

            // Show the hologram thingy
            var active = Custom.SCurve(Mathf.Lerp(Drawable.lastHoloFade, Drawable.holoFade, timeStacker), 0.65f) * Drawable.holoShape.Fade.SmoothValue(timeStacker);
            if (active > 0f)
            {
                for (int i = 5; i < sLeaser.sprites.Length; i++)
                {
                    sLeaser.sprites[i].isVisible = true;
                }
            }
        }
    }
}
