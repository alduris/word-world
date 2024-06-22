using System;
using System.Runtime.CompilerServices;
using MoreSlugcats;
using RWCustom;
using WordWorld.Creatures;
using WordWorld.Creatures.MoreSlugcats;
using WordWorld.Defaults;
using WordWorld.Effects;
using WordWorld.Items;
using WordWorld.Misc;

namespace WordWorld
{
    internal static class CWTs
    {
        
        private static readonly ConditionalWeakTable<RoomCamera.SpriteLeaser, IWordify> graphicsCWT = new();
        public static IWordify GetWordify(this RoomCamera.SpriteLeaser module, RoomCamera rCam) => graphicsCWT.GetValue(module, self => {
            var obj = self.drawableObject;
            if (!Plugin.DoThings || rCam == null || (Plugin.Disabled.Count > 0 && Plugin.Disabled.Contains(obj.GetType()))) return null;

            try
            {
                // Something to create new thing here
                throw new NotImplementedException();

                // Test API stuff first
                /*if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs) && funcs.InitLabels != null)
                {
                    return funcs.InitLabels.Invoke(self.drawableObject, self);
                }

                // Get thing to draw

                // If it's a creature, get its creature template type
                CreatureTemplate.Type type = null;
                if (obj is GraphicsModule gm && gm.owner is Creature c)
                {
                    type = c.abstractCreature.creatureTemplate.type;
                }

                // Return labels
                var labels = obj switch
                {
                    // Creatures
                    BigEelGraphics        => BigEelWords.Init(obj as BigEelGraphics, type),
                    BigSpiderGraphics     => BigSpiderWords.Init(obj as BigSpiderGraphics, type),
                    CentipedeGraphics     => CentipedeWords.Init(obj as CentipedeGraphics, type),
                    CicadaGraphics        => CicadaWords.Init(obj as CicadaGraphics, type, self),
                    DaddyGraphics         => DaddyWords.Init(obj as DaddyGraphics, type),
                    DeerGraphics          => DeerWords.Init(obj as DeerGraphics, type),
                    DropBugGraphics       => DropBugWords.Init(obj as DropBugGraphics, type),
                    EggBugGraphics        => EggBugWords.Init(obj as EggBugGraphics, type),
                    FlyGraphics           => FlyWords.Init(obj as FlyGraphics, type, self),
                    GarbageWormGraphics   => GarbageWormWords.Init(obj as GarbageWormGraphics, type, self),
                    HazerGraphics         => HazerWords.Init(obj as HazerGraphics, type),
                    JetFishGraphics       => JetFishWords.Init(obj as JetFishGraphics, type, self),
                    LeechGraphics         => LeechWords.Init(obj as LeechGraphics, type),
                    LizardGraphics        => LizardWords.Init(obj as LizardGraphics, type),
                    MirosBirdGraphics     => MirosBirdWords.Init(obj as MirosBirdGraphics, type, self),
                    MouseGraphics         => MouseWords.Init(obj as MouseGraphics, type),
                    NeedleWormGraphics    => NeedleWormWords.Init(obj as NeedleWormGraphics, type),
                    OverseerGraphics      => OverseerWords.Init(obj as OverseerGraphics, type),
                    PlayerGraphics        => PlayerWords.Init(obj as PlayerGraphics, type),
                    PoleMimicGraphics     => PoleMimicWords.Init(obj as PoleMimicGraphics, type),
                    ScavengerGraphics     => ScavengerWords.Init(obj as ScavengerGraphics, type),
                    SnailGraphics         => SnailWords.Init(obj as SnailGraphics, type),
                    SpiderGraphics        => SpiderWords.Init(obj as SpiderGraphics, type),
                    TempleGuardGraphics   => TempleGuardWords.Init(obj as TempleGuardGraphics, type, self),
                    TentaclePlantGraphics => TentaclePlantWords.Init(obj as TentaclePlantGraphics, type),
                    TubeWormGraphics      => TubeWormWords.Init(obj as TubeWormGraphics, type),
                    VultureGrubGraphics   => VultureGrubWords.Init(obj as VultureGrubGraphics, type),
                    VultureGraphics       => VultureWords.Init(obj as VultureGraphics, type, self),

                    InspectorGraphics   => InspectorWords.Init(obj as InspectorGraphics, type),
                    StowawayBugGraphics => StowawayBugWords.Init(obj as StowawayBugGraphics, type, self),
                    YeekGraphics        => YeekWords.Init(obj as YeekGraphics, type),

                    // Items
                    BubbleGrass      => BubbleGrassWords.Init(obj as BubbleGrass),
                    DandelionPeach   => DandelionPeachWords.Init(obj as DandelionPeach),
                    DangleFruit      => DangleFruitWords.Init(obj as DangleFruit),
                    DataPearl        => DataPearlWords.Init(obj as DataPearl),
                    EggBugEgg        => EggBugEggWords.Init(obj as EggBugEgg),
                    EnergyCell       => EnergyCellWords.Init(),
                    FirecrackerPlant => FirecrackerPlantWords.Init(),
                    FireEgg          => FireEggWords.Init(obj as FireEgg),
                    FlareBomb        => FlareBombWords.Init(obj as FlareBomb),
                    FlyLure          => FlyLureWords.Init(),
                    GlowWeed         => GlowWeedWords.Init(obj as GlowWeed),
                    GooieDuck        => GooieDuckWords.Init(obj as GooieDuck),
                    JokeRifle        => JokeRifleWords.Init(self),
                    KarmaFlower      => KarmaFlowerWords.Init(obj as KarmaFlower),
                    Lantern          => LanternWords.Init(obj as Lantern, self),
                    LillyPuck        => LillyPuckWords.Init(obj as LillyPuck),
                    MoonCloak        => MoonCloakWords.Init(obj as MoonCloak),
                    Mushroom         => MushroomWords.Init(obj as Mushroom, self),
                    NeedleEgg        => NeedleEggWords.Init(obj as NeedleEgg),
                    NSHSwarmer       => NSHSwarmerWords.Init(obj as NSHSwarmer),
                    OracleSwarmer    => OracleSwarmerWords.Init(obj as OracleSwarmer),
                    OverseerCarcass  => OverseerCarcassWords.Init(obj as OverseerCarcass),
                    PuffBall         => PuffBallWords.Init(obj as PuffBall),
                    ScavengerBomb    => ScavengerBombWords.Init(obj as ScavengerBomb),
                    SeedCob          => SeedCobWords.Init(),
                    SingularityBomb  => SingularityBombWords.Init(obj as SingularityBomb),
                    SlimeMold        => SlimeMoldWords.Init(obj as SlimeMold, self),
                    Spear            => SpearWords.Init(obj as Spear, self),
                    SporePlant       => SporePlantWords.Init(obj as SporePlant),
                    SwollenWaterNut  => SwollenWaterNutWords.Init(obj as SwollenWaterNut),
                    VultureMask      => VultureMaskWords.Init(obj as VultureMask),
                    WaterNut         => WaterNutWords.Init(obj as WaterNut),

                    Rock             => RockWords.Init(obj as Rock), // for priority reasons

                    // Misc parts
                    SporePlant.Bee => SporePlantWords.BeeInit(),
                    SporePlant.AttachedBee => SporePlantWords.AttachedBeeInit(self),

                    // Effects
                    GoldFlakes.GoldFlake       => GoldFlakeWords.Init(obj as GoldFlakes.GoldFlake, self),
                    GreenSparks.GreenSpark     => GreenSparkWords.Init(obj as GreenSparks.GreenSpark, self),
                    SkyDandelions.SkyDandelion => SkyDandelionWords.Init(self),

                    // Misc
                    AncientBot        => AncientBotWords.Init(),
                    BigJellyFish      => BigJellyFishWords.Init(obj as BigJellyFish),
                    Bullet            => BulletWords.Init(obj as Bullet),
                    CosmeticInsect    => CosmeticInsectWords.Init(obj as CosmeticInsect, self),
                    DartMaggot        => DartMaggotWords.Init(),
                    Ghost             => GhostWords.Init(obj as Ghost),
                    JellyFish         => JellyFishWords.Init(obj as JellyFish),
                    LizardSpit        => LizardSpitWords.Init(obj as LizardSpit, self),
                    OracleGraphics    => OracleWords.Init(obj as OracleGraphics, self),
                    VoidSpawnGraphics => VoidSpawnWords.Init(obj as VoidSpawnGraphics),
                    WormGrass.Worm    => WormGrassWords.Init(obj as WormGrass.Worm, self),

                    // Default
                    _ => null
                };

                if (labels == null && obj is GraphicsModule && (obj as GraphicsModule).owner is Creature)
                {
                    GMWords.Init(obj as GraphicsModule, self, type.value);
                }

                // Assign container
                if (labels != null)
                {
                    var container = self.sprites[0].container ?? rCam.ReturnFContainer("Midground");
                    for (int i = 0; i < labels.Length; i++)
                    {
                        var label = labels[i];
                        label.alignment = FLabelAlignment.Center;
                        container.AddChild(label);
                    }
                }

                return labels;*/
            }
            catch(Exception e)
            {
                Plugin.Disabled.Add(obj.GetType());
                Plugin.Logger.LogError("Ran into error in CWT! Disabling future effects on cause. Cause: " + obj.GetType().FullName);
                Plugin.Logger.LogError(e);
                return null;
            }
        });
    }
}
