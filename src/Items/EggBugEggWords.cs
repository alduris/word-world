using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class EggBugEggWords
    {
        public static FLabel[] Init(EggBugEgg egg)
        {
            return [new FLabel(Font, "Egg") { scale = egg.firstChunk.rad * 3f / TextWidth("Egg"), color = egg.eggColors[1] }];
        }

        public static void Draw(EggBugEgg egg, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(egg, labels, timeStacker, camPos);
    }
}
