using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MoreSlugcats;
using RWCustom;
using UnityEngine;

namespace WordWorld
{
    internal static class CWTs
    {
        
        private static readonly ConditionalWeakTable<RoomCamera.SpriteLeaser, FLabel[]> graphicsCWT = new();
        public static FLabel[] GetLabels(this RoomCamera.SpriteLeaser module) => graphicsCWT.GetValue(module, self => {
            if (!Plugin.DoThings) return null;
            try
            {
                var Font = Custom.GetFont();

                // Test API stuff first
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs) && funcs.CreateLabels != null)
                {
                    var strs = funcs.CreateLabels.Invoke(self.drawableObject);
                    return strs.Select(x => new FLabel(Font, x)).ToArray();
                }

                // Built-in stuff
                if ((self.drawableObject as GraphicsModule)?.owner is Creature)
                {
                    return null;
                }
                else if (module.drawableObject is OracleGraphics oracleGraf)
                {
                    List<FLabel> labels = [];
                    var str = "Iterator";

                    // Figure out the name of the iterator
                    var id = oracleGraf.oracle.ID;
                    if (WordAPI.RegisteredIterators.ContainsKey(id))
                        str = WordAPI.RegisteredIterators[id];
                    else if (id == Oracle.OracleID.SL)
                        str = "Looks to the Moon";
                    else if (id == Oracle.OracleID.SS)
                        str = "Five Pebbles";
                    else if (ModManager.MSC) {
                        if (id == MoreSlugcatsEnums.OracleID.CL)
                            str = "Five Pebbles";
                        else if (id == MoreSlugcatsEnums.OracleID.DM)
                            str = "Looks to the Moon";
                        else if (id == MoreSlugcatsEnums.OracleID.ST)
                            str = "Sliver of Straw";
                    }
                    labels.Add(new(Font, string.Join(Environment.NewLine, str.Split(' '))));

                    // Arm
                    if (oracleGraf.oracle.arm != null)
                    {
                        for (int i = 0; i < oracleGraf.oracle.arm.joints.Length; i++)
                        {
                            labels.Add(new(Font, "Joint"));
                            labels.Add(new(Font, "Arm"));
                        }
                    }

                    // Umbilical cord
                    if (oracleGraf.umbCord != null)
                    {
                        foreach (char letter in "UmbilicalCord".ToCharArray())
                        {
                            labels.Add(new(Font, letter.ToString()));
                        }
                    }

                    return [..labels];
                }
                else if (module.drawableObject is VoidSpawnGraphics)
                {
                    return "VoidSpawn".ToCharArray().Select(x => new FLabel(Font, x.ToString())).ToArray();
                }
                else if (module.drawableObject is JellyFish)
                {
                    return [new(Font, $"Jelly{Environment.NewLine}fish")];
                }
                else if (ModManager.MSC && module.drawableObject is BigJellyFish bigJelly)
                {
                    List<FLabel> labels = [new(Font, "Big Jellyfish"), new(Font, "Core")];
                    for (int i = 0; i < bigJelly.tentacles.Length; i++)
                    {
                        foreach (var c in "Tentacle".ToCharArray())
                        {
                            labels.Add(new(Font, c.ToString()));
                        }
                    }

                    return [.. labels];
                }
                else if (module.drawableObject is Ghost)
                {
                    return [new(Font, "Echo")];
                }
                else if (module.drawableObject is OracleSwarmer || module.drawableObject is NSHSwarmer)
                {
                    return [new(Font, "N")];
                }

                else if (module.drawableObject is BubbleGrass)
                {
                    return [new(Font, $"Bubble{Environment.NewLine}Weed")];
                }
                else if (module.drawableObject is DartMaggot)
                {
                    return [new(Font, "Maggot")];
                }
                else if (module.drawableObject is DataPearl || module.drawableObject is DandelionPeach)
                {
                    return [new(Font, "P")];
                }
                else if (module.drawableObject is EggBugEgg || module.drawableObject is FireEgg)
                {
                    return [new(Font, "Egg")];
                }
                else if (module.drawableObject is FlareBomb)
                {
                    return [new(Font, "F")];
                }
                else if (module.drawableObject is GlowWeed)
                {
                    return [new(Font, $"Glow{Environment.NewLine}Weed")];
                }
                else if (module.drawableObject is GooieDuck)
                {
                    return [new(Font, $"Gooie{Environment.NewLine}duck")];
                }
                else if (module.drawableObject is Lantern)
                {
                    return [new(Font, "L")];
                }
                else if (module.drawableObject is LillyPuck)
                {
                    return [new(Font, "Lillypuck")];
                }
                else if (module.drawableObject is LizardSpit)
                {
                    return [new(Font, "Spit")];
                }
                else if (module.drawableObject is MoonCloak)
                {
                    return [new(Font, "Cloak")];
                }
                else if (module.drawableObject is PuffBall)
                {
                    return [new(Font, "Puff")];
                }
                else if (module.drawableObject is Rock)
                {
                    return [new(Font, "R")];
                }
                else if (module.drawableObject is ScavengerBomb || module.drawableObject is Bullet)
                {
                    return [new(Font, "B")];
                }
                else if (module.drawableObject is SingularityBomb)
                {
                    return [new(Font, "S")];
                }
                else if (module.drawableObject is SlimeMold slime)
                {
                    if (slime.JellyfishMode)
                        return [new(Font, "Jelly")];
                    else if (ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed)
                        return [new(Font, "Seed")];
                    else
                        return [new(Font, "Mold")];
                }
                else if (module.drawableObject is Spear)
                {
                    return [new(Font, "Spear")];
                }
                
                return null;
            }
            catch(Exception e)
            {
                Plugin.Logger.LogError("Ran into error in CWT!");
                Plugin.Logger.LogError(e);
                Plugin.DoThings = false;
                return null;
            }
        });

        public static bool HasLabel(GraphicsModule gm)
        {
            return gm.owner is Creature || gm is OracleGraphics;
        }
    }
}
