using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class KarmaFlowerWords() : POWordify<KarmaFlower>("K")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            Label.scale = 20f / FontSize;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            sLeaser.sprites[Drawable.StalkSprite].isVisible = true;
            for (int i = 0; i < 3; i++)
            {
                sLeaser.sprites[Drawable.EffectSprite(i)].isVisible = true;
            }
        }
    }
}
