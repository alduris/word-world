using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class MirosBirdWords : CreatureWordify<MirosBirdGraphics>
    {
        private FLabel bodyLabel;
        private FLabel eyeLabel;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            bodyLabel = new(Font, Unpascal(Type))
            {
                scale = Drawable.bird.mainBodyChunk.rad * 2f / FontSize,
                color = sLeaser.sprites[0].color
            };
            eyeLabel = new(Font, "Eye")
            {
                scale = Drawable.bird.Head.rad * 2f / FontSize,
                color = Drawable.EyeColor
            };
            labels.Add(bodyLabel);
            labels.Add(eyeLabel);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main body label
            bodyLabel.SetPosition(sLeaser.sprites[Drawable.BodySprite].GetPosition());
            bodyLabel.rotation = sLeaser.sprites[Drawable.BodySprite].rotation;

            // Eye label
            eyeLabel.SetPosition(sLeaser.sprites[Drawable.HeadSprite].GetPosition());
            eyeLabel.rotation = sLeaser.sprites[Drawable.HeadSprite].rotation;
            eyeLabel.color = Drawable.EyeColor;

            // Re-enable eye trail sprite
            sLeaser.sprites[Drawable.EyeTrailSprite].isVisible = true;
        }
    }
}
