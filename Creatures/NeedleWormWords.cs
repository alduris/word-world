using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class NeedleWormWords
    {
        public static FLabel[] Init(NeedleWormGraphics nootGraf, CreatureTemplate.Type type)
        {
            int cut = type.value.IndexOf("Needle");
            if (cut == -1) cut = type.value.IndexOf("Noodle");
            if (cut == -1) cut = type.value.IndexOf("Noot");
            if (cut == -1) cut = type.value.Length;

            FLabel[] labels = LabelsFromLetters(type.value.Substring(0, cut) + "Noot");
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].scale = nootGraf.worm.OnBodyRad(0) * 8f / FontSize;
            }

            return labels;
        }

        public static void Draw(NeedleWormGraphics nootGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].SetPosition(nootGraf.worm.OnBodyPos((float)i / labels.Length, timeStacker) - camPos);
                labels[i].rotation = AngleFrom(nootGraf.worm.OnBodyDir((float)i / labels.Length, timeStacker));

                // Color = body color if not angry, white if fang out as warning
                labels[i].color = Color.Lerp(nootGraf.bodyColor, Color.white, Mathf.Lerp(nootGraf.lastFangOut, nootGraf.fangOut, timeStacker));
            }
        }
    }
}
