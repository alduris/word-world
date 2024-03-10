using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public static class OverseerCarcassWords
    {
        public static FLabel[] Init(OverseerCarcass eye)
        {
            var abstr = (eye.abstractPhysicalObject as OverseerCarcass.AbstractOverseerCarcass);
            var big = ModManager.MSC && abstr.InspectorMode;
            var labels = POWords.Init(eye, big ? "Eye" : "O");
            if (!big)
            {
                labels[0].scale /= 1.5f;
            }
            labels[0].color = abstr.color;
            return labels;
        }

        public static void Draw(OverseerCarcass eye, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(eye, labels, timeStacker, camPos);
            labels[0].color = sLeaser.sprites[5].color;
        }
    }
}
