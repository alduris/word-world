using System.Collections.Generic;
using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class VultureWords
    {
        public static FLabel[] Init(VultureGraphics vultureGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            List<FLabel> list = [
                new(Font, PascalRegex.Replace(type.value, Environment.NewLine))
                {
                    scale = vultureGraf.vulture.bodyChunks[0].rad * 4f / FontSize,
                    color = sLeaser.sprites[vultureGraf.BodySprite].color
                },
                new(Font, "Head")
                {
                    scale = vultureGraf.vulture.bodyChunks[4].rad * 4f / FontSize,
                    color = sLeaser.sprites[vultureGraf.EyesSprite].color
                },
                new(Font, "Mask")
                {
                    scale = 17.5f / FontSize * (vultureGraf.IsKing ? 1.15f : 1f)
                }
            ];
            for (int i = 0; i < vultureGraf.vulture.tentacles.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    list.Add(new(Font, "Wing"[j].ToString())
                    {
                        scale = 1.5f,
                        color = HSLColor.Lerp(vultureGraf.ColorA, vultureGraf.ColorB, j / 3f).rgb
                    });
                }
            }
            if (vultureGraf.vulture.kingTusks != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    list.Add(new(Font, "Tusk")
                    {
                        scale = KingTusks.Tusk.length / TextWidth("Tusk"),
                        color = sLeaser.sprites[vultureGraf.MaskSprite].color
                    });
                }
            }
            return [.. list];
        }

        public static void Draw(VultureGraphics vultureGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Labels: 0 -> body, 1 -> mask, [2,2+len(tentacles)*4] -> wings, the rest -> tusks (may not be present)
            var chunks = vultureGraf.vulture.bodyChunks;
            var tentacles = vultureGraf.vulture.tentacles; // vulture wings

            // Body sprite
            labels[0].SetPosition(GetPos(chunks[0], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(chunks[0], chunks[2], timeStacker));
            labels[0].color = sLeaser.sprites[vultureGraf.BodySprite].color;

            // Head
            labels[1].SetPosition(chunks[4].pos - camPos);
            labels[1].rotation = sLeaser.sprites[vultureGraf.HeadSprite].rotation;
            labels[1].color = sLeaser.sprites[vultureGraf.EyesSprite].color;

            // Mask
            labels[2].isVisible = (vultureGraf.vulture.State as Vulture.VultureState).mask && !vultureGraf.IsMiros;
            if (labels[2].isVisible)
            {
                labels[2].color = vultureGraf.vulture.kingTusks != null ? sLeaser.sprites[vultureGraf.MaskArrowSprite].color : sLeaser.sprites[vultureGraf.MaskSprite].color;
                labels[2].SetPosition(chunks[4].pos - camPos);
                labels[2].rotation = sLeaser.sprites[vultureGraf.HeadSprite].rotation + 90f;
            }

            // Vulture wings
            int k = 0;
            for (int i = 3; i < 3 + tentacles.Length * 4; i += 4)
            {
                var tentacle = tentacles[k++];
                for (int j = i; j < i + 4; j++) // 4 letters per wing
                {
                    var pos = PointAlongTentacle(j - i, 4, tentacle, timeStacker);
                    labels[j].SetPosition(pos - camPos);
                    labels[j].rotation = FixRotation(AngleBtwn(pos, GetPos(tentacle.connectedChunk, timeStacker)));
                    labels[j].color = HSLColor.Lerp(vultureGraf.ColorA, vultureGraf.ColorB, (j - i) / 3f).rgb;
                }
            }

            // Tusks
            if (vultureGraf.vulture.kingTusks != null)
            {
                var tusks = vultureGraf.vulture.kingTusks;

                int offset = 3 + tentacles.Length * 4;
                for (int i = 0; i < vultureGraf.tusks.Length; i++)
                {
                    labels[i + offset].SetPosition(AvgVectors(tusks.tusks[i].chunkPoints[0, 0], tusks.tusks[i].chunkPoints[1, 0]) - camPos);
                    labels[i + offset].rotation = AngleBtwn(tusks.tusks[i].chunkPoints[0, 0], tusks.tusks[i].chunkPoints[1, 0]) + 90f;
                    labels[i + offset].color = sLeaser.sprites[vultureGraf.MaskSprite].color;
                    //labels[i + offset].SetPosition(vultureGraf.tusks[i].pos - camPos);
                    //labels[i + offset].rotation = vultureGraf.tuskRotations[i] + (i % 2 == 0 ? -90f : 90f);
                    sLeaser.sprites[tusks.tusks[i].LaserSprite(vultureGraf)].isVisible = true;
                }
            }

            // Miros laser
            if (vultureGraf.IsMiros)
            {
                sLeaser.sprites[vultureGraf.LaserSprite()].isVisible = Mathf.Lerp(vultureGraf.lastLaserActive, vultureGraf.laserActive, timeStacker) > 0f;
            }
        }
    }
}
