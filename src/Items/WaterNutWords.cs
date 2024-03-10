using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class WaterNutWords
    {
        public static FLabel[] Init(WaterNut nut) => POWords.Init(nut, "N");

        public static void Draw(WaterNut nut, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(nut, labels, timeStacker, camPos);
            labels[0].color = Color.Lerp(sLeaser.sprites[0].color, nut.color, 0.4f);
        }
    }
}
