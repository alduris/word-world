using MoreSlugcats;
using RWCustom;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class SingularityBombWords : Wordify<SingularityBomb>
    {
        public static FLabel[] Init(SingularityBomb bomb)
        {
            var labels = POWords.Init(bomb, "S");
            labels[0].color = Custom.HSL2RGB(0.6638889f, 1f, 0.35f);
            return labels;
        }

        public static void Draw(SingularityBomb bomb, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(bomb, labels, timeStacker, camPos);
    }
}
