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
using WordWorld.Creatures.MoreSlugcats;
using WordWorld.Creatures;
using WordWorld.Defaults;
using WordWorld.Items;
using WordWorld.Misc;

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

            var labels = CWTs.GetLabels(self, rCam);
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
                        // Creatures
                        case BigEelGraphics:        BigEelWords.Draw(obj as BigEelGraphics, labels, self, timeStacker, camPos); break;
                        case BigSpiderGraphics:     BigSpiderWords.Draw(obj as BigSpiderGraphics, labels, self, timeStacker, camPos); break;
                        case CentipedeGraphics:     CentipedeWords.Draw(obj as CentipedeGraphics, labels, self, timeStacker, camPos); break;
                        case CicadaGraphics:        CicadaWords.Draw(obj as CicadaGraphics, labels, timeStacker, camPos); break;
                        case DaddyGraphics:         DaddyWords.Draw(obj as DaddyGraphics, labels, timeStacker, camPos); break;
                        case DeerGraphics:          DeerWords.Draw(obj as DeerGraphics, labels, timeStacker, camPos); break;
                        case DropBugGraphics:       DropBugWords.Draw(obj as DropBugGraphics, labels, self, timeStacker, camPos); break;
                        case EggBugGraphics:        EggBugWords.Draw(obj as EggBugGraphics, labels, self, timeStacker, camPos); break;
                        case FlyGraphics:           FlyWords.Draw(obj as FlyGraphics, labels, self, timeStacker, camPos); break;
                        case GarbageWormGraphics:   GarbageWormWords.Draw(obj as GarbageWormGraphics, labels, self, timeStacker, camPos); break;
                        case HazerGraphics:         HazerWords.Draw(obj as HazerGraphics, labels, self, timeStacker, camPos); break;
                        case JetFishGraphics:       JetFishWords.Draw(obj as JetFishGraphics, labels, self, timeStacker, camPos); break;
                        case LeechGraphics:         LeechWords.Draw(obj as LeechGraphics, labels, self, timeStacker, camPos); break;
                        case LizardGraphics:        LizardWords.Draw(obj as LizardGraphics, labels, timeStacker, camPos); break;
                        case MirosBirdGraphics:     MirosBirdWords.Draw(obj as MirosBirdGraphics, labels, self); break;
                        case MouseGraphics:         MouseWords.Draw(obj as MouseGraphics, labels, timeStacker, camPos); break;
                        case NeedleWormGraphics:    NeedleWormWords.Draw(obj as NeedleWormGraphics, labels, timeStacker, camPos); break;
                        case OverseerGraphics:      OverseerWords.Draw(obj as OverseerGraphics, labels, timeStacker, camPos); break;
                        case PlayerGraphics:        PlayerWords.Draw(obj as PlayerGraphics, labels, timeStacker, camPos); break;
                        case PoleMimicGraphics:     PoleMimicWords.Draw(obj as PoleMimicGraphics, labels, self, timeStacker, camPos); break;
                        case ScavengerGraphics:     ScavengerWords.Draw(obj as ScavengerGraphics, labels, self, timeStacker, camPos); break;
                        case SnailGraphics:         SnailWords.Draw(obj as SnailGraphics, labels, timeStacker, camPos); break;
                        case SpiderGraphics:        SpiderWords.Draw(obj as SpiderGraphics, labels, self, timeStacker, camPos); break;
                        case TempleGuardGraphics:   TempleGuardWords.Draw(obj as TempleGuardGraphics, labels, self, timeStacker, camPos); break;
                        case TentaclePlantGraphics: TentaclePlantWords.Draw(obj as TentaclePlantGraphics, labels, self, timeStacker, camPos); break;
                        case TubeWormGraphics:      TubeWormWords.Draw(obj as TubeWormGraphics, labels, timeStacker, camPos); break;
                        case VultureGrubGraphics:   VultureGrubWords.Draw(obj as VultureGrubGraphics, labels, self, timeStacker, camPos); break;
                        case VultureGraphics:       VultureWords.Draw(obj as VultureGraphics, labels, self, timeStacker, camPos); break;

                        case InspectorGraphics:    InspectorWords.Draw(obj as InspectorGraphics, labels, timeStacker, camPos); break;
                        case StowawayBugGraphics:  StowawayBugWords.Draw(obj as StowawayBugGraphics, labels, self, timeStacker, camPos); break;
                        case YeekGraphics:         YeekWords.Draw(obj as YeekGraphics, labels, self, timeStacker, camPos); break;

                        // Items
                        case BubbleGrass:     BubbleGrassWords.Draw(obj as BubbleGrass, labels, timeStacker, camPos); break;
                        case DandelionPeach:  DandelionPeachWords.Draw(obj as DandelionPeach, labels, timeStacker, camPos); break;
                        case DataPearl:       DataPearlWords.Draw(obj as DataPearl, labels, timeStacker, camPos); break;
                        case EggBugEgg:       EggBugEggWords.Draw(obj as EggBugEgg, labels, timeStacker, camPos); break;
                        case FireEgg:         FireEggWords.Draw(obj as FireEgg, labels, self, timeStacker, camPos); break;
                        case GlowWeed:        GlowWeedWords.Draw(obj as GlowWeed, labels, timeStacker, camPos); break;
                        case GooieDuck:       GooieDuckWords.Draw(obj as GooieDuck, labels, timeStacker, camPos); break;
                        case Lantern:         LanternWords.Draw(obj as Lantern, labels, self, timeStacker, camPos); break;
                        case LillyPuck:       LillyPuckWords.Draw(obj as LillyPuck, labels, self, timeStacker, camPos); break;
                        case MoonCloak:       MoonCloakWords.Draw(obj as MoonCloak, labels, timeStacker, camPos); break;
                        case NSHSwarmer:      NSHSwarmerWords.Draw(obj as NSHSwarmer, labels, self, timeStacker, camPos); break;
                        case OracleSwarmer:   OracleSwarmerWords.Draw(obj as OracleSwarmer, labels, self, timeStacker, camPos); break;
                        case PuffBall:        PuffBallWords.Draw(obj as PuffBall, labels, timeStacker, camPos); break;
                        case Rock:            RockWords.Draw(obj as Rock, labels, timeStacker, camPos); break;
                        case ScavengerBomb:   ScavengerBombWords.Draw(obj as ScavengerBomb, labels, self, timeStacker, camPos); break;
                        case SingularityBomb: SingularityBombWords.Draw(obj as SingularityBomb, labels, timeStacker, camPos); break;
                        case SlimeMold:       SlimeMoldWords.Draw(obj as SlimeMold, labels, self, timeStacker, camPos); break;
                        case Spear:           SpearWords.Draw(obj as Spear, labels, self, timeStacker, camPos); break;

                        // Misc
                        case BigJellyFish:      BigJellyFishWords.Draw(obj as BigJellyFish, labels, timeStacker, camPos); break;
                        case Bullet:            BulletWords.Draw(obj as Bullet, labels, timeStacker, camPos); break;
                        case DartMaggot:        DartMaggotWords.Draw(obj as DartMaggot, labels, timeStacker, camPos); break;
                        case Ghost:             GhostWords.Draw(obj as Ghost, labels, self, timeStacker, camPos); break;
                        case JellyFish:         JellyFishWords.Draw(obj as JellyFish, labels, self, timeStacker, camPos); break;
                        case LizardSpit:        LizardSpitWords.Draw(obj as LizardSpit, labels, timeStacker, camPos); break;
                        case OracleGraphics:    OracleWords.Draw(obj as OracleGraphics, labels, self, timeStacker, camPos); break;
                        case VoidSpawnGraphics: VoidSpawnWords.Draw(obj as VoidSpawnGraphics, labels, self, timeStacker, camPos); break;


                        // Defaults
                        case GraphicsModule:  GMWords.Draw(obj as GraphicsModule, labels, self, timeStacker, camPos); break;
                        case PhysicalObject:  POWords.Draw(obj as PhysicalObject, labels, timeStacker, camPos); break;
                    };
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
            if ((labels = CWTs.GetLabels(self, null)) != null)
            {
                foreach (var label in labels)
                {
                    label.RemoveFromContainer();
                }
            }
        }

    }
}
