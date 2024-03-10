using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class FireEggWords
    {
        public static FLabel[] Init(FireEgg egg)
        {

            return [new FLabel(Font, "Egg") { scale = egg.firstChunk.rad * 3f / TextWidth("Egg"), color = egg.eggColors[1] }];
        }

        public static void Draw(FireEgg egg, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(egg.firstChunk, timeStacker) - camPos);
            labels[0].scale = egg.firstChunk.rad * 3f / TextWidth(labels[0].text);
            labels[0].color = sLeaser.sprites[1].color;
        }
    }
}
