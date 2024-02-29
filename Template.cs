// This is a template file not intended to be used normally, I think we can afford to shut up IntelliSense
#pragma warning disable IDE0005 // Using directive is unnecessary
#pragma warning disable IDE0060 // Remove unused parameter

using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld
{
    public static class Template
    {
        public static FLabel[] Init(IDrawable self, RoomCamera.SpriteLeaser sLeaser)
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
