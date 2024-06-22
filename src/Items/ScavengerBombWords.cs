using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class ScavengerBombWords : Wordify<ScavengerBomb>
    {
        public static FLabel[] Init(ScavengerBomb bomb) => POWords.Init(bomb, "B");

        public static void Draw(ScavengerBomb bomb, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(bomb, labels, timeStacker, camPos);
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
