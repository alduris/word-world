using System.Collections.Generic;
using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class VultureWords : CreatureWordify<VultureGraphics>
    {
        private FLabel bodyLabel, headLabel, maskLabel;
        private readonly List<List<FLabel>> wingLabels = [];
        private FLabel[] tuskLabels;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            bodyLabel = new(Font, PascalRegex.Replace(Type.value, Environment.NewLine))
            {
                scale = Drawable.vulture.bodyChunks[0].rad * 4f / FontSize,
                color = sLeaser.sprites[Drawable.BodySprite].color
            };
            headLabel = new(Font, "Head")
            {
                scale = Drawable.vulture.bodyChunks[4].rad * 4f / FontSize,
                color = sLeaser.sprites[Drawable.EyesSprite].color
            };
            maskLabel = new(Font, "Mask")
            {
                scale = 17.5f / FontSize * (Drawable.IsKing ? 1.15f : 1f)
            };

            for (int i = 0; i < Drawable.vulture.tentacles.Length; i++)
            {
                List<FLabel> list = [];
                for (int j = 0; j < 4; j++)
                {
                    list.Add(new(Font, "Wing"[j].ToString())
                    {
                        scale = 1.5f,
                        color = HSLColor.Lerp(Drawable.ColorA, Drawable.ColorB, j / 3f).rgb
                    });
                }

                wingLabels.Add(list);
                labels.AddRange(list);
            }
            if (Drawable.vulture.kingTusks != null)
            {
                tuskLabels = new FLabel[Drawable.tusks.Length];
                for (int i = 0; i < Drawable.tusks.Length; i++)
                {
                    labels.Add(tuskLabels[i] = new(Font, "Tusk")
                    {
                        scale = KingTusks.Tusk.length / TextWidth("Tusk"),
                        color = sLeaser.sprites[Drawable.MaskSprite].color
                    });
                }
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Labels: 0 -> body, 1 -> mask, [2,2+len(tentacles)*4] -> wings, the rest -> tusks (may not be present)
            var chunks = Drawable.vulture.bodyChunks;
            var tentacles = Drawable.vulture.tentacles; // vulture wings

            // Body sprite
            bodyLabel.SetPosition(GetPos(chunks[0], timeStacker) - camPos);
            bodyLabel.rotation = FixRotation(AngleBtwnChunks(chunks[0], chunks[2], timeStacker));
            bodyLabel.color = sLeaser.sprites[Drawable.BodySprite].color;

            // Head
            headLabel.SetPosition(chunks[4].pos - camPos);
            headLabel.rotation = sLeaser.sprites[Drawable.HeadSprite].rotation;
            headLabel.color = sLeaser.sprites[Drawable.EyesSprite].color;

            // Mask
            maskLabel.isVisible = (Drawable.vulture.State as Vulture.VultureState).mask && !Drawable.IsMiros;
            if (maskLabel.isVisible)
            {
                maskLabel.color = Drawable.vulture.kingTusks != null ? sLeaser.sprites[Drawable.MaskArrowSprite].color : sLeaser.sprites[Drawable.MaskSprite].color;
                maskLabel.SetPosition(chunks[4].pos - camPos);
                maskLabel.rotation = sLeaser.sprites[Drawable.HeadSprite].rotation + 90f;
            }

            // Vulture wings
            for (int i = 0; i < wingLabels.Count; i++)
            {
                var tentacle = tentacles[i];
                var labels = wingLabels[i];
                for (int j = 0; j < labels.Count; j++)
                {
                    var pos = PointAlongTentacle(j, labels.Count, tentacle, timeStacker);
                    labels[j].SetPosition(pos - camPos);
                    labels[j].rotation = FixRotation(AngleBtwn(pos, GetPos(tentacle.connectedChunk, timeStacker)));
                    labels[j].color = HSLColor.Lerp(Drawable.ColorA, Drawable.ColorB, j / (labels.Count - 1f)).rgb;
                }
            }

            // Tusks
            if (Drawable.vulture.kingTusks != null)
            {
                var tusks = Drawable.vulture.kingTusks;
                for (int i = 0; i < Drawable.tusks.Length; i++)
                {
                    tuskLabels[i].SetPosition(AvgVectors(tusks.tusks[i].chunkPoints[0, 0], tusks.tusks[i].chunkPoints[1, 0]) - camPos);
                    tuskLabels[i].rotation = AngleBtwn(tusks.tusks[i].chunkPoints[0, 0], tusks.tusks[i].chunkPoints[1, 0]) + 90f;
                    tuskLabels[i].color = sLeaser.sprites[Drawable.MaskSprite].color;
                    sLeaser.sprites[tusks.tusks[i].LaserSprite(Drawable)].isVisible = true;
                }
            }

            // Miros laser
            if (Drawable.IsMiros)
            {
                sLeaser.sprites[Drawable.LaserSprite()].isVisible = Mathf.Lerp(Drawable.lastLaserActive, Drawable.laserActive, timeStacker) > 0f;
            }
        }
    }
}
