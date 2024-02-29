using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;
using RWCustom;
using MoreSlugcats;
using UnityEngine;
using SpriteLeaser = RoomCamera.SpriteLeaser;
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

                        // Items
                        case Spear spear:
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
