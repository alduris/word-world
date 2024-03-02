using System;
using System.Collections.Generic;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;
using Random = UnityEngine.Random;

namespace WordWorld.Creatures
{
    public static class TubeWormWords
    {
        private static bool EasterEgg(TubeWorm worm)
        {
            // silly :3
            Random.State state = Random.state;
            Random.InitState(worm.abstractCreature.ID.RandomSeed);
            bool yes = Random.value < 0.0001f;
            Random.state = state;
            return yes;
        }

        private static int Length(float idealLength) => Math.Max((int)(idealLength / (FontSize * 0.65f)), 6); // 0.65f is how much we scale it later

        public static FLabel[] Init(TubeWormGraphics wormGraf, CreatureTemplate.Type type)
        {
            var egg = EasterEgg(wormGraf.worm) && type == CreatureTemplate.Type.TubeWorm;

            // Main chunk
            var text = type == CreatureTemplate.Type.TubeWorm ? (egg ? "SAIT" : "Worm") : Unpascal(type);
            var labels = new List<FLabel>
            {
                new(Font, text)
                {
                    scale = (wormGraf.worm.bodyChunks[0].rad + wormGraf.worm.bodyChunks[1].rad + wormGraf.worm.bodyChunkConnections[0].distance) * 2f / TextWidth(text),
                    color = egg ? new Color(0.66667f, 0.9451f, 0.33725f) : wormGraf.color
                }
            };

            // Tongues
            for (int i = 0; i < wormGraf.worm.tongues.Length; i++)
            {
                // 6 = len("Tongue")
                var tongue = wormGraf.worm.tongues[i];
                int length = Length(tongue.idealRopeLength);
                int numOfOs = length - 6;

                for (int j = 0; j > length; j++)
                {
                    int k = (j >= 1 && j < 1 + numOfOs) ? 1 : (j < 1 ? j : j - numOfOs);
                    float num = Mathf.Sin(Mathf.InverseLerp(0, length - 1, j) * Mathf.PI);
                    labels.Add(new(Font, "Tongue"[k].ToString())
                    {
                        scale = 0.65f,
                        color = Custom.HSL2RGB(Mathf.Lerp(0.95f, 1f, num), 1f, Mathf.Lerp(0.75f, 0.9f, Mathf.Pow(num, 0.15f)))
                    });
                }
            }

            // Return
            return [.. labels];
        }

        public static void Draw(TubeWormGraphics wormGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            // Main chunk
            labels[0].SetPosition(AvgBodyChunkPos(wormGraf.worm.bodyChunks[0], wormGraf.worm.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(wormGraf.worm.bodyChunks[0], wormGraf.worm.bodyChunks[1], timeStacker)) - 90f;

            // Tongues
            int k = 1;
            for (int i = 0; i < wormGraf.worm.tongues.Length; i++)
            {
                var tongue = wormGraf.worm.tongues[i];
                int length = Length(tongue.idealRopeLength); // I know ideally these should all be the same but you can never trust mischiveous modders
                var start = k;
                var end = k + length;

                if (tongue.Free || tongue.Attached)
                {
                    List<Vector2> positions = [GetPos(tongue.baseChunk, timeStacker)];
                    positions.AddRange(tongue.rope.GetAllPositions());

                    for (int j = k; j < end; j++, k++)
                    {
                        labels[j].isVisible = true;

                        // Lerp along tongue
                        var x = Mathf.InverseLerp(end - 1, start, j);
                        labels[j].SetPosition(PointAlongVectors(x, positions) - camPos);
                    }
                }
                else
                {
                    for (int j = k; j < end; j++, k++)
                    {
                        labels[j].isVisible = false;
                    }
                }
            }
        }
    }
}
