using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class FlyWords : CreatureWordify<FlyGraphics>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, Unpascal(Type))
            {
                scale = Drawable.lowerBody.rad * 4f / FontSize,
                color = sLeaser.sprites[0].color
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.lowerBody, timeStacker) - camPos);
            label.rotation = sLeaser.sprites[0].rotation;
        }
    }
}
