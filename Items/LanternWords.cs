using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public static class LanternWords
    {
        public static FLabel[] Init(Lantern lantern, RoomCamera.SpriteLeaser sLeaser)
        {
            var labels = POWords.Init(lantern, "L");
            labels[0].color = sLeaser.sprites[0].color;
            return labels;
        }

        public static void Draw(Lantern lantern, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(lantern, labels, timeStacker, camPos);
            sLeaser.sprites[3].isVisible = true;
        }
    }
}
