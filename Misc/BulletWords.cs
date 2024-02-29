using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Misc
{
    public static class BulletWords
    {
        public static FLabel[] Init(Bullet bullet) => POWords.Init(bullet, "B");

        public static void Draw(Bullet bullet, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(bullet, labels, timeStacker, camPos);
    }
}
