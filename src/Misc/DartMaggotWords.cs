using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class DartMaggotWords : Wordify<DartMaggot>
    {
        public static FLabel[] Init()
        {
            return [new(Font, "Maggot")];
        }

        public static void Draw(DartMaggot maggot, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(maggot.firstChunk, timeStacker) - camPos);
            // this rotation thing doesn't work as intended but oh well /shrug
            labels[0].rotation = FixRotation(AngleBtwn(
                Vector2.Lerp(maggot.body[0, 1], maggot.body[0, 0], timeStacker),
                Vector2.Lerp(maggot.body[maggot.body.GetLength(0) - 1, 1], maggot.body[maggot.body.GetLength(0) - 1, 0], timeStacker))) - 90f;
            labels[0].color = maggot.yellow;
        }
    }
}
