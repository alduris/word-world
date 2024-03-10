using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class MushroomWords
    {
        public static FLabel[] Init(Mushroom plant, RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, "M")
            {
                scale = sLeaser.sprites[plant.HatSprite].element.sourcePixelSize.y / FontSize * 2f
            };
            return [label];
        }

        public static void Draw(Mushroom plant, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(plant, labels, timeStacker, camPos);
            labels[0].color = sLeaser.sprites[plant.HatSprite].color;
            sLeaser.sprites[plant.StalkSprite].isVisible = true;
            sLeaser.sprites[plant.EffectSprite].isVisible = true;
        }
    }
}
