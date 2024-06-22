using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class RockWords : Wordify<Rock>
    {
        public static FLabel[] Init(Rock rock) => POWords.Init(rock, "R");

        public static void Draw(Rock rock, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(rock, labels, timeStacker, camPos);
    }
}
