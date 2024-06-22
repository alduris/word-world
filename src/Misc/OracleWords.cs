using System;
using System.Collections.Generic;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class OracleWords : Wordify<Oracle>
    {
        public static FLabel[] Init(OracleGraphics oracleGraf, RoomCamera.SpriteLeaser sLeaser)
        {
            List<FLabel> labels = [];
            var str = "Iterator";

            // Figure out the name of the iterator
            var id = oracleGraf.oracle.ID;
            if (WordAPI.RegisteredIterators.ContainsKey(id))
                str = WordAPI.RegisteredIterators[id];
            else if (id == Oracle.OracleID.SL)
                str = "Looks to the Moon";
            else if (id == Oracle.OracleID.SS)
                str = "Five Pebbles";
            else if (ModManager.MSC)
            {
                if (id == MoreSlugcatsEnums.OracleID.CL)
                    str = "Five Pebbles";
                else if (id == MoreSlugcatsEnums.OracleID.DM)
                    str = "Looks to the Moon";
                else if (id == MoreSlugcatsEnums.OracleID.ST)
                    str = "Sliver of Straw";
            }
            labels.Add(new(Font, string.Join(Environment.NewLine, str.Split(' ')))
            {
                scale = 1.25f,
                color = sLeaser.sprites[oracleGraf.HeadSprite].color
            });

            // Arm
            if (oracleGraf.oracle.arm != null)
            {
                for (int i = 0; i < oracleGraf.oracle.arm.joints.Length; i++)
                {
                    labels.Add(new(Font, "Joint") { color = oracleGraf.GenericJointBaseColor() });
                    labels.Add(new(Font, "Arm") { color = oracleGraf.GenericJointHighLightColor() });
                }
            }

            // Umbilical cord
            if (oracleGraf.umbCord != null)
            {
                foreach (char letter in "UmbilicalCord".ToCharArray())
                {
                    labels.Add(new(Font, letter.ToString()) { scale = 0.75f, color = oracleGraf.GenericJointBaseColor() });
                }
            }

            return [.. labels];
        }

        public static void Draw(OracleGraphics oracleGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body
            //labels[0].SetPosition(AvgBodyChunkPos(oracleGraf.oracle.bodyChunks[0], oracleGraf.oracle.bodyChunks[1], timeStacker) - camPos);
            labels[0].SetPosition(sLeaser.sprites[oracleGraf.HeadSprite].GetPosition());
            labels[0].rotation = AngleBtwnChunks(oracleGraf.oracle.bodyChunks[1], oracleGraf.oracle.bodyChunks[0], timeStacker);

            // Arm stuff
            int armStop = (oracleGraf.umbCord != null || oracleGraf.discUmbCord != null) ? labels.Length - "UmbilicalCord".Length : labels.Length;
            for (int i = 1; i < armStop; i++)
            {
                bool isArm = i % 2 == 0;
                var arm = oracleGraf.oracle.arm.joints[(i - 1) / 2];
                var armPos = Vector2.Lerp(arm.lastPos, arm.pos, timeStacker);
                if (isArm)
                {
                    Vector2 armNextPos = arm.next != null ? Vector2.Lerp(arm.next.lastPos, arm.next.pos, timeStacker) : GetPos(oracleGraf.oracle.bodyChunks[1], timeStacker);
                    labels[i].SetPosition(AvgVectors(armPos, armNextPos) - camPos);
                    // labels[i].SetPosition(arm.ElbowPos(timeStacker, armNextPos));
                }
                else
                {
                    labels[i].SetPosition(armPos - camPos);
                }
            }

            // Umbilical stuff
            if (oracleGraf.umbCord != null)
            {
                var cord = oracleGraf.umbCord.coord;
                for (int i = armStop; i < labels.Length; i++)
                {
                    // adds a padding of one space around it
                    var index = Custom.LerpMap(i, armStop - 1, labels.Length, 0, cord.GetLength(0));
                    var prevPos = Vector2.Lerp(cord[Mathf.FloorToInt(index), 1], cord[Mathf.FloorToInt(index), 0], timeStacker);
                    var nextPos = Vector2.Lerp(cord[Mathf.CeilToInt(index), 1], cord[Mathf.CeilToInt(index), 0], timeStacker);
                    var pos = Vector2.Lerp(prevPos, nextPos, index % 1f);
                    var rot = AngleBtwn(prevPos, nextPos) + 90f;

                    labels[i].SetPosition(pos - camPos);
                    labels[i].rotation = rot;
                }
            }

            // Show halo
            if (oracleGraf.halo != null)
            {
                for (int i = oracleGraf.halo.firstSprite; i < oracleGraf.halo.firstSprite + oracleGraf.halo.totalSprites; i++)
                {
                    sLeaser.sprites[i].isVisible = true;
                }
            }
        }
    }
}
