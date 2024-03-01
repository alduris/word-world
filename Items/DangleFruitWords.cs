using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public static class DangleFruitWords
    {
        public static FLabel[] Init(DangleFruit fruit) => POWords.Init(fruit, "F");

        public static void Draw(DangleFruit fruit, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(fruit, labels, timeStacker, camPos);
    }
}
