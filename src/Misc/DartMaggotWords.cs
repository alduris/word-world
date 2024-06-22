using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class DartMaggotWords : Wordify<DartMaggot>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new(Font, "Maggot");
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            // this rotation thing doesn't work as intended but oh well /shrug
            label.rotation = FixRotation(AngleBtwn(
                Vector2.Lerp(Drawable.body[0, 1], Drawable.body[0, 0], timeStacker),
                Vector2.Lerp(Drawable.body[Drawable.body.GetLength(0) - 1, 1], Drawable.body[Drawable.body.GetLength(0) - 1, 0], timeStacker))) - 90f;
            label.color = Drawable.yellow;
        }
    }
}
