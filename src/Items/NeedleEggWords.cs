using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class NeedleEggWords : Wordify<NeedleEgg>
    {
        public static FLabel[] Init(NeedleEgg egg) => POWords.Init(egg, "Egg");

        public static void Draw(NeedleEgg egg, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(egg, labels, timeStacker, camPos);
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
