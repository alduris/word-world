using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class JokeRifleWords : Wordify<JokeRifle>
    {
        public static FLabel[] Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, "Rifle")
            {
                scale = sLeaser.sprites[0].element.sourcePixelSize.x / TextWidth("Rifle"),
                color = sLeaser.sprites[0].color
            };
            return [label];
        }

        public static void Draw(JokeRifle rifle, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(rifle, labels, timeStacker, camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[0].rotation);
        }
    }
}
