using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class VultureMaskWords : Wordify<VultureMask>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "Mask")
            {
                scale = 17.5f / FontSize * (Drawable.King ? 1.15f : 1f),
                rotation = 90f
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(sLeaser.sprites[0].GetPosition());
            label.rotation = sLeaser.sprites[0].rotation + 90f;
            label.color = sLeaser.sprites[0].color;
        }
    }
}
