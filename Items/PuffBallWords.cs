using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public static class PuffBallWords
    {
        public static FLabel[] Init(PuffBall puff) => POWords.Init(puff, "Puff");

        public static void Draw(PuffBall puff, FLabel[] labels, float timeStacker, Vector2 camPos) => POWords.Draw(puff, labels, timeStacker, camPos);
    }
}
