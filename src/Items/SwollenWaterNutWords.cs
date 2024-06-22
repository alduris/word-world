using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SwollenWaterNutWords : Wordify<SwollenWaterNut>
    {
        public static FLabel[] Init(SwollenWaterNut nut) => POWords.Init(nut, "N");

        public static void Draw(SwollenWaterNut nut, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(nut, labels, timeStacker, camPos);
            labels[0].color = Color.Lerp(sLeaser.sprites[2].color, new Color(0f, 0.4f, 1f), 0.4f);
        }
    }
}
