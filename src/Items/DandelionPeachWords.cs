using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class DandelionPeachWords : Wordify<DandelionPeach>
    {
        public static FLabel[] Init(DandelionPeach obj) => POWords.Init(obj, "P");

        public static void Draw(DandelionPeach obj, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(obj, labels, timeStacker, camPos);
    }
}
