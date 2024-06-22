using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class MushroomWords() : POWordify<Mushroom>("M")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            Label.scale = sLeaser.sprites[Drawable.HatSprite].element.sourcePixelSize.y / FontSize * 2f;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            Label.color = sLeaser.sprites[Drawable.HatSprite].color;
            sLeaser.sprites[Drawable.StalkSprite].isVisible = true;
            sLeaser.sprites[Drawable.EffectSprite].isVisible = true;
        }
    }
}
