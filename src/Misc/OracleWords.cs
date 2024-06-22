using System;
using System.Collections.Generic;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class OracleWords : Wordify<OracleGraphics>
    {
        private string OracleName
        {
            get
            {
                var id = Drawable.oracle.ID;

                // Check API
                if (WordAPI.RegisteredIterators.ContainsKey(id))
                    return WordAPI.RegisteredIterators[id];

                // Default names
                return id.value switch
                {
                    "SL" or "DM" => $"Looks{Environment.NewLine}to the{Environment.NewLine}Moon",
                    "SS" or "CL" => $"Five{Environment.NewLine}Pebbles",
                    "ST" => $"Sliver{Environment.NewLine}of Straw",
                    "CW" => $"Chasing{Environment.NewLine}Wind",
                    "NSH" => $"No Significant{Environment.NewLine}Harrassment",
                    "SRS" => $"Seven{Environment.NewLine}Red Suns",
                    _ => "Iterator"
                };
            }
        }

        private FLabel bodyLabel;
        private List<(FLabel joint, FLabel arm)> armLabels;
        private List<FLabel> umbLabels;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Body
            bodyLabel = new(Font, OracleName)
            {
                scale = 1.25f,
                color = sLeaser.sprites[Drawable.HeadSprite].color
            };
            labels.Add(bodyLabel);

            // Arm
            if (Drawable.oracle.arm != null)
            {
                armLabels = [];
                for (int i = 0; i < Drawable.oracle.arm.joints.Length; i++)
                {
                    (FLabel joint, FLabel arm) thing = (
                        new(Font, "Joint") { color = Drawable.GenericJointBaseColor() },
                        new(Font, "Arm") { color = Drawable.GenericJointHighLightColor() }
                    );
                    armLabels.Add(thing);
                    labels.Add(thing.joint);
                    labels.Add(thing.arm);
                }
            }

            // Umbilical cord
            if (Drawable.umbCord != null)
            {
                umbLabels = [];
                foreach (char letter in "UmbilicalCord".ToCharArray())
                {
                    umbLabels.Add(new(Font, letter.ToString()) { scale = 0.75f, color = Drawable.GenericJointBaseColor() });
                }
                labels.AddRange(umbLabels);
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body
            labels[0].SetPosition(sLeaser.sprites[Drawable.HeadSprite].GetPosition());
            labels[0].rotation = AngleBtwnChunks(Drawable.oracle.bodyChunks[1], Drawable.oracle.bodyChunks[0], timeStacker);

            // Arm stuff
            if (armLabels != null)
            {
                for (int i = 0; i < armLabels.Count; i++)
                {
                    var arm = Drawable.oracle.arm.joints[i];
                    var armPos = Vector2.Lerp(arm.lastPos, arm.pos, timeStacker);
                    Vector2 armNextPos = arm.next != null ? Vector2.Lerp(arm.next.lastPos, arm.next.pos, timeStacker) : GetPos(Drawable.oracle.bodyChunks[1], timeStacker);

                    armLabels[i].joint.SetPosition(armPos - camPos);
                    armLabels[i].arm.SetPosition(AvgVectors(armPos, armNextPos) - camPos);
                }
            }

            // Umbilical stuff
            if (umbLabels != null)
            {
                var cord = Drawable.umbCord.coord;
                for (int i = 0; i < umbLabels.Count; i++)
                {
                    // adds a padding of one space around it
                    var index = Custom.LerpMap(i, 0, umbLabels.Count, 0, cord.GetLength(0) - 1);
                    var prevPos = Vector2.Lerp(cord[Mathf.FloorToInt(index), 1], cord[Mathf.FloorToInt(index), 0], timeStacker);
                    var nextPos = Vector2.Lerp(cord[Mathf.CeilToInt(index), 1], cord[Mathf.CeilToInt(index), 0], timeStacker);
                    var pos = Vector2.Lerp(prevPos, nextPos, index % 1f);
                    var rot = AngleBtwn(prevPos, nextPos) + 90f;

                    umbLabels[i].SetPosition(pos - camPos);
                    umbLabels[i].rotation = rot;
                }
            }

            // Show halo
            if (Drawable.halo != null)
            {
                for (int i = Drawable.halo.firstSprite; i < Drawable.halo.firstSprite + Drawable.halo.totalSprites; i++)
                {
                    sLeaser.sprites[i].isVisible = true;
                }
            }
        }
    }
}
