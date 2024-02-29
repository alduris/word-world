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

                        // Items and physical objects
                        case EggBugEgg eggBugEgg:
                            {
                                labels[0].scale = eggBugEgg.firstChunk.rad * 3f / TextWidth(labels[0].text);
                                labels[0].color = eggBugEgg.eggColors[1];
                                break;
                            }
                        case FirecrackerPlant cherrybomb:
                            {
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
                        case LizardSpit spit:
                            {
                                labels[0].color = Color.Lerp(self.sprites[spit.DotSprite].color, self.sprites[spit.JaggedSprite].color, 0.4f);
                                break;
                            }
                        case MoonCloak cloak:
                            {
                                labels[0].scale = cloak.firstChunk.rad * 2f / FontSize;
                                labels[0].color = cloak.Color(0.25f);
                                break;
                            }
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
                                labels[0].scaleY /= 2f;

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
                        case SporePlant beehive:
                            {
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

                // Assign container
                for (int i = 0; i < labels.Length; i++)
                {
                    var label = labels[i];
                    label.alignment = FLabelAlignment.Center;
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
                                labels[0].color = Color.Lerp(bubbleWeed.blackColor, bubbleWeed.color, oxygen);
                                labels[0].scale = Vector2.Lerp(bubbleWeed.stalk[end].lastPos - bubbleWeed.stalk[0].lastPos, bubbleWeed.stalk[end].pos - bubbleWeed.stalk[0].pos, timeStacker).magnitude
                                    / FontSize * Mathf.Lerp(0.5f, 0.75f, oxygen);
                                break;
                            }
                        case DartMaggot maggot:
                            {
                                labels[0].SetPosition(GetPos(maggot.firstChunk, timeStacker) - camPos);
                                // this rotation thing doesn't work as intended but oh well /shrug
                                labels[0].rotation = FixRotation(AngleBtwn(
                                    Vector2.Lerp(maggot.body[0,1], maggot.body[0,0], timeStacker),
                                    Vector2.Lerp(maggot.body[maggot.body.GetLength(0)-1,1], maggot.body[maggot.body.GetLength(0)-1, 0], timeStacker))) - 90f;
                                labels[0].color = maggot.yellow;
                                break;
                            }
                        case DataPearl pearl:
                            {
                                labels[0].SetPosition(GetPos(pearl.firstChunk, timeStacker) - camPos);
                                labels[0].color = Color.Lerp(pearl.color, pearl.highlightColor ?? pearl.color, Mathf.Lerp(pearl.lastGlimmer, pearl.glimmer, timeStacker));
                                break;
                            }
                        case FirecrackerPlant cherrybomb:
                            {
                                break;
                            }
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
                        case LizardSpit spit:
                            {
                                labels[0].SetPosition(Vector2.Lerp(spit.lastPos, spit.pos, timeStacker) - camPos);
                                labels[0].scale = spit.Rad * 4f / FontSize;
                                break;
                            }
                        case MoonCloak cloak:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker);
                                break;
                            }
                        case ScavengerBomb bomb:
                            {
                                labels[0].SetPosition(GetPos(bomb.firstChunk, timeStacker) - camPos);
                                labels[0].color = self.sprites[0].color;
                                break;
                            }
                        case SlimeMold slime:
                            {
                                bool isSeed = ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed;
                                labels[0].SetPosition(GetPos(slime.firstChunk, timeStacker) - camPos);
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
                        case SporePlant beehive:
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
