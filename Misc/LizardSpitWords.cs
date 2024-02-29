using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public static class LizardSpitWords
    {
        public static FLabel[] Init(LizardSpit spit, RoomCamera.SpriteLeaser sLeaser)
        {
            return [new(Font, "Spit") { color = Color.Lerp(sLeaser.sprites[spit.DotSprite].color, sLeaser.sprites[spit.JaggedSprite].color, 0.4f) }];
        }

        public static void Draw(LizardSpit spit, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(Vector2.Lerp(spit.lastPos, spit.pos, timeStacker) - camPos);
            labels[0].scale = spit.Rad * 4f / FontSize;
        }
    }
}
