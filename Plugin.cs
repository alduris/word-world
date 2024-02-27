using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using RWCustom;
using MoreSlugcats;
using UnityEngine;
using SpriteLeaser = RoomCamera.SpriteLeaser;
using Random = UnityEngine.Random;
using static WordWorld.WordUtil;

#pragma warning disable CS0618
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace WordWorld
{

    [BepInPlugin("alduris.wordworld", "Word World", "1.0.0")]
    internal class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        public Plugin()
        {
            try
            {
                Logger = base.Logger;
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                throw;
            }
        }

        internal static bool DoThings = true;
        internal static bool ShowSprites = false;
        internal static bool YeekFix = false;
        internal static bool ClownLongLegs = false;

#pragma warning disable IDE0051 // Remove unused private members
        private void OnEnable()
#pragma warning restore IDE0051 // Remove unused private members
        {
            Logger.LogInfo("Hooking");
            try
            {
                // Important hooks
                On.RoomCamera.SpriteLeaser.ctor += SpriteLeaser_ctor;
                On.RoomCamera.SpriteLeaser.Update += SpriteLeaser_Update;
                On.RoomCamera.SpriteLeaser.CleanSpritesAndRemove += SpriteLeaser_CleanSpritesAndRemove;

                // This is literally only here for testing stuff
                On.RainWorldGame.Update += RainWorldGame_Update;

                // Mod compatibility
                On.RainWorld.OnModsInit += RainWorld_OnModsInit;

                Logger.LogInfo("Success");
            }
            catch (Exception e)
            {
                Logger.LogError("Ran into error hooking");
                Logger.LogError(e);
            }
        }

        private static void RainWorldGame_Update(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            orig(self);
            if (self.devToolsActive && self.IsArenaSession && Input.GetKeyDown(KeyCode.Backslash))
            {
                ShowSprites = !ShowSprites;
            }
        }

        private static void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            YeekFix = ModManager.ActiveMods.Exists(mod => mod.id == "vigaro.yeekfix");
            ClownLongLegs = ModManager.ActiveMods.Exists(mod => mod.id == "clownlonglegs");
        }

        private static void SpriteLeaser_ctor(On.RoomCamera.SpriteLeaser.orig_ctor orig, SpriteLeaser self, IDrawable obj, RoomCamera rCam)
        {
            orig(self, obj, rCam);

            var labels = CWTs.GetLabels(self);
            if (labels == null || !DoThings) return;

            try
            {
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs))
                {
                    // Deal with API stuff
                    funcs.Item2.Invoke(obj, labels);
                }
                else
                {
                    // Built-in stuff
                    switch (obj)
                    {
                        case BigEelGraphics bigEelGraf:
                            {
                                var scale = bigEelGraf.eel.bodyChunks.Max(c => c.rad) * 2.5f / FontSize;
                                var colorA = bigEelGraf.eel.iVars.patternColorA;
                                var colorB = bigEelGraf.eel.iVars.patternColorB;
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    var label = labels[i];
                                    label.scale = scale;
                                    label.color = HSLColor.Lerp(colorA, colorB, i / (float)(labels.Length - 1)).rgb;
                                }
                                break;
                            }
                        case BigSpiderGraphics bigSpiderGraf:
                            {
                                labels[0].scale = (bigSpiderGraf.bug.bodyChunks[0].rad + bigSpiderGraf.bug.bodyChunks[1].rad + bigSpiderGraf.bug.bodyChunkConnections[0].distance) * 1.5f / TextWidth(labels[0].text);
                                if (bigSpiderGraf.bug.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.BigSpider)
                                    labels[0].scale *= 2.5f / 1.5f;
                                labels[0].color = bigSpiderGraf.yellowCol;
                                break;
                            }
                        case CentipedeGraphics centiGraf:
                            {
                                bool fit = labels.Length > centiGraf.centipede.bodyChunks.Length;
                                float scale = centiGraf.centipede.bodyChunks.Max(c => c.rad) * 3f / FontSize;
                                if (fit) scale *= (float)centiGraf.centipede.bodyChunks.Length / labels.Length;
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].scale = scale;
                                    labels[i].color = centiGraf.ShellColor;
                                }
                                break;
                            }
                        case CicadaGraphics cicadaGraf:
                            {
                                var bodySprite = self.sprites[cicadaGraf.BodySprite];
                                labels[0].scale = bodySprite.element.sourcePixelSize.y / TextWidth(labels[0].text);
                                labels[0].color = bodySprite.color;
                                break;
                            }
                        case DaddyGraphics daddyGraf:
                            {
                                // Colors
                                Random.State state = Random.state;
                                Random.InitState(daddyGraf.daddy.abstractCreature.ID.RandomSeed);
                                Color blinkColor = ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.5f) : daddyGraf.daddy.eyeColor;
                                Color bodyColor = ClownLongLegs ? Color.white : daddyGraf.blackColor;

                                // Main body chunk
                                labels[0].scale = Mathf.Sqrt(daddyGraf.daddy.bodyChunks.Length) * daddyGraf.daddy.bodyChunks.Average(c => c.rad) * 2f / FontSize;
                                labels[0].color = blinkColor;

                                // Tentacles
                                var tentacles = daddyGraf.daddy.tentacles;
                                int k = 1;

                                for (int i = 0; i < tentacles.Length; i++)
                                {
                                    var tentacle = tentacles[i];
                                    int length = (int)(tentacle.idealLength / 20f);
                                    Color tipColor = ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.625f) : daddyGraf.daddy.eyeColor;
                                    for (int j = 0; j < length; j++, k++)
                                    {
                                        labels[k].scale = 1.5f;
                                        labels[k].color = Color.Lerp(bodyColor, tipColor, Custom.LerpMap(j, 0, length, 0f, 1f, 1.5f));
                                    }
                                }
                                Random.state = state;
                                break;
                            }
                        case DeerGraphics deerGraf:
                            {
                                labels[0].scale = (deerGraf.deer.bodyChunks[1].rad + deerGraf.deer.bodyChunks[2].rad + deerGraf.deer.bodyChunks[3].rad + deerGraf.deer.bodyChunks[4].rad) / 2f / FontSize;
                                labels[0].color = deerGraf.bodyColor;
                                labels[1].scale = deerGraf.deer.bodyChunks[5].rad / FontSize;
                                labels[1].color = deerGraf.bodyColor;
                                break;
                            }
                        case DropBugGraphics dropBugGraf:
                            {
                                labels[0].scale = dropBugGraf.bug.mainBodyChunk.rad * 3f / FontSize;
                                break;
                            }
                        case EggBugGraphics eggBugGraf:
                            {
                                // Labels: 0 -> body, everything else -> eggs
                                labels[0].scale = (eggBugGraf.bug.bodyChunks[0].rad + eggBugGraf.bug.bodyChunks[1].rad + eggBugGraf.bug.bodyChunkConnections[0].distance) * 1.75f / TextWidth(labels[0].text);
                                labels[0].color = eggBugGraf.blackColor;
                                for (int i = 0; i < 2; i++)
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        int k = i * 3 + j + 1;
                                        labels[k].scale = eggBugGraf.eggs[i, j].rad * 3f / LabelTest.GetWidth(labels[k].text, false);
                                        labels[k].color = eggBugGraf.eggColors[1];
                                    }
                                }
                                break;
                            }
                        case FlyGraphics flyGraf:
                            {
                                labels[0].scale = flyGraf.lowerBody.rad * 4f / FontSize;
                                labels[0].color = rCam.currentPalette.blackColor;
                                break;
                            }
                        case GarbageWormGraphics gWormGraf:
                            {
                                foreach (var label in labels)
                                {
                                    label.scale = 1.25f;
                                    label.color = rCam.currentPalette.blackColor;
                                }
                                break;
                            }
                        case HazerGraphics hazerGraf:
                            {
                                labels[0].scale = hazerGraf.bug.mainBodyChunk.rad * 2f / FontSize;
                                break;
                            }
                        case JetFishGraphics jetfishGraf:
                            {
                                labels[0].scale = jetfishGraf.fish.bodyChunks[0].rad * 2.5f / FontSize;
                                labels[0].color = self.sprites[jetfishGraf.BodySprite].color;
                                break;
                            }
                        case LeechGraphics leechGraf:
                            {
                                labels[0].scale = leechGraf.leech.mainBodyChunk.rad * 4f / FontSize;
                                break;
                            }
                        case LizardGraphics lizGraf:
                            {
                                labels[0].scale = (lizGraf.lizard.bodyChunks[0].rad + 2f * lizGraf.lizard.bodyChunks[2].rad + lizGraf.lizard.bodyChunks[2].rad + lizGraf.lizard.bodyChunkConnections[0].distance + lizGraf.lizard.bodyChunkConnections[1].distance) / TextWidth(labels[0].text);
                                if (lizGraf.tongue != null)
                                {
                                    for (int i = 1; i <= 6; i++) // 6 letters in "tongue"
                                    {
                                        labels[i].scale = 0.875f;
                                        labels[i].color = rCam.currentPalette.blackColor;
                                    }
                                }
                                break;
                            }
                        case MirosBirdGraphics mirosGraf:
                            {
                                labels[0].scale = mirosGraf.bird.mainBodyChunk.rad * 2f / FontSize;
                                labels[0].color = rCam.currentPalette.blackColor;
                                labels[1].scale = mirosGraf.bird.Head.rad * 2f / FontSize;
                                labels[1].color = mirosGraf.EyeColor;
                                break;
                            }
                        case MoreSlugcats.InspectorGraphics inspGraf:
                            {
                                // Inspectors' main body chunks are teeny tiny little things (1/4 of a tile width in diameter, not radius)
                                // Therefore, using it would be a bad idea (can barely see the label) so instead I hardcode it
                                labels[0].scale = 1.5f; // inspGraf.myInspector.bodyChunks[0].rad * 2f / FontSize;
                                for (int i = 0; i < inspGraf.myInspector.heads.Length; i++)
                                {
                                    labels[i + 1].scale = 1.125f;
                                }
                                break;
                            }
                        case MoreSlugcats.StowawayBugGraphics stowawayGraf:
                            {
                                // Main body
                                labels[0].scale = (stowawayGraf.myBug.bodyChunks[0].rad + stowawayGraf.myBug.bodyChunks[1].rad) * 2f / FontSize;
                                labels[0].color = stowawayGraf.bodyColor;

                                // Tentacles
                                for (int i = 0; i < stowawayGraf.myBug.heads.Length; i++)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        var label = labels[i * 8 + j + 1];
                                        label.scale = 1.125f;
                                        label.color = self.sprites[stowawayGraf.SpritesBegin_Mouth].color;
                                    }
                                }
                                break;
                            }
                        case MoreSlugcats.YeekGraphics yeekGraf:
                            {
                                labels[0].scale = yeekGraf.myYeek.bodyChunks.Sum(c => c.rad) * 2f / TextWidth(labels[0].text);
                                labels[0].color = yeekGraf.tailHighlightColor;
                                break;
                            }
                        case MouseGraphics mouseGraf:
                            {
                                labels[0].scale = (mouseGraf.mouse.bodyChunks.Sum(x => x.rad) + mouseGraf.mouse.bodyChunkConnections[0].distance) / TextWidth(labels[0].text);
                                break;
                            }
                        case NeedleWormGraphics nootGraf:
                            {
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].scale = nootGraf.worm.OnBodyRad(0) * 8f / FontSize;
                                }
                                break;
                            }
                        case OverseerGraphics overseerGraf:
                            {
                                labels[0].color = overseerGraf.MainColor;
                                break;
                            }
                        case PlayerGraphics playerGraf:
                            {
                                labels[0].scale = (playerGraf.player.bodyChunks.Sum(x => x.rad) + playerGraf.player.bodyChunkConnections[0].distance) / TextWidth(labels[0].text);
                                labels[0].color = playerGraf.player.isNPC ? playerGraf.player.ShortCutColor() : PlayerGraphics.SlugcatColor(playerGraf.CharacterForColor);
                                break;
                            }
                        case PoleMimicGraphics poleMimicGraf:
                            {
                                foreach (var label in labels)
                                {
                                    label.scale = 1.5f;
                                }
                                break;
                            }
                        case ScavengerGraphics scavGraf:
                            {
                                labels[0].scale = (scavGraf.scavenger.bodyChunks[0].rad + scavGraf.scavenger.bodyChunks[1].rad) * 3f / TextWidth(labels[0].text);
                                labels[0].color = scavGraf.bodyColor.rgb;
                                labels[1].color = scavGraf.eyeColor.rgb;
                                break;
                            }
                        case SnailGraphics snailGraf:
                            {
                                var snailWidth = snailGraf.snail.bodyChunks.Sum(x => x.rad) + snailGraf.snail.bodyChunkConnections.Sum(x => x.distance);
                                var words = CWTs.pascalRegex.Split(snailGraf.snail.abstractCreature.creatureTemplate.type.value).Where(x => x.Length > 0).ToArray();
                                var maxWidth = words.Max(TextWidth);
                                var fontSize = snailWidth * 2f / maxWidth;
                                int j = 0;
                                foreach (var word in words)
                                {
                                    if (word.Length == 0) continue;
                                    var thisWidth = TextWidth(word);
                                    var invlerpMax = thisWidth - TextWidth(word[word.Length - 1].ToString());
                                    for (int i = 0; i < word.Length; i++, j++)
                                    {
                                        var blendAmt = Mathf.InverseLerp(0f, invlerpMax, (i == 0 ? 0f : TextWidth(word.Substring(0, i))) + (maxWidth - thisWidth));
                                        var colorBlend = Color.Lerp(snailGraf.snail.shellColor[0], snailGraf.snail.shellColor[1], blendAmt);
                                        labels[j].scale = fontSize;
                                        labels[j].color = colorBlend;
                                    }
                                }
                                break;
                            }
                        case SpiderGraphics spiderGraf:
                            {
                                labels[0].scale = spiderGraf.spider.firstChunk.rad * 4f / TextWidth(labels[0].text);
                                labels[0].color = spiderGraf.blackColor;
                                break;
                            }
                        case TempleGuardGraphics guardGraf:
                            {
                                labels[0].scale = self.sprites[guardGraf.HeadSprite].element.sourcePixelSize.x / TextWidth(labels[0].text);
                                labels[0].color = RainWorld.SaturatedGold;
                                break;
                            }
                        case TentaclePlantGraphics kelpGraf:
                            {
                                break;
                            }
                        case TubeWormGraphics wormGraf:
                            {
                                break;
                            }
                        case VultureGraphics vultureGraf:
                            {
                                // Labels: 0 -> body, 1 -> mask, [2,2+len(tentacles)*4] -> wings, the rest -> tusks (may not be present)

                                var tentacles = vultureGraf.vulture.tentacles; // vulture wings
                                labels[0].scale = vultureGraf.vulture.bodyChunks[0].rad * 4f / FontSize; // 4f because 2 chunks and 2*radius=diameter
                                labels[0].color = self.sprites[vultureGraf.BodySprite].color;
                                labels[1].scale = vultureGraf.vulture.bodyChunks[4].rad * 4f / FontSize;
                                labels[1].color = self.sprites[vultureGraf.MaskSprite].color;

                                // Vulture wings
                                for (int i = 0; i < tentacles.Length; i++)
                                {
                                    var tentacle = tentacles[i];
                                    for (int j = 0; j < 4; j++)
                                    {
                                        var label = labels[i * 4 + j + 2];
                                        label.scale = 1.5f;
                                        label.color = HSLColor.Lerp(vultureGraf.ColorA, vultureGraf.ColorB, j / 3f).rgb;
                                    }
                                }

                                // Tusks
                                if (vultureGraf.vulture.kingTusks != null)
                                {
                                    for (int i = 0; i < vultureGraf.tusks.Length; i++)
                                    {
                                        int j = 2 + tentacles.Length * 4 + i;
                                        labels[j].scale = KingTusks.Tusk.length / TextWidth(labels[j].text);
                                        labels[j].color = self.sprites[vultureGraf.MaskSprite].color;
                                    }
                                }
                                break;
                            }
                        case VultureGrubGraphics grubGraf:
                            {
                                break;
                            }

                        // Things that are not Creatures
                        case OracleGraphics oracleGraf:
                            {
                                break;
                            }
                        case VoidSpawnGraphics voidSpawnGraphics:
                            {
                                break;
                            }

                        case JellyFish jelly:
                            {
                                break;
                            }
                        case BigJellyFish bigJelly:
                            {
                                break;
                            }

                        // Default cases
                        case GraphicsModule module:
                            {
                                var label = labels[0];

                                // Yeek fix compatibility
                                if (YeekFix && module.GetType().Name == "FixedYeekGraphics")
                                {
                                    // My incredibly sketchy solution to not requiring yeek fix in build or game
                                    // Yeek fix outright replaces yeeks with a class of its own type
                                    int bodySpritesStart = (int)module.GetType().GetMethod("get_BodySpritesStart").Invoke(module, []);
                                    label.scale = module.owner.bodyChunks[0].rad * 4f / TextWidth(labels[0].text);
                                    TriangleMesh bodySprite = self.sprites[bodySpritesStart] as TriangleMesh;
                                    int numVerts = bodySprite.verticeColors.Length;
                                    label.color = bodySprite.verticeColors[numVerts - 15];
                                    break;
                                }

                                // Creature should be single thingy
                                label.scale = module.owner.bodyChunks.Max(chunk => chunk.rad) * 3f / TextWidth(label.text);
                                label.color = self.sprites[0].color;
                                break;
                            }
                        default: { break; } // do nothing
                    }
                }

                // Assign colors and container
                // NOTE: items with * need to be updated at runtime
                /*Color colorA = module switch {
                    BigEelGraphics bigEelGraf => bigEelGraf.eel.iVars.patternColorA.rgb,
                    BigSpiderGraphics bigSpiderGraf => bigSpiderGraf.yellowCol,
                    CentipedeGraphics centiGraf => centiGraf.ShellColor, // * (maybe)
                    CicadaGraphics cicadaGraf => cicadaGraf.shieldColor,
                    DaddyGraphics daddyGraf => daddyGraf.EffectColor, // * (probably)
                    DeerGraphics deerGraf => deerGraf.bodyColor,
                    DropBugGraphics dropBugGraf => dropBugGraf.currSkinColor,
                    EggBugGraphics eggBugGraf => eggBugGraf.blackColor,
                    HazerGraphics hazerGraf => self.sprites[hazerGraf.BodySprite].color, // *
                    LeechGraphics => self.sprites[0].color, // *
                    LizardGraphics lizGraf => lizGraf.HeadColor(0), // *
                    MirosBirdGraphics mirosGraf => mirosGraf.EyeColor, // *
                    MoreSlugcats.InspectorGraphics inspGraf => inspGraf.myInspector.bodyColor, // *
                    MoreSlugcats.StowawayBugGraphics stowawayGraf => stowawayGraf.bodyColor,
                    MoreSlugcats.YeekGraphics yeekGraf => yeekGraf.HeadfurColor,
                    MouseGraphics mouseGraf => mouseGraf.BodyColor, // *
                    NeedleWormGraphics nootGraf => nootGraf.bodyColor,
                    OverseerGraphics overseerGraf => overseerGraf.MainColor,
                    PlayerGraphics playerGraf => playerGraf.player.isNPC ? playerGraf.player.ShortCutColor() : PlayerGraphics.SlugcatColor(playerGraf.CharacterForColor),
                    PoleMimicGraphics poleMimicGraf => poleMimicGraf.mimicColor,
                    ScavengerGraphics scavGraf => scavGraf.bodyColor.rgb,
                    SnailGraphics snailGraf => snailGraf.snail.shellColor[0],
                    SpiderGraphics spiderGraf => spiderGraf.blackColor,
                    TempleGuardGraphics => RainWorld.SaturatedGold,
                    TentaclePlantGraphics kelpGraf => UndoColorLerp(self.sprites[1].color, rCam.currentPalette.blackColor, rCam.room.Darkness(kelpGraf.plant.rootPos)),
                    TubeWormGraphics wormGraf => wormGraf.color,
                    VultureGraphics vultureGraf => vultureGraf.ColorA.rgb,
                    VultureGrubGraphics grubGraf => self.sprites[grubGraf.MeshSprite].color, // * (?)
                    OracleGraphics oracleGraf => self.sprites[oracleGraf.HeadSprite].color,
                    FlyGraphics or GarbageWormGraphics or JetFishGraphics => rCam.currentPalette.blackColor,
                    _ => self.sprites[0].color
                };
                Color? colorB = module switch {
                    BigEelGraphics beg => beg.eel.iVars.patternColorB.rgb,
                    SnailGraphics snailGraf => snailGraf.snail.shellColor[1],
                    VultureGraphics vultureGraf => vultureGraf.ColorB.rgb,
                    OracleGraphics oracleGraf => oracleGraf.GenericJointBaseColor(),
                    _ => null
                };*/
                for (int i = 0; i < labels.Length; i++)
                {
                    var label = labels[i];
                    label.alignment = FLabelAlignment.Center;
                    // label.color = colorB != null ? Color.LerpUnclamped(colorA, (Color)colorB, (float)i / (labels.Length - 1)) : colorA;
                    (self.sprites[0].container ?? rCam.ReturnFContainer("Midground")).AddChild(label);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Ran into error in SpriteLeaser.ctor, disabling hook effects");
                Logger.LogError(e);
                DoThings = false;
            }
        }
        private static void SpriteLeaser_Update(On.RoomCamera.SpriteLeaser.orig_Update orig, SpriteLeaser self, float timeStacker, RoomCamera rCam, Vector2 camPos)
        {
            orig(self, timeStacker, rCam, camPos);

            var labels = CWTs.GetLabels(self);
            if (labels == null || !DoThings) return;
            var obj = self.drawableObject;

            if (!ShowSprites)
            {
                foreach (var sprite in self.sprites)
                {
                    sprite.isVisible = false;
                }
            }

            try
            {
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs))
                {
                    // Deal with API stuff
                    funcs.Item3.Invoke(obj, labels, self, timeStacker, camPos);
                }
                else
                {
                    // Built-in stuff
                    switch (obj)
                    {
                        case BigEelGraphics bigEelGraf:
                            {
                                // Text says "Leviathan" but split into individual chars
                                var chunks = bigEelGraf.eel.bodyChunks;
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].SetPosition(PointAlongChunks(i, labels.Length, chunks, timeStacker) - camPos);
                                    labels[i].rotation = RotationAlongSprites(i, labels.Length, chunks.Length, self.sprites, bigEelGraf.BodyChunksSprite);
                                }
                                break;
                            }
                        case BigSpiderGraphics bigSpiderGraf:
                            {
                                var label = labels[0];
                                var pos = GetPos(bigSpiderGraf.bug.bodyChunks[1], timeStacker);
                                label.SetPosition(pos - camPos);
                                label.rotation = FixRotation(self.sprites[bigSpiderGraf.HeadSprite].rotation) + 90f;
                                break;
                            }
                        case CentipedeGraphics centiGraf:
                            {
                                var chunks = centiGraf.centipede.bodyChunks;
                                bool fit = labels.Length > chunks.Length;
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    if (fit)
                                    {
                                        labels[i].SetPosition(PointAlongChunks(i, labels.Length, chunks, timeStacker) - camPos);
                                        labels[i].rotation = RotationAlongSprites(i, labels.Length, chunks.Length, self.sprites, x => x);
                                    }
                                    else
                                    {
                                        labels[i].SetPosition(chunks[i].pos - camPos);
                                        labels[i].rotation = self.sprites[i].rotation;
                                    }
                                }
                                break;
                            }
                        case CicadaGraphics cicadaGraf:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(cicadaGraf.cicada.bodyChunks[0], cicadaGraf.cicada.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(cicadaGraf.cicada.bodyChunks[0], cicadaGraf.cicada.bodyChunks[1], timeStacker);
                                break;
                            }
                        case DaddyGraphics daddyGraf:
                            {
                                // Colors n stuff
                                Random.State state = Random.state;
                                Random.InitState(daddyGraf.daddy.abstractCreature.ID.RandomSeed);
                                Color eyeColor = ClownLongLegs ? Custom.HSL2RGB(Random.value, 1f, 0.5f) : daddyGraf.daddy.eyeColor;
                                Color bodyColor = ClownLongLegs ? Color.white : daddyGraf.blackColor;

                                // Main body chunk
                                labels[0].SetPosition(daddyGraf.daddy.MiddleOfBody - camPos);
                                labels[0].color = Color.LerpUnclamped(eyeColor, bodyColor, Mathf.Lerp(daddyGraf.eyes[0].lastClosed, daddyGraf.eyes[0].closed, timeStacker));

                                // Tentacles
                                var tentacles = daddyGraf.daddy.tentacles;
                                int k = 1;
                                for (int i = 0; i < tentacles.Length; i++)
                                {
                                    // len("Tentacle") = 8
                                    var tentacle = tentacles[i];
                                    int length = (int)(tentacle.idealLength / 20f);
                                    for (int j = 0; j < length; j++, k++)
                                    {
                                        // Offset position by 1 to move away from center a bit
                                        var pos = PointAlongRope(j + 1, length + 1, daddyGraf.legGraphics[i], timeStacker);
                                        var prevPos = PointAlongRope(j, length + 1, daddyGraf.legGraphics[i], timeStacker);
                                        labels[k].SetPosition(pos - camPos);
                                        labels[k].rotation = AngleBtwn(pos, prevPos);
                                    }
                                }
                                Random.state = state;
                                break;
                            }
                        case DeerGraphics deerGraf:
                            {
                                // Labels: 0 -> body, 1 -> antlers
                                labels[0].SetPosition(
                                    AvgVectors(
                                        AvgBodyChunkPos(deerGraf.deer.bodyChunks[1], deerGraf.deer.bodyChunks[2], timeStacker),
                                        AvgBodyChunkPos(deerGraf.deer.bodyChunks[3], deerGraf.deer.bodyChunks[4], timeStacker)
                                    ) - camPos
                                );
                                labels[1].SetPosition(GetPos(deerGraf.deer.bodyChunks[5], timeStacker));
                                labels[1].rotation = Custom.VecToDeg(deerGraf.deer.HeadDir);
                                break;
                            }
                        case DropBugGraphics dropBugGraf:
                            {
                                labels[0].SetPosition(GetPos(dropBugGraf.bug.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = FixRotation(self.sprites[dropBugGraf.HeadSprite].rotation) + 90f;
                                labels[0].color = dropBugGraf.currSkinColor;
                                break;
                            }
                        case EggBugGraphics eggBugGraf:
                            {
                                // Labels: 0 -> body, everything else -> eggs
                                labels[0].SetPosition(eggBugGraf.bug.bodyChunks[1].pos - camPos);
                                labels[0].rotation = FixRotation(self.sprites[eggBugGraf.HeadSprite].rotation) + 90f;
                                for (int i = 0; i < 6; i++)
                                {
                                    var eggSprite = self.sprites[eggBugGraf.BackEggSprite(i % 2, i / 2, 2)]; // todo: verify part
                                                                                                             // var egg = eggBugGraf.eggs[i / 2, i % 2];
                                    labels[i + 1].x = eggSprite.x;
                                    labels[i + 1].y = eggSprite.y;
                                    labels[i + 1].rotation = eggSprite.rotation;
                                    if (eggBugGraf.bug.FireBug && i < eggBugGraf.bug.eggsLeft) labels[i + 1].isVisible = false;
                                }
                                break;
                            }
                        case FlyGraphics flyGraf:
                            {
                                labels[0].SetPosition(GetPos(flyGraf.lowerBody, timeStacker) - camPos);
                                labels[0].rotation = self.sprites[0].rotation;
                                break;
                            }
                        case GarbageWormGraphics gWormGraf:
                            {
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].SetPosition(PointAlongTentacle(labels.Length - i - 1, labels.Length, gWormGraf.worm.tentacle, timeStacker) - camPos);
                                    labels[i].isVisible = gWormGraf.worm.extended > 0f;
                                }
                                labels[0].color = self.sprites[0].color;
                                break;
                            }
                        case HazerGraphics hazerGraf:
                            {
                                FSprite bodySprite = self.sprites[hazerGraf.BodySprite];
                                labels[0].SetPosition(bodySprite.GetPosition());
                                labels[0].rotation = FixRotation(bodySprite.rotation);
                                labels[0].color = bodySprite.color;
                                break;
                            }
                        case JetFishGraphics jetfishGraf:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(jetfishGraf.fish.bodyChunks[0], jetfishGraf.fish.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = FixRotation(self.sprites[jetfishGraf.BodySprite].rotation + 90f);
                                break;
                            }
                        case LeechGraphics leechGraf:
                            {
                                labels[0].color = self.sprites[0].color;
                                labels[0].SetPosition(GetPos(leechGraf.leech.mainBodyChunk, timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnParts(leechGraf.body[0], leechGraf.body[leechGraf.body.Length - 1], timeStacker) + 90f;
                                break;
                            }
                        case LizardGraphics lizGraf:
                            {
                                // Main body
                                var chunks = lizGraf.lizard.bodyChunks;
                                labels[0].color = lizGraf.HeadColor(timeStacker);
                                labels[0].SetPosition(GetPos(chunks[1], timeStacker) - camPos);
                                labels[0].rotation = FixRotation(AngleBtwnChunks(chunks[0], chunks[2], timeStacker)) - 90f;

                                // Tongue
                                if (lizGraf.tongue != null)
                                {
                                    for (int i = 0; i < 6; i++)
                                    {
                                        var label = labels[i + 1];
                                        label.SetPosition(PointAlongParts(i, 6, lizGraf.tongue, timeStacker) - camPos);
                                        label.isVisible = lizGraf.lizard.tongue.Out;
                                        // future thing maybe: cyan lizards have custom tongue color depending on tongue vertex
                                    }
                                }
                                break;
                            }
                        case MirosBirdGraphics mirosGraf:
                            {
                                // Main body label
                                labels[0].SetPosition(self.sprites[mirosGraf.BodySprite].GetPosition());
                                labels[0].rotation = self.sprites[mirosGraf.BodySprite].rotation;

                                // Eye label
                                labels[1].SetPosition(self.sprites[mirosGraf.HeadSprite].GetPosition());
                                labels[1].rotation = self.sprites[mirosGraf.HeadSprite].rotation;
                                labels[1].color = mirosGraf.EyeColor;

                                // Re-enable eye trail sprite
                                self.sprites[mirosGraf.EyeTrailSprite].isVisible = true;
                                break;
                            }
                        case MoreSlugcats.InspectorGraphics inspGraf:
                            {
                                var insp = inspGraf.myInspector;
                                labels[0].SetPosition(GetPos(insp.bodyChunks[0], timeStacker) - camPos);
                                labels[0].color = insp.bodyColor;
                                var heads = insp.heads;
                                for (int i = 0; i < heads.Length; i++)
                                {
                                    Vector2 pos = GetPos(heads[i].Tip, timeStacker);
                                    labels[i + 1].SetPosition(pos - camPos);
                                    labels[i + 1].rotation = AngleBtwn(GetPos(insp.bodyChunks[0], timeStacker), pos);
                                    labels[i + 1].color = insp.bodyColor;
                                }
                                break;
                            }
                        case MoreSlugcats.StowawayBugGraphics stowawayGraf:
                            {
                                // Main body
                                labels[0].SetPosition(GetPos(stowawayGraf.myBug.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = FixRotation(self.sprites[1].rotation);

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
                                break;
                            }
                        case MoreSlugcats.YeekGraphics yeekGraf:
                            {
                                labels[0].SetPosition(GetPos(yeekGraf.myYeek.mainBodyChunk, timeStacker) - camPos);
                                // labels[0].rotation = Custom.VecToDeg(yeekGraf.myYeek.bodyDirection);
                                labels[0].rotation = self.sprites[yeekGraf.HeadSpritesStart + 2].rotation;
                                break;
                            }
                        case MouseGraphics mouseGraf:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(mouseGraf.mouse.bodyChunks[0], mouseGraf.mouse.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnParts(mouseGraf.head, mouseGraf.tail, timeStacker) + 90f;
                                labels[0].color = mouseGraf.BodyColor;
                                break;
                            }
                        case NeedleWormGraphics nootGraf:
                            {
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].SetPosition(nootGraf.worm.OnBodyPos((float)i / labels.Length, timeStacker) - camPos);
                                    labels[i].rotation = AngleBtwn(Vector2.zero, nootGraf.worm.OnBodyDir((float)i / labels.Length, timeStacker));
                                    labels[i].color = Color.Lerp(nootGraf.bodyColor, Color.white, Mathf.Lerp(nootGraf.lastFangOut, nootGraf.fangOut, timeStacker));
                                }
                                break;
                            }
                        case OverseerGraphics overseerGraf:
                            {
                                labels[0].SetPosition(AvgVectors(overseerGraf.DrawPosOfSegment(0f, timeStacker), overseerGraf.DrawPosOfSegment(1f, timeStacker)) - camPos);
                                labels[0].rotation = AngleBtwn(overseerGraf.DrawPosOfSegment(0f, timeStacker), overseerGraf.DrawPosOfSegment(1f, timeStacker)) + 90f;
                                labels[0].scale = (overseerGraf.DrawPosOfSegment(0f, timeStacker) - overseerGraf.DrawPosOfSegment(1f, timeStacker)).magnitude / TextWidth(labels[0].text);
                                /*for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].SetPosition(overseerGraf.DrawPosOfSegment((float)i / labels.Length, timeStacker) - camPos);
                                    labels[i].scale = overseerGraf.RadOfSegment((float)i / labels.Length, timeStacker) * 4f / FontSize;
                                }*/
                                break;
                            }
                        case PlayerGraphics playerGraf:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(playerGraf.player.bodyChunks[0], playerGraf.player.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(playerGraf.player.bodyChunks[0], playerGraf.player.bodyChunks[1], timeStacker) + 90f;
                                break;
                            }
                        case PoleMimicGraphics poleMimicGraf:
                            {
                                float poleAlpha = Mathf.Lerp(poleMimicGraf.lastLookLikeAPole, poleMimicGraf.lookLikeAPole, timeStacker);
                                // Move labels
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    var label = labels[i];
                                    label.SetPosition(PointAlongTentacle(labels.Length - i, labels.Length + 1, poleMimicGraf.pole.tentacle, timeStacker) - camPos);

                                    int leafPair = Mathf.RoundToInt((1f - (float)i / labels.Length) * (poleMimicGraf.leafPairs - 1));
                                    label.color = Color.Lerp(poleMimicGraf.blackColor, poleMimicGraf.mimicColor, poleAlpha);
                                    label.alpha = 1f - poleAlpha;
                                    if (leafPair < poleMimicGraf.decoratedLeafPairs)
                                    {
                                        FSprite sprite = self.sprites[poleMimicGraf.LeafDecorationSprite(leafPair, 0)];
                                        float revealAmt = Mathf.Lerp(poleMimicGraf.leavesMimic[leafPair, 0, 4], poleMimicGraf.leavesMimic[leafPair, 0, 3], timeStacker);
                                        label.color = Color.Lerp(label.color, sprite.color, revealAmt);
                                        // label.alpha = Mathf.Max(label.alpha, revealAmt);
                                    }
                                }

                                // Unhide all sprites but hide them differently
                                foreach (var sprite in self.sprites)
                                {
                                    sprite.isVisible = true;
                                    sprite.alpha = poleAlpha;
                                }
                                break;
                            }
                        case ScavengerGraphics scavGraf:
                            {
                                // Body chunk guide: 0 -> body, 1 -> hips, 2 -> head
                                labels[0].SetPosition(GetPos(scavGraf.scavenger.bodyChunks[0], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(scavGraf.scavenger.bodyChunks[1], scavGraf.scavenger.bodyChunks[0], timeStacker);

                                labels[1].SetPosition(GetPos(scavGraf.scavenger.bodyChunks[2], timeStacker) - camPos);
                                labels[1].rotation = FixRotation(AngleBtwn(GetPos(scavGraf.scavenger.bodyChunks[2], timeStacker), scavGraf.lookPoint)) - 90f;
                                labels[1].scale = scavGraf.scavenger.bodyChunks[2].rad * Mathf.Lerp(4f, 8f, Mathf.Lerp(scavGraf.lastEyesPop, scavGraf.eyesPop, timeStacker)) / TextWidth("Head");
                                break;
                            }
                        case SnailGraphics snailGraf:
                            {
                                var words = CWTs.pascalRegex.Split(snailGraf.snail.abstractCreature.creatureTemplate.type.value).Where(x => x.Length > 0).ToArray();
                                var angle = AngleBtwnChunks(snailGraf.snail.bodyChunks[1], snailGraf.snail.bodyChunks[0], timeStacker); // + 90f; // readd +90f for head to end
                                var snailPos = GetPos(snailGraf.snail.bodyChunks[1], timeStacker);
                                int k = 0;
                                for (int i = 0; i < words.Length; i++)
                                {
                                    // Negative because positive = up and we want later words to be below if custom creature uses Snail as base
                                    var yPos = -FontSize * (i - (words.Length - 1f) / 2f);
                                    for (int j = 0; j < words[i].Length; j++, k++)
                                    {
                                        var xPos = (TextWidth(words[i].Substring(0, j)) - TextWidth(words[i]) / 2f + TextWidth(words[i][j].ToString())) * labels[k].scale;
                                        var angleOff = Mathf.Atan2(yPos, xPos);
                                        var dist = Mathf.Sqrt(xPos * xPos + yPos * yPos);

                                        var pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + angleOff), -Mathf.Sin(angle * Mathf.Deg2Rad + angleOff)) * dist + snailPos - camPos;
                                        labels[k].SetPosition(pos);
                                        labels[k].rotation = angle;
                                    }
                                }
                                break;
                            }
                        case SpiderGraphics spiderGraf:
                            {
                                var label = labels[0];
                                label.SetPosition(spiderGraf.spider.mainBodyChunk.pos - camPos);

                                var rot = self.sprites[spiderGraf.BodySprite].rotation;
                                if (spiderGraf.spider.dead) rot = FixRotation(rot + 90f);
                                label.rotation = rot;
                                break;
                            }
                        case TempleGuardGraphics guardGraf:
                            {
                                labels[0].SetPosition(self.sprites[guardGraf.HeadSprite].GetPosition());
                                labels[0].rotation = self.sprites[guardGraf.HeadSprite].rotation - 180f;
                                labels[0].color = self.sprites[guardGraf.EyeSprite(1)].color;

                                for (int i = guardGraf.FirstHaloSprite; i < guardGraf.halo.totalSprites; i++)
                                {
                                    self.sprites[i].isVisible = true;
                                }
                                break;
                            }
                        case TentaclePlantGraphics kelpGraf:
                            {
                                break;
                            }
                        case TubeWormGraphics wormGraf:
                            {
                                break;
                            }
                        case VultureGraphics vultureGraf:
                            {
                                // Labels: 0 -> body, 1 -> mask, [2,2+len(tentacles)*4] -> wings, the rest -> tusks (may not be present)
                                var chunks = vultureGraf.vulture.bodyChunks;
                                var tentacles = vultureGraf.vulture.tentacles; // vulture wings

                                // Body sprite
                                labels[0].SetPosition(GetPos(chunks[0], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(chunks[1], chunks[3], timeStacker);

                                // Mask sprite
                                labels[1].SetPosition(chunks[4].pos - camPos);
                                labels[1].rotation = self.sprites[vultureGraf.HeadSprite].rotation;
                                labels[1].isVisible = (vultureGraf.vulture.State as Vulture.VultureState).mask;

                                // Vulture wings
                                int k = 0;
                                for (int i = 2; i < 2 + tentacles.Length * 4; i += 4)
                                {
                                    var tentacle = tentacles[k++];
                                    for (int j = i; j < i + 4; j++) // 4 letters per wing
                                    {
                                        var pos = PointAlongTentacle(j - i, 4, tentacle, timeStacker);
                                        labels[j].SetPosition(pos - camPos);
                                        labels[j].rotation = FixRotation(AngleBtwn(pos, GetPos(tentacle.connectedChunk, timeStacker)));
                                    }
                                }

                                // Tusks
                                if (vultureGraf.vulture.kingTusks != null)
                                {
                                    int offset = 2 + tentacles.Length * 4;
                                    for (int i = 0; i < vultureGraf.tusks.Length; i++)
                                    {
                                        labels[i + offset].SetPosition(vultureGraf.tusks[i].pos - camPos);
                                        labels[i + offset].rotation = vultureGraf.tuskRotations[i] + (i % 2 == 0 ? -90f : 90f);
                                    }
                                }
                                break;
                            }
                        case VultureGrubGraphics grubGraf:
                            {
                                break;
                            } // * (?)

                        // Things that are not Creatures
                        case OracleGraphics oracleGraf:
                            {
                                break;
                            }
                        case VoidSpawnGraphics voidSpawnGraphics:
                            {
                                break;
                            }

                        case JellyFish jelly:
                            {
                                break;
                            }
                        case BigJellyFish bigJelly:
                            {
                                break;
                            }

                        // Default cases
                        case GraphicsModule module:
                            {
                                // Creature should be single thingy
                                var pos = GetPos(module.owner.bodyChunks[0], timeStacker) - camPos;
                                var rot = self.sprites[0].rotation;

                                // Yeek fix compatibility
                                if (YeekFix && module.GetType().Name == "FixedYeekGraphics")
                                {
                                    // The head sprite should be the last sprite that has rotation anything but 0
                                    rot = 0f;
                                    IEnumerable<FSprite> rots = self.sprites.Where(s => s.rotation != 0f);
                                    if (rots.Count() > 0) rot = rots.Last().rotation;
                                }

                                labels[0].SetPosition(pos);
                                labels[0].rotation = rot;
                                break;
                            }
                        default: { break; } // do nothing
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Ran into error in SpriteLeaser.Update, disabling hook effects");
                Logger.LogError(e);
                DoThings = false;
            }
        }
        private static void SpriteLeaser_CleanSpritesAndRemove(On.RoomCamera.SpriteLeaser.orig_CleanSpritesAndRemove orig, SpriteLeaser self)
        {
            orig(self);

            // Remove labels
            FLabel[] labels;
            if ((labels = CWTs.GetLabels(self)) != null)
            {
                foreach (var label in labels)
                {
                    label.RemoveFromContainer();
                }
            }
        }

    }
}
