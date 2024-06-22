using System;
using System.Collections.Generic;
using System.Linq;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class PlayerWords : CreatureWordify<PlayerGraphics>
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
                Vector3 customCol;
                if (self.useJollyColor)
                {
                    customCol = (Vector3)(Vector4)PlayerGraphics.JollyColor(self.player.playerState.playerNumber, 2);
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

        private List<FLabel> tongueLabels = null;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Main body
            var str = Unpascal(Type);
            if (ModManager.MSC && Type == MoreSlugcatsEnums.CreatureTemplateType.SlugNPC)
                str = "Slugpup";

            labels.Add(new FLabel(Font, str)
            {
                scale = (Drawable.player.bodyChunks.Sum(x => x.rad) + Drawable.player.bodyChunkConnections[0].distance) / TextWidth(str),
            });

            // Tongue
            if (Drawable.player.tongue != null)
            {
                // 6 = len("Tongue")
                tongueLabels = [];
                var tongue = Drawable.player.tongue;
                int length = Math.Max((int)(tongue.idealRopeLength / (FontSize * 0.65f)), 6); // 0.65f is how much we scale it later
                int numOfOs = length - 6;
                
                for (int i = 0; i < length; i++)
                {
                    int k = (i >= 1 && i < 1 + numOfOs) ? 1 : (i < 1 ? i : i - numOfOs);
                    tongueLabels.Add(new(Font, "Tongue"[k].ToString())
                    {
                        scale = 0.65f,
                        color = TongueColor(Drawable, Mathf.Sin(Mathf.InverseLerp(0, length - 1, i) * Mathf.PI))
                    });
                }

                labels.AddRange(tongueLabels);
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Calculate pounce
            bool readyToPounce = Drawable.player.superLaunchJump > 19;
            var offset = readyToPounce ? new Vector2(0f, 3f) : Vector2.zero;

            // Main body
            labels[0].SetPosition(AvgBodyChunkPos(Drawable.player.bodyChunks[0], Drawable.player.bodyChunks[1], timeStacker) - camPos + (readyToPounce ? new Vector2(0, 1.5f) : Vector2.zero));
            labels[0].rotation = AngleBtwn(GetPos(Drawable.player.bodyChunks[0], timeStacker), GetPos(Drawable.player.bodyChunks[1], timeStacker) + offset) + 90f;

            if (ModManager.MSC && Drawable.player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Spear)
            {
                labels[0].color = Color.Lerp(sLeaser.sprites[0].color, new Color(1f, 1f, 1f), Drawable.tailSpecks.spearProg);
            }
            else
            {
                labels[0].color = sLeaser.sprites[0].color;
            }

            // Tongue
            if (tongueLabels != null)
            {
                var tongue = Drawable.player.tongue;
                if (tongue.Free || tongue.Attached)
                {
                    List<Vector2> positions = [GetPos(Drawable.player.bodyChunks[0], timeStacker)];
                    positions.AddRange(tongue.rope.GetAllPositions());
                    for (int i = 0; i < tongueLabels.Count; i++)
                    {
                        // Label needs to be visible (I forgot this line and got confused like 3 times trying to test my code)
                        tongueLabels[i].isVisible = true;

                        // Lerp along tongue
                        var x = Mathf.InverseLerp(tongueLabels.Count - 1, 0, i);
                        tongueLabels[i].SetPosition(PointAlongVectors(x, positions) - camPos);
                    }
                }
                else
                {
                    foreach (var label in tongueLabels)
                    {
                        label.isVisible = false;
                    }
                }
            }

            // The mark
            sLeaser.sprites[10].isVisible = true;
            sLeaser.sprites[11].isVisible = true;

            // Saint stuff
            if (Drawable.player.room != null && ModManager.MSC && Drawable.player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {
                // no clue if this works, this is based on stuff in PlayerGraphics.DrawSprites :monksilly:
                sLeaser.sprites[14].isVisible = true;
                sLeaser.sprites[13].isVisible = Drawable.player.killFac > 0f || Drawable.player.forceBurst;
                sLeaser.sprites[15 + Drawable.numGodPips + Drawable.tentacles.Length * 2].isVisible = Drawable.darkenFactor > 0f;
                for (int i = 15; i < 15 + Drawable.numGodPips; i++)
                {
                    sLeaser.sprites[i].isVisible = true;
                }
                for (int i = 0; i < Drawable.tentacles.Length; i++)
                {
                    sLeaser.sprites[Drawable.tentacles[i].startSprite].isVisible = i < Drawable.tentaclesVisible;
                    sLeaser.sprites[Drawable.tentacles[i].startSprite + 1].isVisible = i < Drawable.tentaclesVisible;
                }
            }
        }
    }
}
