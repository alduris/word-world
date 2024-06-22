using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class KarmaFlowerWords : Wordify<KarmaFlower>
    {
        public static FLabel[] Init(KarmaFlower plant)
        {
            return [new FLabel(Font, "K") { scale = 20f / FontSize, color = plant.color }];
        }

        public static void Draw(KarmaFlower plant, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(plant, labels, timeStacker, camPos);
            sLeaser.sprites[plant.StalkSprite].isVisible = true;
            for (int i = 0; i < 3; i++)
            {
                sLeaser.sprites[plant.EffectSprite(i)].isVisible = true;
            }
        }
    }
}
