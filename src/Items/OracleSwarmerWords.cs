using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class OracleSwarmerWords() : POWordify<OracleSwarmer>("N")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            Label.scale *= 1.5f;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            Label.color = sLeaser.sprites[0].color;
        }
    }
}
