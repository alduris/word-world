using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class LizardWords : CreatureWordify<LizardGraphics>
    {
        private readonly List<FLabel> bodyLabels = [];
        private List<FLabel> tongueLabels = null;

        private bool isCyan = false;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var lizard = Drawable.lizard;

            // Body
            var name = PascalRegex.Replace(Type.value, " ");
            var nameScale = (lizard.bodyChunks.Sum(x => x.rad) * 2f + lizard.bodyChunkConnections.Sum(x => x.distance)) / TextWidth(name);
            foreach (var c in name)
            {
                bodyLabels.Add(new FLabel(Font, c.ToString()) { scale = nameScale });
            }
            labels.AddRange(bodyLabels);

            // Tongue
            if (Drawable.tongue != null)
            {
                tongueLabels = [];
                for (int i = 0; i < 6; i++)
                {
                    tongueLabels.Add(new(Font, "Tongue"[i].ToString())
                    {
                        scale = 0.875f,
                        color = Drawable.palette.blackColor
                    });
                }
                labels.AddRange(tongueLabels);
            }

            // Extra data
            isCyan = lizard.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.CyanLizard;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main body
            var name = PascalRegex.Replace(Drawable.lizard.abstractCreature.creatureTemplate.type.value, " ");
            var nameLen = TextWidth(name);
            var chunks = Drawable.lizard.bodyChunks;
            var headSize = Mathf.Max(0.3f, (Drawable.head.rad + Drawable.headConnectionRad) / (Drawable.head.rad + Drawable.headConnectionRad + Drawable.BodyAndTailLength));

            bool side = GetPos(chunks[0], timeStacker).x < GetPos(chunks[2], timeStacker).x;
            var angle = FixRotation(AngleBtwnChunks(chunks[0], chunks[2], timeStacker)) - 90f;
            List<Vector2> points = chunks.Select(x => GetPos(x, timeStacker)).ToList();
            foreach (var part in Drawable.tail)
            {
                points.Add(GetPos(part, timeStacker));
            }

            for (int i = 0; i < bodyLabels.Count; i++)
            {
                var lerp = (TextWidth(name.Substring(0, i)) + TextWidth(name[i].ToString()) / 2f) / nameLen; //(float)i / (nameLen - 1);
                lerp = side ? lerp : 1 - lerp;
                bodyLabels[i].color = (lerp <= headSize || i == 0) ? Drawable.HeadColor(timeStacker) : Drawable.BodyColor(lerp);
                bodyLabels[i].SetPosition(PointAlongVectors(lerp, points) - camPos);
                bodyLabels[i].rotation = RotationAlongVectors(lerp, points) + (side ? -90f : 90f);
            }
            
            // Tongue
            if (Drawable.tongue != null)
            {
                for (int i = 0; i < tongueLabels.Count; i++)
                {
                    var label = tongueLabels[i];
                    label.SetPosition(PointAlongParts(i, 6, Drawable.tongue, timeStacker) - camPos);
                    label.isVisible = Drawable.lizard.tongue.Out;
                    
                    // Cyans get special tongue color
                    if (isCyan)
                        label.color = Color.Lerp(Drawable.HeadColor(timeStacker), Drawable.palette.blackColor, i / 5f);
                }
            }
        }
    }
}
