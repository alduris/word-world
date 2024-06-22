using System;
using System.Collections.Generic;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;
using Random = UnityEngine.Random;

namespace WordWorld.Creatures
{
    public class TubeWormWords : CreatureWordify<TubeWormGraphics>
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

        private FLabel bodyLabel;
        private readonly List<List<FLabel>> tongueLabels = [];

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var egg = EasterEgg(Critter as TubeWorm) && Type == CreatureTemplate.Type.TubeWorm;

            // Main chunk
            var text = Type == CreatureTemplate.Type.TubeWorm ? (egg ? "SAIT" : "Worm") : Unpascal(Type);
            bodyLabel = new(Font, text)
            {
                scale = (Drawable.worm.bodyChunks[0].rad + Drawable.worm.bodyChunks[1].rad + Drawable.worm.bodyChunkConnections[0].distance) * 2f / TextWidth(text),
                color = egg ? new Color(0.66667f, 0.9451f, 0.33725f) : Drawable.color
            };
            labels.Add(bodyLabel);

            // Tongues
            for (int i = 0; i < Drawable.worm.tongues.Length; i++)
            {
                // 6 = len("Tongue")
                var tongue = Drawable.worm.tongues[i];
                int length = Length(tongue.idealRopeLength);
                int numOfOs = length - 6;

                List<FLabel> list = [];
                for (int j = 0; j < length; j++)
                {
                    int k = (j >= 1 && j < 1 + numOfOs) ? 1 : (j < 1 ? j : j - numOfOs);
                    float num = Mathf.Sin(Mathf.InverseLerp(0, length - 1, j) * Mathf.PI);
                    list.Add(new(Font, "Tongue"[k].ToString())
                    {
                        scale = 0.65f,
                        color = Custom.HSL2RGB(Mathf.Lerp(0.95f, 1f, num), 1f, Mathf.Lerp(0.75f, 0.9f, Mathf.Pow(num, 0.15f)))
                    });
                }
                tongueLabels.Add(list);
                labels.AddRange(list);
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main chunk
            labels[0].SetPosition(AvgBodyChunkPos(Drawable.worm.bodyChunks[0], Drawable.worm.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(Drawable.worm.bodyChunks[0], Drawable.worm.bodyChunks[1], timeStacker)) - 90f;

            // Tongues
            for (int i = 0; i < Drawable.worm.tongues.Length; i++)
            {
                var tongue = Drawable.worm.tongues[i];

                if (tongue.Free || tongue.Attached)
                {
                    List<Vector2> positions = [GetPos(tongue.baseChunk, timeStacker)];
                    positions.AddRange(tongue.rope.GetAllPositions());

                    var labels = tongueLabels[i];
                    for (int j = 0; j < labels.Count; j++)
                    {
                        labels[j].isVisible = true;

                        // Lerp along tongue
                        var x = Mathf.InverseLerp(labels.Count, 0, j);
                        labels[j].SetPosition(PointAlongVectors(x, positions) - camPos);
                    }
                }
                else
                {
                    foreach (var label in tongueLabels[i])
                    {
                        labels[i].isVisible = false;
                    }
                }
            }
        }
    }
}
