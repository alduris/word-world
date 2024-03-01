using System;
using System.Collections.Generic;
using System.Linq;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class PlayerWords
    {
        private static Color TongueColor(PlayerGraphics self, float x)
        {
            float h1 = 0.95f;
            float h2 = 1f;
            float s = 1f;
            float l1 = 0.75f;
            float l2 = 0.9f;
            if (PlayerGraphics.CustomColorsEnabled() || self.useJollyColor)
            {
                Vector3 customCol = Vector3.one;
                if (self.useJollyColor)
                {
                    Color color3 = PlayerGraphics.JollyColor(self.player.playerState.playerNumber, 2);
                    customCol = new Vector3(color3.r, color3.g, color3.b);
                }
                else
                {
                    customCol = Custom.RGB2HSL(PlayerGraphics.CustomColorSafety(2));
                }
                if (customCol.x > 0.95)
                {
                    h2 = customCol.x;
                    h1 = customCol.x - 0.05f;
                }
                else
                {
                    h1 = customCol.x;
                    h2 = customCol.x + 0.05f;
                }
                s = customCol.y;
                if (customCol.z > 0.85)
                {
                    l2 = customCol.z;
                    l1 = customCol.z - 0.15f;
                }
                else
                {
                    l1 = customCol.z;
                    l2 = customCol.z + 0.15f;
                }
            }
            return Custom.HSL2RGB(Mathf.Lerp(h1, h2, x), s, Mathf.Lerp(l1, l2, Mathf.Pow(x, 0.15f))); // actual tongue is lerped (fog color, tongue color, 0.7f) but too bad
        }

        public static FLabel[] Init(PlayerGraphics playerGraf, CreatureTemplate.Type type)
        {
            // Main body
            var str = Unpascal(type);
            if (ModManager.MSC && type == MoreSlugcatsEnums.CreatureTemplateType.SlugNPC)
                str = "Slugpup";

            List<FLabel> labels = [new FLabel(Font, str)
            {
                scale = (playerGraf.player.bodyChunks.Sum(x => x.rad) + playerGraf.player.bodyChunkConnections[0].distance) / TextWidth(str),
                color = playerGraf.player.isNPC ? playerGraf.player.ShortCutColor() : PlayerGraphics.SlugcatColor(playerGraf.CharacterForColor)
            }];

            // Tongue
            if (playerGraf.player.tongue != null)
            {
                // 6 = len("Tongue")
                var tongue = playerGraf.player.tongue;
                int length = Math.Max((int)(tongue.idealRopeLength / (FontSize * 0.65f)), 6); // 0.65f is how much we scale it later
                int numOfOs = length - 6;
                
                for (int i = length - 1; i >= 0; i--)
                {
                    int k = (i >= 1 && i < numOfOs) ? 1 : (i < 1 ? i : i - numOfOs);
                    labels.Add(new(Font, "Tongue"[k].ToString())
                    {
                        scale = 0.65f,
                        color = TongueColor(playerGraf, Mathf.Sin(Mathf.InverseLerp(0, length - 1, i) * Mathf.PI))
                    });
                }
            }

            // Return
            return [.. labels];
        }

        public static void Draw(PlayerGraphics playerGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main body
            labels[0].SetPosition(AvgBodyChunkPos(playerGraf.player.bodyChunks[0], playerGraf.player.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(playerGraf.player.bodyChunks[0], playerGraf.player.bodyChunks[1], timeStacker) + 90f;
            labels[0].color = sLeaser.sprites[0].color; // playerGraf.player.isNPC ? playerGraf.player.ShortCutColor() : PlayerGraphics.SlugcatColor(playerGraf.CharacterForColor);
            if (ModManager.MSC)
            {
                if (playerGraf.player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Spear)
                {
                    labels[0].color = Color.Lerp(labels[0].color, new Color(1, 1, 1), Mathf.InverseLerp(0, 110, playerGraf.player.swallowAndRegurgitateCounter));
                }
            }

            // Tongue
            if (playerGraf.player.tongue != null)
            {
                var tongue = playerGraf.player.tongue;
                if (tongue.Free || tongue.Attached)
                {
                    List<Vector2> positions = [GetPos(playerGraf.player.bodyChunks[0], timeStacker)];
                    // positions.AddRange(playerGraf.ropeSegments.Select(s => Vector2.Lerp(s.lastPos, s.pos, timeStacker)));
                    positions.AddRange(tongue.rope.GetAllPositions());
                    for (int i = 1; i < labels.Length; i++)
                    {
                        // Label needs to be visible (I forgot this line and got confused like 3 times trying to test my code)
                        labels[i].isVisible = true;

                        // Lerp along tongue
                        var x = Mathf.InverseLerp(1, labels.Length - 1, i);
                        labels[i].SetPosition(PointAlongVectors(x, positions) - camPos);
                    }
                }
                else
                {
                    for (int i = 1; i < labels.Length; i++)
                    {
                        labels[i].isVisible = false;
                    }
                }
            }

            // The mark
            sLeaser.sprites[10].isVisible = true;
            sLeaser.sprites[11].isVisible = true;

            // Saint stuff
            if (playerGraf.player.room != null && ModManager.MSC && playerGraf.player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                // no clue if this works, this is based on stuff in PlayerGraphics.DrawSprites :monksilly:
                sLeaser.sprites[14].isVisible = true;
                sLeaser.sprites[13].isVisible = playerGraf.player.killFac > 0f || playerGraf.player.forceBurst;
                sLeaser.sprites[15 + playerGraf.numGodPips + playerGraf.tentacles.Length * 2].isVisible = playerGraf.darkenFactor > 0f;
                for (int i = 15; i < 15 + playerGraf.numGodPips; i++)
                {
                    sLeaser.sprites[i].isVisible = true;
                }
                for (int i = 0; i < playerGraf.tentacles.Length; i++)
                {
                    sLeaser.sprites[playerGraf.tentacles[i].startSprite].isVisible = i < playerGraf.tentaclesVisible;
                    sLeaser.sprites[playerGraf.tentacles[i].startSprite + 1].isVisible = i < playerGraf.tentaclesVisible;
                }
            }
        }
    }
}
