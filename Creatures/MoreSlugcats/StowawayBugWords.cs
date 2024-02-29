using System.Collections.Generic;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.MoreSlugcats
{
    public static class StowawayBugWords
    {
        public static FLabel[] Init(StowawayBugGraphics stowawayGraf, RoomCamera.SpriteLeaser sLeaser)
        {
            // Main body
            List<FLabel> labels = [
                new(Font, "Stowaway") {
                    scale = (stowawayGraf.myBug.bodyChunks[0].rad + stowawayGraf.myBug.bodyChunks[1].rad) * 2f / FontSize,
                    color = stowawayGraf.bodyColor
                }
            ];

            // Tentacles
            for (int i = 0; i < stowawayGraf.myBug.heads.Length; i++)
            {
                foreach (var c in "Tentacle")
                {
                    labels.Add(
                        new(Font, c.ToString()) {
                            scale = 1.125f,
                            color = sLeaser.sprites[stowawayGraf.SpritesBegin_Mouth].color
                        }
                    );
                }
            }

            return [.. labels];
        }

        public static void Draw(StowawayBugGraphics stowawayGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main body
            labels[0].SetPosition(GetPos(stowawayGraf.myBug.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[1].rotation);

            // Tentacles
            for (int i = 0; i < stowawayGraf.myBug.heads.Length; i++)
            {
                var head = stowawayGraf.myBug.heads[i];
                for (int j = 0; j < 8; j++)
                {
                    var label = labels[i * 8 + j + 1];
                    var pos = PointAlongTentacle(j + 1, 9, head, timeStacker);
                    var prevPos = PointAlongTentacle(j, 9, head, timeStacker);
                    label.SetPosition(pos - camPos);
                    label.rotation = AngleBtwn(pos, prevPos);
                    label.isVisible = stowawayGraf.myBug.headFired[i];
                }
            }
        }
    }
}
