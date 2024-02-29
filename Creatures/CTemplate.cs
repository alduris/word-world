#pragma warning disable IDE0005 // Using directive is unnecessary
#pragma warning disable IDE0060 // Remove unused parameter

using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class CTemplate
    {
        public static FLabel[] Init(IDrawable self, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            return null;
        }

        public static void Draw(IDrawable obj, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            //
        }
    }
}

#pragma warning restore IDE0005 // Using directive is unnecessary
#pragma warning restore IDE0060 // Remove unused parameter
