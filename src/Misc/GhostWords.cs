using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class GhostWords : Wordify<Ghost>
    {
        public static FLabel[] Init(Ghost echo)
        {
            return [new(Font, "Echo")
            {
                scale = 8f * echo.scale,
                color = echo.goldColor
            }];
        }

        public static void Draw(Ghost echo, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var startPos = Vector2.Lerp(echo.spine[0].lastPos, echo.spine[0].pos, timeStacker);
            var endPos = Vector2.Lerp(echo.spine[echo.spine.Length - 1].lastPos, echo.spine[echo.spine.Length - 1].pos, timeStacker);
            labels[0].SetPosition(AvgVectors(startPos, endPos) - camPos);
            labels[0].rotation = AngleBtwn(startPos, endPos);
            sLeaser.sprites[echo.DistortionSprite].isVisible = true;
            sLeaser.sprites[echo.LightSprite].isVisible = true;
        }
    }
}
