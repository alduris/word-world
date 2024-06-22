using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class NeedleWormWords : CreatureWordify<NeedleWormGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            int cut = Type.value.IndexOf("Needle");
            if (cut == -1) cut = Type.value.IndexOf("Noodle");
            if (cut == -1) cut = Type.value.IndexOf("Noot");
            if (cut == -1) cut = Type.value.Length;

            labels.AddRange(LabelsFromLetters(Type.value.Substring(0, cut) + "Noot"));
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].scale = Drawable.worm.OnBodyRad(0) * 8f / FontSize;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                float p = i / (labels.Count - 1f);
                labels[i].SetPosition(Drawable.worm.OnBodyPos(p, timeStacker) - camPos);
                labels[i].rotation = AngleFrom(Drawable.worm.OnBodyDir(p, timeStacker));

                // Color = body color if not angry, white if fang out as warning
                labels[i].color = Color.Lerp(Drawable.bodyColor, Color.white, Mathf.Lerp(Drawable.lastFangOut, Drawable.fangOut, timeStacker));
            }
        }
    }
}
