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
    public static class DaddyWords
    {
        private static int Length(Tentacle tentacle) => (int)(tentacle.idealLength / FontSize);

        public static FLabel[] Init(DaddyGraphics daddyGraf, CreatureTemplate.Type type)
        {
            // Colors
            Random.State state = Random.state;
            Random.InitState(daddyGraf.daddy.abstractCreature.ID.RandomSeed);
            Color blinkColor = Plugin.ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.5f) : daddyGraf.daddy.eyeColor;
            Color bodyColor = Plugin.ClownLongLegs ? Color.white : daddyGraf.blackColor;

            // Get short name
            int cut = type.value.IndexOf("LongLegs");
            string shortname = cut <= 0 ? PascalRegex.Replace(type.value, Environment.NewLine) : type.value.Substring(0, cut);
            if (ModManager.MSC)
            {
                if (type == MoreSlugcatsEnums.CreatureTemplateType.HunterDaddy) shortname = "Hunter";
                else if (type == MoreSlugcatsEnums.CreatureTemplateType.TerrorLongLegs) shortname = $"Your{Environment.NewLine}Mother";
            }
            List<FLabel> list = [new(Font, shortname) {
                scale = Mathf.Sqrt(daddyGraf.daddy.bodyChunks.Length) * daddyGraf.daddy.bodyChunks.Average(c => c.rad) * 2f / FontSize,
                color = blinkColor
            }];

            // Tentacles
            for (int i = 0; i < daddyGraf.daddy.tentacles.Length; i++)
            {
                var tentacle = daddyGraf.daddy.tentacles[i];
                int length = Length(tentacle);
                int numOfOs = length - 7; // len("LongLeg") = 7
                Color tipColor = Plugin.ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.625f) : daddyGraf.daddy.eyeColor;

                for (int j = 0; j < length; j++)
                {
                    int k = (j >= 1 && j < 1 + numOfOs) ? 1 : (j < 1 ? j : j - numOfOs);
                    list.Add(new(Font, "LongLeg"[k].ToString())
                    {
                        scale = 1.5f,
                        color = Color.Lerp(bodyColor, tipColor, Custom.LerpMap(j, 0, length, 0f, 1f, 1.5f))
                    });
                }
            }

            Random.state = state;
            return [.. list];
        }

        public static void Draw(DaddyGraphics daddyGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            // Colors n stuff
            Random.State state = Random.state;
            Random.InitState(daddyGraf.daddy.abstractCreature.ID.RandomSeed);
            Color eyeColor = Plugin.ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.5f) : daddyGraf.daddy.eyeColor;
            Color bodyColor = Plugin.ClownLongLegs ? Color.white : daddyGraf.blackColor;
            Random.state = state;

            // Main body chunk
            labels[0].SetPosition(daddyGraf.daddy.MiddleOfBody - camPos);
            labels[0].color = Color.LerpUnclamped(eyeColor, bodyColor, Mathf.Lerp(daddyGraf.eyes[0].lastClosed, daddyGraf.eyes[0].closed, timeStacker));

            // Tentacles
            var tentacles = daddyGraf.daddy.tentacles;
            int k = 1;
            for (int i = 0; i < tentacles.Length; i++)
            {
                int length = Length(tentacles[i]);
                for (int j = 0; j < length; j++, k++)
                {
                    // Offset position by 1 to move away from center a bit
                    var pos = PointAlongRope(j + 1, length + 1, daddyGraf.legGraphics[i], timeStacker);
                    var prevPos = PointAlongRope(j, length + 1, daddyGraf.legGraphics[i], timeStacker);
                    labels[k].SetPosition(pos - camPos);
                    labels[k].rotation = AngleBtwn(pos, prevPos);
                }
            }
        }
    }
}
