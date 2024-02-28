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
            ShowSprites = self.devToolsActive;
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
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs) && funcs.StyleLabels != null)
                {
                    // Deal with API stuff
                    funcs.StyleLabels.Invoke(obj, labels);
                }
                else
                {
                    // Built-in stuff
                    switch (obj)
                    {
                        // Creatures
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
                                foreach (var label in labels)
                                {
                                    label.scale = 1.75f;
                                }
                                break;
                            }
                        case TubeWormGraphics wormGraf:
                            {
                                labels[0].scale = (wormGraf.worm.bodyChunks[0].rad + wormGraf.worm.bodyChunks[1].rad + wormGraf.worm.bodyChunkConnections[0].distance) * 2f / TextWidth(labels[0].text);
                                labels[0].color = wormGraf.color;
                                break;
                            }
                        case VultureGraphics vultureGraf:
                            {
                                // Labels: 0 -> body, 1 -> head, [2,2+len(tentacles)*4] -> wings, the rest -> tusks (may not be present)

                                var tentacles = vultureGraf.vulture.tentacles; // vulture wings
                                labels[0].scale = vultureGraf.vulture.bodyChunks[0].rad * 4f / FontSize; // 4f because 2 chunks and 2*radius=diameter
                                labels[0].color = self.sprites[vultureGraf.BodySprite].color;
                                labels[1].scale = vultureGraf.vulture.bodyChunks[4].rad * 4f / FontSize;
                                labels[1].color = self.sprites[vultureGraf.EyesSprite].color;

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
                                labels[0].scale = grubGraf.worm.bodyChunks.Sum(x => x.rad) * 3f / TextWidth(labels[0].text);
                                break;
                            }

                        // Graphics modules
                        case OracleGraphics oracleGraf:
                            {
                                labels[0].color = self.sprites[oracleGraf.HeadSprite].color;
                                labels[0].scale = 1.25f;
                                
                                // Arm
                                int armStop = (oracleGraf.umbCord != null) ? labels.Length - "UmbilicalCord".Length : labels.Length;
                                for (int i = 1; i < armStop; i++)
                                {
                                    labels[i].color = i % 2 == 0 ? oracleGraf.GenericJointBaseColor() : oracleGraf.GenericJointHighLightColor();
                                }

                                // Umbilical
                                for (int i = armStop; i < labels.Length; i++)
                                {
                                    labels[i].color = oracleGraf.GenericJointBaseColor();
                                    labels[i].scale = 0.75f;
                                }
                                break;
                            }
                        case VoidSpawnGraphics spawnGraf:
                            {
                                var scale = spawnGraf.spawn.bodyChunks.Sum(x => x.rad) * 1.75f / TextWidth(string.Join("", labels.Select(x => x.text)));
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].scale = scale;
                                    labels[i].color = RainWorld.SaturatedGold;
                                }
                                break;
                            }

                        // Misc I guess
                        case JellyFish jelly:
                            {
                                labels[0].scale = jelly.bodyChunks[0].rad * 1.5f / FontSize;
                                break;
                            }
                        case BigJellyFish bigJelly:
                            {
                                foreach (var label in labels)
                                {
                                    label.color = bigJelly.color;
                                }
                                var chunks = bigJelly.bodyChunks;
                                //labels[0].scale = (chunks[0].rad + chunks[bigJelly.leftHoodChunk].rad + chunks[bigJelly.rightHoodChunk].rad) * 2f / TextWidth(labels[0].text);
                                labels[0].scale = (chunks.Sum(x => x.rad) - chunks[bigJelly.CoreChunk].rad) * 2f / TextWidth(labels[0].text);
                                labels[1].color = bigJelly.coreColor;
                                labels[1].scale = chunks[bigJelly.CoreChunk].rad * 2f / TextWidth(labels[1].text);
                                break;
                            }
                        case Ghost echo:
                            {
                                labels[0].color = echo.goldColor;
                                labels[0].scale = 8f * echo.scale;
                                break;
                            }
                        case NSHSwarmer greenNeuron: // why don't you extend OracleSwarmer...
                            {
                                labels[0].scale = greenNeuron.firstChunk.rad * 3f / FontSize;
                                labels[0].color = greenNeuron.myColor;
                                break;
                            }
                        case OracleSwarmer neuron:
                            {
                                labels[0].scale = neuron.firstChunk.rad * 3f / FontSize;
                                break;
                            }

                        // Items
                        /*case BubbleGrass grass:
                            {
                                break;
                            }
                        case Bullet bullet:
                            {
                                break;
                            }
                        case DataPearl pearl:
                            {
                                break;
                            }
                        case DandelionPeach peach:
                            {
                                break;
                            }*/
                        case EggBugEgg eggBugEgg:
                            {
                                labels[0].scale = eggBugEgg.firstChunk.rad * 3f / TextWidth(labels[0].text);
                                labels[0].color = eggBugEgg.eggColors[1];
                                break;
                            }
                        case FireEgg fireEgg:
                            {
                                labels[0].scale = fireEgg.firstChunk.rad * 3f / TextWidth(labels[0].text);
                                labels[0].color = fireEgg.eggColors[1];
                                break;
                            }
                        case FlareBomb flare:
                            {
                                labels[0].scale = flare.firstChunk.rad / TextWidth(labels[0].text); // the lack of a multiplier is intentional
                                labels[0].color = Color.Lerp(flare.color, new(1f, 1f, 1f), 0.9f);
                                break;
                            }
                        /*case GlowWeed glowWeed:
                            {
                                break;
                            }*/
                        case GooieDuck gooieduck:
                            {
                                labels[0].color = gooieduck.CoreColor;
                                break;
                            }
                        case Lantern lantern:
                            {
                                labels[0].scale = lantern.firstChunk.rad * 3f / FontSize;
                                labels[0].color = self.sprites[0].color;
                                break;
                            }
                        case LillyPuck lillyPuck:
                            {
                                labels[0].scale = lillyPuck.firstChunk.rad * 3f / FontSize;
                                //labels[0].scale = self.sprites[0].element.sourcePixelSize.y / TextWidth(labels[0].text);
                                labels[0].color = lillyPuck.flowerColor;
                                break;
                            }
                        case MoonCloak cloak:
                            {
                                labels[0].scale = cloak.firstChunk.rad * 2f / FontSize;
                                labels[0].color = cloak.Color(0.25f);
                                break;
                            }
                        /*case Rock rock:
                            {
                                break;
                            }*/
                        case ScavengerBomb bomb:
                            {
                                labels[0].scale = bomb.firstChunk.rad * 3f / FontSize;
                                break;
                            }
                        case SingularityBomb sBomb:
                            {
                                labels[0].scale = sBomb.firstChunk.rad * 3f / FontSize;
                                labels[0].color = Custom.HSL2RGB(0.6638889f, 1f, 0.35f);
                                break;
                            }
                        case SlimeMold slime:
                            {
                                bool isSeed = ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed; // why the hell are seeds slime mold
                                labels[0].scale = slime.firstChunk.rad * 3f / FontSize * (slime.JellyfishMode ? 2.4f : (slime.big ? 1.3f : 1f));
                                labels[0].color = isSeed ? self.sprites[0].color : slime.color;
                                break;
                            }
                        case Spear spear:
                            {
                                bool explosive = spear is ExplosiveSpear;
                                labels[0].color = spear.color;
                                labels[0].scale = self.sprites[spear.bugSpear || explosive ? 1 : 0].element.sourcePixelSize.y  * 0.9f / TextWidth(labels[0].text);
                                labels[0].scaleY /= 1.75f;

                                if (explosive)
                                {
                                    labels[0].color = (spear as ExplosiveSpear).redColor;
                                }
                                else if (spear.bugSpear)
                                {
                                    labels[0].color = self.sprites[0].color;
                                }
                                break;
                            }

                        // Room effects

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
                        case PlayerCarryableItem item:
                            {
                                labels[0].scale = item.firstChunk.rad * 3f / FontSize;
                                labels[0].color = item.color;
                                break;
                            }
                        case PhysicalObject physObj:
                            {
                                labels[0].scale = physObj.firstChunk.rad * 3f / FontSize;
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
            if (ShowSprites)
            {
                foreach (var sprite in self.sprites)
                {
                    sprite.isVisible = true;
                }
            }
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
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs) && funcs.DrawLabels != null)
                {
                    // Deal with API stuff
                    funcs.DrawLabels.Invoke(obj, labels, self, timeStacker, camPos);
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
                                // Body
                                labels[0].SetPosition(eggBugGraf.bug.bodyChunks[1].pos - camPos);
                                labels[0].rotation = FixRotation(self.sprites[eggBugGraf.HeadSprite].rotation) + 90f;

                                // Eggs
                                for (int i = 0; i < 6; i++)
                                {
                                    var eggSprite = self.sprites[eggBugGraf.BackEggSprite(i % 2, i / 2, 2)];
                                    labels[i + 1].x = eggSprite.x;
                                    labels[i + 1].y = eggSprite.y;
                                    labels[i + 1].rotation = eggSprite.rotation;
                                    if (eggBugGraf.bug.FireBug && i >= eggBugGraf.bug.eggsLeft) labels[i + 1].isVisible = false;
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
                                // Place letters along body
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].SetPosition(PointAlongTentacle(labels.Length - i - 1, labels.Length, gWormGraf.worm.tentacle, timeStacker) - camPos);
                                    labels[i].isVisible = gWormGraf.worm.extended > 0f;
                                }

                                // Set first letter to eye color
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

                                // Body
                                labels[0].SetPosition(GetPos(insp.bodyChunks[0], timeStacker) - camPos);
                                labels[0].color = insp.bodyColor;

                                // Heads
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
                                    labels[i].rotation = AngleFrom(nootGraf.worm.OnBodyDir((float)i / labels.Length, timeStacker));

                                    // Color = body color if not angry, white if fang out as warning
                                    labels[i].color = Color.Lerp(nootGraf.bodyColor, Color.white, Mathf.Lerp(nootGraf.lastFangOut, nootGraf.fangOut, timeStacker));
                                }
                                break;
                            }
                        case OverseerGraphics overseerGraf:
                            {
                                labels[0].isVisible = overseerGraf.overseer.room != null;

                                // Fixes a crash moving rooms in safari, which was where I discovered it. No clue if it existed in normal gameplay, didn't happen in arena.
                                if (overseerGraf.overseer.room != null)
                                {
                                    labels[0].SetPosition(AvgVectors(overseerGraf.DrawPosOfSegment(0f, timeStacker), overseerGraf.DrawPosOfSegment(1f, timeStacker)) - camPos);
                                    labels[0].rotation = AngleBtwn(overseerGraf.DrawPosOfSegment(0f, timeStacker), overseerGraf.DrawPosOfSegment(1f, timeStacker)) + 90f;
                                    labels[0].scale = (overseerGraf.DrawPosOfSegment(0f, timeStacker) - overseerGraf.DrawPosOfSegment(1f, timeStacker)).magnitude / TextWidth(labels[0].text);
                                }
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

                                // Reenable mask (elites/chieftan)
                                if (scavGraf.maskGfx != null)
                                {
                                    for (int i = scavGraf.MaskSprite; i < scavGraf.MaskSprite + scavGraf.maskGfx.TotalSprites; i++)
                                    {
                                        self.sprites[i].isVisible = true;
                                    }
                                }
                                break;
                            }
                        case SnailGraphics snailGraf:
                            {
                                // This one is a bit weird, I wanted the letters to be colored individually as a gradient because snails have two colors.
                                // So this code basically manually positions and rotates all letters.
                                var words = CWTs.pascalRegex.Split(snailGraf.snail.abstractCreature.creatureTemplate.type.value).Where(x => x.Length > 0).ToArray();
                                var angle = AngleBtwnChunks(snailGraf.snail.bodyChunks[0], snailGraf.snail.bodyChunks[1], timeStacker); // + 90f; // readd +90f for head to end
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
                                // Head, color = eye color
                                labels[0].SetPosition(self.sprites[guardGraf.HeadSprite].GetPosition());
                                labels[0].rotation = self.sprites[guardGraf.HeadSprite].rotation - 180f;
                                labels[0].color = self.sprites[guardGraf.EyeSprite(1)].color;

                                // Re-enable halo
                                for (int i = guardGraf.FirstHaloSprite; i < guardGraf.FirstHaloSprite + guardGraf.halo.totalSprites; i++)
                                {
                                    self.sprites[i].isVisible = true;
                                }
                                break;
                            }
                        case TentaclePlantGraphics kelpGraf:
                            {
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    // Calculate position (we don't care about rotation lol)
                                    labels[i].SetPosition(PointAlongRope(i, labels.Length, kelpGraf.ropeGraphic, timeStacker) - camPos);

                                    // Calculate color since it can vary
                                    float colorIndex = Custom.LerpMap(i, 0, labels.Length - 1, 0, kelpGraf.danglers.Length - 1);
                                    Color color = Color.Lerp(self.sprites[Mathf.FloorToInt(colorIndex)].color, self.sprites[Mathf.CeilToInt(colorIndex)].color, colorIndex % 1f);
                                    labels[i].color = color; // UndoColorLerp(color, rCam.currentPalette.blackColor, rCam.room.Darkness(kelpGraf.plant.rootPos));
                                }
                                break;
                            }
                        case TubeWormGraphics wormGraf:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(wormGraf.worm.bodyChunks[0], wormGraf.worm.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = FixRotation(AngleBtwnChunks(wormGraf.worm.bodyChunks[0], wormGraf.worm.bodyChunks[1], timeStacker)) - 90f;
                                break;
                            }
                        case VultureGraphics vultureGraf:
                            {
                                // Labels: 0 -> body, 1 -> mask, [2,2+len(tentacles)*4] -> wings, the rest -> tusks (may not be present)
                                var chunks = vultureGraf.vulture.bodyChunks;
                                var tentacles = vultureGraf.vulture.tentacles; // vulture wings

                                // Body sprite
                                labels[0].SetPosition(GetPos(chunks[0], timeStacker) - camPos);
                                // labels[0].rotation = AngleBtwnChunks(chunks[1], chunks[3], timeStacker); // doesn't work

                                // Head
                                labels[1].SetPosition(chunks[4].pos - camPos);
                                labels[1].rotation = self.sprites[vultureGraf.HeadSprite].rotation;

                                // Mask (reenable)
                                //labels[1].isVisible = (vultureGraf.vulture.State as Vulture.VultureState).mask;
                                if ((vultureGraf.vulture.State as Vulture.VultureState).mask)
                                {
                                    self.sprites[vultureGraf.MaskSprite].isVisible = true;
                                    if (vultureGraf.IsKing)
                                    {
                                        self.sprites[vultureGraf.MaskArrowSprite].isVisible = true;
                                    }
                                }

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
                                    var tusks = vultureGraf.vulture.kingTusks;

                                    int offset = 2 + tentacles.Length * 4;
                                    for (int i = 0; i < vultureGraf.tusks.Length; i++)
                                    {
                                        labels[i + offset].SetPosition(AvgVectors(tusks.tusks[i].chunkPoints[0,0], tusks.tusks[i].chunkPoints[1,0]) - camPos);
                                        labels[i + offset].rotation = AngleBtwn(tusks.tusks[i].chunkPoints[0, 0], tusks.tusks[i].chunkPoints[1, 0]) + 90f;
                                        //labels[i + offset].SetPosition(vultureGraf.tusks[i].pos - camPos);
                                        //labels[i + offset].rotation = vultureGraf.tuskRotations[i] + (i % 2 == 0 ? -90f : 90f);
                                        self.sprites[tusks.tusks[i].LaserSprite(vultureGraf)].isVisible = true;
                                    }
                                }

                                // Miros laser
                                if (vultureGraf.IsMiros)
                                {
                                    self.sprites[vultureGraf.LaserSprite()].isVisible = Mathf.Lerp(vultureGraf.lastLaserActive, vultureGraf.laserActive, timeStacker) > 0f;
                                }
                                break;
                            }
                        case VultureGrubGraphics grubGraf:
                            {
                                // Body
                                labels[0].SetPosition(GetPos(grubGraf.worm.bodyChunks[0], timeStacker) - camPos);
                                labels[0].rotation = FixRotation(AngleBtwnChunks(grubGraf.worm.bodyChunks[1], grubGraf.worm.bodyChunks[2], timeStacker)) - 90f;
                                labels[0].color = self.sprites[grubGraf.MeshSprite].color;

                                // Show laser sprite
                                self.sprites[grubGraf.LaserSprite].isVisible = Mathf.Lerp(grubGraf.lastLaserActive, grubGraf.laserActive, timeStacker) > 0f;
                                break;
                            }

                        case OracleGraphics oracleGraf:
                            {
                                // Body
                                //labels[0].SetPosition(AvgBodyChunkPos(oracleGraf.oracle.bodyChunks[0], oracleGraf.oracle.bodyChunks[1], timeStacker) - camPos);
                                labels[0].SetPosition(self.sprites[oracleGraf.HeadSprite].GetPosition());
                                labels[0].rotation = AngleBtwnChunks(oracleGraf.oracle.bodyChunks[1], oracleGraf.oracle.bodyChunks[0], timeStacker);

                                // Arm stuff
                                int armStop = (oracleGraf.umbCord != null || oracleGraf.discUmbCord != null) ? labels.Length - "UmbilicalCord".Length : labels.Length;
                                for (int i = 1; i < armStop; i++)
                                {
                                    bool isArm = i % 2 == 0;
                                    var arm = oracleGraf.oracle.arm.joints[(i - 1) / 2];
                                    var armPos = Vector2.Lerp(arm.lastPos, arm.pos, timeStacker);
                                    if (isArm)
                                    {
                                        Vector2 armNextPos = arm.next != null ? Vector2.Lerp(arm.next.lastPos, arm.next.pos, timeStacker) : GetPos(oracleGraf.oracle.bodyChunks[1], timeStacker);
                                        // labels[i].SetPosition(AvgVectors(armPos, armNextPos) - camPos);
                                        labels[i].SetPosition(arm.ElbowPos(timeStacker, armNextPos));
                                    }
                                    else
                                    {
                                        labels[i].SetPosition(armPos - camPos);
                                    }
                                }

                                // Umbilical stuff
                                if (oracleGraf.umbCord != null)
                                {
                                    var cord = oracleGraf.umbCord.coord;
                                    for (int i = armStop; i < labels.Length; i++)
                                    {
                                        // adds a padding of one space around it
                                        var index = Custom.LerpMap(i, armStop - 1, labels.Length, 0, cord.GetLength(0));
                                        var prevPos = Vector2.Lerp(cord[Mathf.FloorToInt(index), 1], cord[Mathf.FloorToInt(index), 0], timeStacker);
                                        var nextPos = Vector2.Lerp(cord[Mathf.CeilToInt(index), 1], cord[Mathf.CeilToInt(index), 0], timeStacker);
                                        var pos = Vector2.Lerp(prevPos, nextPos, index % 1f);
                                        var rot = AngleBtwn(prevPos, nextPos) + 90f;
                                        
                                        labels[i].SetPosition(pos - camPos);
                                        labels[i].rotation = rot;
                                    }
                                }

                                // Show halo
                                if (oracleGraf.halo != null)
                                {
                                    for (int i = oracleGraf.halo.firstSprite; i < oracleGraf.halo.firstSprite + oracleGraf.halo.totalSprites; i++)
                                    {
                                        self.sprites[i].isVisible = true;
                                    }
                                }
                                break;
                            }
                        case VoidSpawnGraphics spawnGraf:
                            {
                                var chunks = spawnGraf.spawn.bodyChunks;
                                for (int i = 0; i < labels.Length; i++)
                                {
                                    labels[i].SetPosition(PointAlongChunks(i, labels.Length, chunks, timeStacker) - camPos);
                                    var index = Custom.LerpMap(i, 0, labels.Length - 1, 0, chunks.Length - 1);
                                    // labels[i].rotation = AngleBtwnChunks(chunks[Mathf.FloorToInt(index)], chunks[Mathf.CeilToInt(index)], timeStacker);

                                    labels[i].alpha = 1 - spawnGraf.AlphaFromGlowDist(labels[i].GetPosition(), spawnGraf.glowPos);
                                }
                                self.sprites[spawnGraf.GlowSprite].isVisible = true;
                                if (spawnGraf.hasOwnGoldEffect)
                                    self.sprites[spawnGraf.EffectSprite].isVisible = true;
                                break;
                            }

                        case JellyFish jelly:
                            {
                                labels[0].SetPosition(GetPos(jelly.bodyChunks[0], timeStacker) - camPos);
                                labels[0].rotation = AngleFrom(jelly.rotation);
                                labels[0].color = self.sprites[jelly.BodySprite(1)].color;
                                break;
                            }
                        case BigJellyFish bigJelly:
                            {
                                // Main and core
                                labels[0].SetPosition(GetPos(bigJelly.mainBodyChunk, timeStacker) - camPos);
                                labels[1].SetPosition(GetPos(bigJelly.bodyChunks[bigJelly.CoreChunk], timeStacker) - camPos);

                                // Tentacles
                                for (int i = 0; i < bigJelly.tentacles.Length; i++)
                                {
                                    var tentacle = bigJelly.tentacles[i];
                                    // 8 = "Tentacle".Length
                                    for (int j = 0; j < 8; j++)
                                    {
                                        int k = 2 + i * 8 + j;
                                        var index = Custom.LerpMap(j, -1, 7, 0, tentacle.GetLength(0) - 1);
                                        var prevPos = Vector2.Lerp(tentacle[Mathf.FloorToInt(index), 1], tentacle[Mathf.FloorToInt(index), 0], timeStacker);
                                        var nextPos = Vector2.Lerp(tentacle[Mathf.CeilToInt(index), 1], tentacle[Mathf.CeilToInt(index), 0], timeStacker);

                                        labels[k].SetPosition(Vector2.Lerp(prevPos, nextPos, index % 1) - camPos);
                                        labels[k].rotation = AngleBtwn(nextPos, prevPos);
                                    }
                                }
                                break;
                            }
                        case Ghost echo:
                            {
                                var startPos = Vector2.Lerp(echo.spine[0].lastPos, echo.spine[0].pos, timeStacker);
                                var endPos = Vector2.Lerp(echo.spine[echo.spine.Length - 1].lastPos, echo.spine[echo.spine.Length - 1].pos, timeStacker);
                                labels[0].SetPosition(AvgVectors(startPos, endPos) - camPos);
                                labels[0].rotation = AngleBtwn(startPos, endPos);
                                self.sprites[echo.DistortionSprite].isVisible = true;
                                self.sprites[echo.LightSprite].isVisible = true;
                                break;
                            }
                        case NSHSwarmer greenNeuron:
                            {
                                labels[0].SetPosition(GetPos(greenNeuron.firstChunk, timeStacker) - camPos);
                                var active = Custom.SCurve(Mathf.Lerp(greenNeuron.lastHoloFade, greenNeuron.holoFade, timeStacker), 0.65f) * greenNeuron.holoShape.Fade.SmoothValue(timeStacker);
                                if (active > 0)
                                {
                                    for (int i = 5; i < self.sprites.Length; i++)
                                    {
                                        self.sprites[i].isVisible = true;
                                    }
                                }
                                break;
                            }
                        case OracleSwarmer neuron:
                            {
                                labels[0].SetPosition(GetPos(neuron.firstChunk, timeStacker) - camPos);
                                labels[0].color = self.sprites[0].color;
                                break;
                            }

                        case BubbleGrass bubbleWeed:
                            {
                                var oxygen = Mathf.Lerp(bubbleWeed.lastOxygen, bubbleWeed.oxygen, timeStacker);
                                var end = bubbleWeed.stalk.Length - 1;
                                labels[0].SetPosition(GetPos(bubbleWeed.firstChunk, timeStacker) - camPos);
                                labels[0].color = Color.Lerp(bubbleWeed.color, bubbleWeed.blackColor, oxygen);
                                labels[0].scale = Vector2.Lerp(bubbleWeed.stalk[end].lastPos - bubbleWeed.stalk[0].lastPos, bubbleWeed.stalk[end].pos - bubbleWeed.stalk[0].pos, timeStacker).magnitude
                                    / FontSize * Mathf.Lerp(0.75f, 1f, oxygen);
                                break;
                            }
                        /*case Bullet bullet:
                            {
                                labels[0].SetPosition(GetPos(bullet.firstChunk, timeStacker) - camPos);
                                break;
                            }*/
                        case DataPearl pearl:
                            {
                                labels[0].SetPosition(GetPos(pearl.firstChunk, timeStacker) - camPos);
                                labels[0].color = Color.Lerp(pearl.color, pearl.highlightColor ?? pearl.color, Mathf.Lerp(pearl.lastGlimmer, pearl.glimmer, timeStacker));
                                break;
                            }
                        /*case DandelionPeach peach:
                            {
                                labels[0].SetPosition(GetPos(peach.firstChunk, timeStacker) - camPos);
                                break;
                            }
                        case EggBugEgg egg:
                            {
                                labels[0].SetPosition(GetPos(egg.firstChunk, timeStacker) - camPos);
                                break;
                            }*/
                        case FireEgg fireEgg:
                            {
                                labels[0].SetPosition(GetPos(fireEgg.firstChunk, timeStacker) - camPos);
                                labels[0].scale = fireEgg.firstChunk.rad * 3f / TextWidth(labels[0].text);
                                labels[0].color = self.sprites[1].color;
                                break;
                            }
                        case FlareBomb flare:
                            {
                                labels[0].SetPosition(GetPos(flare.firstChunk, timeStacker) - camPos);
                                self.sprites[2].isVisible = true;
                                break;
                            }
                        /*case GlowWeed glowWeed:
                            {
                                labels[0].SetPosition(GetPos(glowWeed.firstChunk, timeStacker) - camPos);
                                break;
                            }*/
                        case GooieDuck gooieduck:
                            {
                                labels[0].SetPosition(GetPos(gooieduck.firstChunk, timeStacker) - camPos);
                                labels[0].scale = gooieduck.firstChunk.rad * 3f / FontSize + Mathf.Sin(gooieduck.PulserA) / 6f;
                                break;
                            }
                        case Lantern lantern:
                            {
                                labels[0].SetPosition(GetPos(lantern.firstChunk, timeStacker) - camPos);
                                self.sprites[3].isVisible = true;
                                break;
                            }
                        case LillyPuck lillyPuck:
                            {
                                labels[0].SetPosition(GetPos(lillyPuck.firstChunk, timeStacker) - camPos);
                                labels[0].rotation = FixRotation(self.sprites[0].rotation) - 90f;
                                break;
                            }
                        case MoonCloak cloak:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker);
                                break;
                            }
                        /*case Rock rock:
                            {
                                labels[0].SetPosition(GetPos(rock.firstChunk, timeStacker) - camPos);
                                break;
                            }*/
                        case ScavengerBomb bomb:
                            {
                                labels[0].SetPosition(GetPos(bomb.firstChunk, timeStacker) - camPos);
                                labels[0].color = self.sprites[0].color;
                                break;
                            }
                        /*case SingularityBomb sBomb:
                            {
                                labels[0].SetPosition(GetPos(sBomb.firstChunk, timeStacker) - camPos);
                                break;
                            }*/
                        case SlimeMold slime:
                            {
                                bool isSeed = ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed;
                                labels[0].SetPosition(GetPos(slime.firstChunk, timeStacker) - camPos);
                                labels[0].rotation = Mathf.Lerp(AngleFrom(slime.lastRotation), AngleFrom(slime.rotation), timeStacker);
                                if (!isSeed)
                                {
                                    self.sprites[slime.LightSprite].isVisible = slime.darkMode > 0f;
                                    self.sprites[slime.BloomSprite].isVisible = slime.darkMode > 0f;
                                }
                                break;
                            }
                        case Spear spear:
                            {
                                labels[0].SetPosition(GetPos(spear.firstChunk, timeStacker) - camPos);
                                labels[0].rotation = FixRotation(self.sprites[spear.bugSpear || spear is ExplosiveSpear ? 1 : 0].rotation) - 90f;

                                if (spear.IsNeedle)
                                {
                                    labels[0].color = self.sprites[0].color;
                                }
                                else if (spear is ElectricSpear)
                                {
                                    labels[0].color = self.sprites[1].color;
                                }
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
                        case PhysicalObject physObj:
                            {
                                labels[0].SetPosition(GetPos(physObj.firstChunk, timeStacker) - camPos);
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
