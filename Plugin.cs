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

                        // Misc

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
                        case MoonCloak cloak:
                            {
                                labels[0].scale = cloak.firstChunk.rad * 2f / FontSize;
                                labels[0].color = cloak.Color(0.25f);
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
                        case MoonCloak cloak:
                            {
                                labels[0].SetPosition(AvgBodyChunkPos(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker) - camPos);
                                labels[0].rotation = AngleBtwnChunks(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker);
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
