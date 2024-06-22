using System;
using System.Collections.Generic;
using System.Linq;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using Random = UnityEngine.Random;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class DaddyWords : CreatureWordify<DaddyGraphics>
    {
        private static int Length(Tentacle tentacle) => (int)(tentacle.idealLength / FontSize);

        private FLabel bodyLabel;
        private List<List<FLabel>> tentacleLabels;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Colors
            Random.State state = Random.state;
            Random.InitState(Drawable.daddy.abstractCreature.ID.RandomSeed);
            Color blinkColor = Plugin.ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.5f) : Drawable.daddy.eyeColor;
            Color bodyColor = Plugin.ClownLongLegs ? Color.white : Drawable.blackColor;

            // Get short name
            int cut = Type.value.IndexOf("LongLegs");
            string shortname = cut <= 0 ? PascalRegex.Replace(Type.value, Environment.NewLine) : Type.value.Substring(0, cut);
            if (ModManager.MSC)
            {
                if (Type == MoreSlugcatsEnums.CreatureTemplateType.HunterDaddy) shortname = "Hunter";
                else if (Type == MoreSlugcatsEnums.CreatureTemplateType.TerrorLongLegs) shortname = $"Your{Environment.NewLine}Mother";
            }
            bodyLabel = new(Font, shortname)
            {
                scale = Mathf.Sqrt(Drawable.daddy.bodyChunks.Length) * Drawable.daddy.bodyChunks.Average(c => c.rad) * 2f / FontSize,
                color = blinkColor
            };
            labels.Add(bodyLabel);

            // Tentacles
            for (int i = 0; i < Drawable.daddy.tentacles.Length; i++)
            {
                var tentacle = Drawable.daddy.tentacles[i];
                int length = Length(tentacle);
                int numOfOs = length - 7; // len("LongLeg") = 7
                Color tipColor = Plugin.ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.625f) : Drawable.daddy.eyeColor;

                List<FLabel> list = [];
                tentacleLabels.Add(list);
                for (int j = 0; j < length; j++)
                {
                    int k = (j >= 1 && j < 1 + numOfOs) ? 1 : (j < 1 ? j : j - numOfOs);
                    list.Add(new(Font, "LongLeg"[k].ToString())
                    {
                        scale = 1.5f,
                        color = Color.Lerp(bodyColor, tipColor, Custom.LerpMap(j, 0, length, 0f, 1f, 1.5f))
                    });
                }
                labels.AddRange(list);
            }

            Random.state = state;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Colors n stuff
            Random.State state = Random.state;
            Random.InitState(Drawable.daddy.abstractCreature.ID.RandomSeed);
            Color eyeColor = Plugin.ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.5f) : Drawable.daddy.eyeColor;
            Color bodyColor = Plugin.ClownLongLegs ? Color.white : Drawable.blackColor;
            Random.state = state;

            // Main body chunk
            bodyLabel.SetPosition(Drawable.daddy.MiddleOfBody - camPos);
            bodyLabel.color = Color.LerpUnclamped(eyeColor, bodyColor, Mathf.Lerp(Drawable.eyes[0].lastClosed, Drawable.eyes[0].closed, timeStacker));

            // Tentacles
            for (int i = 0; i < tentacleLabels.Count; i++)
            {
                var list = tentacleLabels[i];
                for (int j = 0; j < list.Count; j++)
                {
                    // Offset position by 1 to move away from center a bit
                    var pos = PointAlongRope(j + 1, list.Count + 1, Drawable.legGraphics[i], timeStacker);
                    var prevPos = PointAlongRope(j, list.Count + 1, Drawable.legGraphics[i], timeStacker);
                    list[j].SetPosition(pos - camPos);
                    list[j].rotation = AngleBtwn(pos, prevPos);
                }
            }
        }
    }
}
