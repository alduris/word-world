using System;
using System.Runtime.CompilerServices;
using MoreSlugcats;
using RWCustom;
using Watcher;
using WordWorld.Creatures;
using WordWorld.Creatures.MoreSlugcats;
using WordWorld.Creatures.Watcher;
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
#warning reimplement api
                return obj switch
                {
                    // Creatures
                    BigEelGraphics =>        new BigEelWords(),
                    BigSpiderGraphics =>     new BigSpiderWords(),
                    CentipedeGraphics =>     new CentipedeWords(),
                    CicadaGraphics =>        new CicadaWords(),
                    DaddyGraphics =>         new DaddyWords(),
                    DeerGraphics =>          new DeerWords(),
                    DropBugGraphics =>       new DropBugWords(),
                    EggBugGraphics =>        new EggBugWords(),
                    FlyGraphics =>           new FlyWords(),
                    GarbageWormGraphics =>   new GarbageWormWords(),
                    HazerGraphics =>         new HazerWords(),
                    JetFishGraphics =>       new JetFishWords(),
                    LeechGraphics =>         new LeechWords(),
                    LizardGraphics =>        new LizardWords(),
                    MirosBirdGraphics =>     new MirosBirdWords(),
                    MouseGraphics =>         new MouseWords(),
                    NeedleWormGraphics =>    new NeedleWormWords(),
                    OverseerGraphics =>      new OverseerWords(),
                    PlayerGraphics =>        new PlayerWords(),
                    PoleMimicGraphics =>     new PoleMimicWords(),
                    ScavengerGraphics =>     new ScavengerWords(),
                    SnailGraphics =>         new SnailWords(),
                    SpiderGraphics =>        new SpiderWords(),
                    TempleGuardGraphics =>   new TempleGuardWords(),
                    TentaclePlantGraphics => new TentaclePlantWords(),
                    TubeWormGraphics =>      new TubeWormWords(),
                    VultureGrubGraphics =>   new VultureGrubWords(),
                    VultureGraphics =>       new VultureWords(),

                    InspectorGraphics =>   new InspectorWords(),
                    StowawayBugGraphics => new StowawayBugWords(),
                    YeekGraphics =>        new YeekWords(),

                    DrillCrab => new DrillCrabWords(),

                    // Items
                    BubbleGrass =>      new BubbleGrassWords(),
                    DandelionPeach =>   new DandelionPeachWords(),
                    DangleFruit =>      new DangleFruitWords(),
                    DataPearl =>        new DataPearlWords(),
                    EggBugEgg =>        new EggBugEggWords(),
                    EnergyCell =>       new EnergyCellWords(),
                    FirecrackerPlant => new FirecrackerPlantWords(),
                    FireEgg =>          new FireEggWords(),
                    FlareBomb =>        new FlareBombWords(),
                    FlyLure =>          new FlyLureWords(),
                    GlowWeed =>         new GlowWeedWords(),
                    GooieDuck =>        new GooieDuckWords(),
                    JokeRifle =>        new JokeRifleWords(),
                    KarmaFlower =>      new KarmaFlowerWords(),
                    Lantern =>          new LanternWords(),
                    LillyPuck =>        new LillyPuckWords(),
                    MoonCloak =>        new MoonCloakWords(),
                    Mushroom =>         new MushroomWords(),
                    NeedleEgg =>        new NeedleEggWords(),
                    NSHSwarmer =>       new NSHSwarmerWords(),
                    OracleSwarmer =>    new OracleSwarmerWords(),
                    OverseerCarcass =>  new OverseerCarcassWords(),
                    PuffBall =>         new PuffBallWords(),
                    ScavengerBomb =>    new ScavengerBombWords(),
                    SeedCob =>          new SeedCobWords(),
                    SingularityBomb =>  new SingularityBombWords(),
                    SlimeMold =>        new SlimeMoldWords(),
                    Spear =>            new SpearWords(),
                    SporePlant =>       new SporePlantWords(),
                    SwollenWaterNut =>  new SwollenWaterNutWords(),
                    VultureMask =>      new VultureMaskWords(),
                    WaterNut =>         new WaterNutWords(),

                    Rock => new RockWords(), // for priority reasons

                    // Misc parts
                    SporePlant.Bee =>         new SporePlantBeeWords(),
                    SporePlant.AttachedBee => new SporePlantAttachedBeeWords(),

                    // Effects
                    GoldFlakes.GoldFlake =>       new GoldFlakeWords(),
                    GreenSparks.GreenSpark =>     new GreenSparkWords(),
                    SkyDandelions.SkyDandelion => new SkyDandelionWords(),

                    // Misc
                    AncientBot =>        new AncientBotWords(),
                    BigJellyFish =>      new BigJellyFishWords(),
                    Bullet =>            new BulletWords(),
                    CosmeticInsect =>    new CosmeticInsectWords(),
                    DartMaggot =>        new DartMaggotWords(),
                    Ghost =>             new GhostWords(),
                    JellyFish =>         new JellyFishWords(),
                    LizardSpit =>        new LizardSpitWords(),
                    OracleGraphics =>    new OracleWords(),
                    VoidSpawnGraphics => new VoidSpawnWords(),
                    WormGrass.Worm =>    new WormGrassWords(),

                    // Default
                    GraphicsModule => new GMWords(),
                    _ => null
                };
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
