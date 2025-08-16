using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;
using MoreSlugcats;
using UnityEngine;
using SpriteLeaser = RoomCamera.SpriteLeaser;
using WordWorld.Creatures.MoreSlugcats;
using WordWorld.Creatures;
using WordWorld.Defaults;
using WordWorld.Effects;
using WordWorld.Items;
using WordWorld.Misc;

#pragma warning disable CS0618
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace WordWorld
{

    [BepInPlugin("alduris.wordworld", "Word World", "1.0.1")]
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
        internal static bool ClownLongLegs = false;
        internal static HashSet<Type> Disabled = [];

        private void OnEnable()
        {
            Logger.LogInfo("Hooking");
            try
            {
                // Important hooks
                On.RoomCamera.SpriteLeaser.Update += SpriteLeaser_Update;
                On.RoomCamera.SpriteLeaser.CleanSpritesAndRemove += SpriteLeaser_CleanSpritesAndRemove;

                // This is literally only here for testing stuff
                #if (DEBUG)
                On.RainWorldGame.Update += RainWorldGame_Update;
                #endif

                // Mod compatibility
                On.RainWorld.OnModsInit += RainWorld_OnModsInit;

                Logger.LogInfo("Success");
            }
            catch (Exception e)
            {
                Logger.LogError("Ran into error hooking");
                Logger.LogError(e);
                DoThings = false;
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
            ClownLongLegs = ModManager.ActiveMods.Exists(mod => mod.id == "clownlonglegs");
        }

        private static void SpriteLeaser_Update(On.RoomCamera.SpriteLeaser.orig_Update orig, SpriteLeaser self, float timeStacker, RoomCamera rCam, Vector2 camPos)
        {
            orig(self, timeStacker, rCam, camPos);

            var wordifier = CWTs.GetWordify(self, rCam);
            var obj = self.drawableObject;
            if (wordifier == null || !DoThings || (Disabled.Count > 0 && Disabled.Contains(obj.GetType()))) return;

            if (!ShowSprites)
            {
                foreach (var sprite in self.sprites)
                {
                    sprite.isVisible = false;
                }
            }

            try
            {
                wordifier.Draw(self, timeStacker, camPos);
            }
            catch (Exception e)
            {
                Disabled.Add(obj.GetType());
                Logger.LogError("Ran into error in SpriteLeaser.Update! Disabling future effects on cause. Cause:" + obj.GetType().FullName);
                Logger.LogError(e);
                foreach (var sprite in self.sprites)
                {
                    sprite.isVisible = true;
                }
            }
        }

        private static void SpriteLeaser_CleanSpritesAndRemove(On.RoomCamera.SpriteLeaser.orig_CleanSpritesAndRemove orig, SpriteLeaser self)
        {
            orig(self);

            // Remove labels
            CWTs.GetWordify(self, null)?.RemoveFromContainer();
        }

    }
}
