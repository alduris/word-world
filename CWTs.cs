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
        // https://stackoverflow.com/questions/3216085/split-a-pascalcase-string-into-separate-words
        internal static readonly Regex pascalRegex = new(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");
        
        private static readonly ConditionalWeakTable<RoomCamera.SpriteLeaser, FLabel[]> graphicsCWT = new();
        public static FLabel[] GetLabels(this RoomCamera.SpriteLeaser module) => graphicsCWT.GetValue(module, self => {
            if (!Plugin.DoThings) return null;
            try
            {
                var font = Custom.GetFont();

                // Test API stuff first
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs) && funcs.CreateLabels != null)
                {
                    var strs = funcs.CreateLabels.Invoke(self.drawableObject);
                    return strs.Select(x => new FLabel(font, x)).ToArray();
                }

                // Built-in stuff
                if ((self.drawableObject as GraphicsModule)?.owner is Creature)
                {
                    var type = ((self.drawableObject as GraphicsModule).owner as Creature).abstractCreature.creatureTemplate.type;

                    if (self.drawableObject is VultureGraphics vultureGraf)
                    {
                        // Vultures get extra sprites
                        List<FLabel> list = [
                            new(font, pascalRegex.Replace(type.value, Environment.NewLine)),
                            new(font, "Head")
                        ];
                        for (int i = 0; i < vultureGraf.vulture.tentacles.Length; i++)
                        {
                            list.Add(new(font, "W"));
                            list.Add(new(font, "i"));
                            list.Add(new(font, "n"));
                            list.Add(new(font, "g"));
                        }
                        if (vultureGraf.vulture.kingTusks != null)
                        {
                            list.Add(new(font, "Tusk"));
                            list.Add(new(font, "Tusk"));
                        }
                        return [.. list];
                    }
                    else if (self.drawableObject is CentipedeGraphics centiGraf)
                    {
                        // Thanks several users on RW Main discord for this idea
                        int numChunks = centiGraf.centipede.bodyChunks.Length;
                        int nameE = type.value.IndexOf("Centi") + 1;
                        if (nameE == 0) nameE = type.value.IndexOf("pede") + 1;
                        if (nameE == 0) nameE = type.value.IndexOf("e") + 1;
                        var chars = type.value.ToCharArray();
                        int numOfEs = numChunks - chars.Length;
                        
                        List<FLabel> list = [];
                        if (numChunks >= type.value.Length)
                        {
                            for (int i = 0; i < numChunks; i++)
                            {
                                int j = (i >= nameE && i < nameE + numOfEs) ? nameE : (i < nameE ? i : i - numOfEs);
                                list.Add(new(font, chars[j].ToString()));
                            }
                        }
                        else
                        {
                            foreach (char c in chars)
                            {
                                list.Add(new(font, c.ToString()));
                            }
                        }

                        return [.. list];
                    }
                    else if (self.drawableObject is EggBugGraphics)
                    {
                        string str;
                        if (type == CreatureTemplate.Type.EggBug) str = "Eggbug";
                        else if (ModManager.MSC && type == MoreSlugcatsEnums.CreatureTemplateType.FireBug) str = "Firebug";
                        else str = pascalRegex.Replace(type.value, Environment.NewLine);

                        List<FLabel> list = [new(font, str)];
                        
                        // Eggs
                        for (int i = 0; i < 6; i++)
                            list.Add(new(font, "Egg"));
                        
                        return [.. list];
                    }
                    else if (self.drawableObject is DaddyGraphics daddyGraf)
                    {
                        int cut = type.value.IndexOf("LongLegs");
                        string shortname = cut <= 0 ? pascalRegex.Replace(type.value, Environment.NewLine) : type.value.Substring(0, cut);
                        if (ModManager.MSC)
                        {
                            if (type == MoreSlugcatsEnums.CreatureTemplateType.HunterDaddy) shortname = "Hunter";
                            else if (type == MoreSlugcatsEnums.CreatureTemplateType.TerrorLongLegs) shortname = $"Your{Environment.NewLine}Mother";
                        }
                        List<FLabel> list = [new(font, shortname)];

                        for (int i = 0; i < daddyGraf.daddy.tentacles.Length; i++)
                        {
                            var tentacle = daddyGraf.daddy.tentacles[i];
                            int length = (int)(tentacle.idealLength / 20f);
                            int numOfOs = length - 7; // len("LongLegs") = 7
                            for (int j = 0; j < length; j++)
                            {
                                int k = (j >= 1 && j < 1 + numOfOs) ? 1 : (j < 1 ? j : j - numOfOs);
                                list.Add(new(font, "LongLeg"[k].ToString()));
                            }
                        }
                        return [.. list];
                    }
                    else if (self.drawableObject is DeerGraphics)
                    {
                        return [new(font, "Deer"), new(font, "Antlers")];
                    }
                    else if (self.drawableObject is MirosBirdGraphics)
                    {
                        return [new(font, $"Miros{Environment.NewLine}Bird"), new(font, "Eye")];
                    }
                    else if (self.drawableObject is LizardGraphics lizGraf)
                    {
                        string name = pascalRegex.Replace(type.value, " ");
                        if (lizGraf.tongue != null)
                        {
                            return
                            [
                                new(font, name),
                                new(font, "T"),
                                new(font, "o"),
                                new(font, "n"),
                                new(font, "g"),
                                new(font, "u"),
                                new(font, "e"),
                            ];
                        }
                        return [new(font, name)];
                    }
                    else if (ModManager.MSC && self.drawableObject is InspectorGraphics inspGraf)
                    {
                        List<FLabel> list = [new(font, "Inspector")];
                        for (int i = 0; i < inspGraf.myInspector.heads.Length; i++)
                            list.Add(new(font, "Head"));
                        return [.. list];
                    }
                    else if (ModManager.MSC && self.drawableObject is StowawayBugGraphics stowawayGraf)
                    {
                        List<FLabel> list = [new(font, "Stowaway")];
                        for (int i = 0; i < stowawayGraf.myBug.heads.Length; i++)
                        {
                            foreach (var c in "Tentacle")
                            {
                                list.Add(new(font, c.ToString()));
                            }
                        }
                        return [.. list];
                    }
                    else if (self.drawableObject is NeedleWormGraphics needleGraf)
                    {
                        int cut = type.value.IndexOf("Needle");
                        if (cut == -1) cut = type.value.IndexOf("Noodle");
                        if (cut == -1) cut = type.value.IndexOf("Noot");
                        if (cut == -1) cut = type.value.Length;

                        return [.. (type.value.Substring(0, cut) + "Noot").ToCharArray().Select(c => new FLabel(font, c.ToString()))];
                    }
                    else if (self.drawableObject is ScavengerGraphics)
                    {
                        return [new(font, pascalRegex.Replace(type.value, Environment.NewLine)), new(font, "Head")];
                    }
                    else if (self.drawableObject is SnailGraphics)
                    {
                        var list = new List<FLabel>();
                        foreach (var word in pascalRegex.Split(type.value).Where(x => x.Length > 0))
                        {
                            for (int i = 0; i < word.Length; i++)
                            {
                                list.Add(new(font, word[i].ToString()));
                            }
                        }
                        return [.. list];
                    }
                    else if (
                        self.drawableObject is PoleMimicGraphics ||
                        self.drawableObject is TentaclePlantGraphics ||
                        self.drawableObject is GarbageWormGraphics
                    )
                    {
                        // Long bendy creature; create many FLabels for individual chars
                        var chars = type.value.ToCharArray();

                        var arr = new FLabel[chars.Length];
                        for(int i = 0; i < chars.Length; i++)
                        {
                            arr[i] = new(font, chars[i].ToString());
                        }
                        return arr;
                    }
                    else
                    {
                        // Normal creature; will only have one FLabel
                        var str = type.value;

                        if (ModManager.MSC && type == MoreSlugcatsEnums.CreatureTemplateType.SlugNPC)
                            str = "Slugpup";
                        else if (type == CreatureTemplate.Type.CicadaA || type == CreatureTemplate.Type.CicadaB)
                            str = "Squidcada";
                        else if (type == CreatureTemplate.Type.BigSpider)
                            str = "Big Spider";
                        else if (type == CreatureTemplate.Type.DropBug)
                            str = "Dropwig";
                        else if (type == CreatureTemplate.Type.JetFish)
                            str = "Jetfish";
                        else if (type == CreatureTemplate.Type.LanternMouse)
                            str = "Mouse";
                        else if (self.drawableObject is LeechGraphics)
                            str = pascalRegex.Replace(str, " ");
                        else
                            str = pascalRegex.Replace(str, Environment.NewLine);
                        
                        return [new(font, str)];
                    }
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
                    labels.Add(new(font, string.Join(Environment.NewLine, str.Split(' '))));

                    // Arm
                    if (oracleGraf.oracle.arm != null)
                    {
                        for (int i = 0; i < oracleGraf.oracle.arm.joints.Length; i++)
                        {
                            labels.Add(new(font, "Joint"));
                            labels.Add(new(font, "Arm"));
                        }
                    }

                    // Umbilical cord
                    if (oracleGraf.umbCord != null)
                    {
                        foreach (char letter in "UmbilicalCord".ToCharArray())
                        {
                            labels.Add(new(font, letter.ToString()));
                        }
                    }

                    return [..labels];
                }
                else if (module.drawableObject is VoidSpawnGraphics)
                {
                    return "VoidSpawn".ToCharArray().Select(x => new FLabel(font, x.ToString())).ToArray();
                }
                else if (module.drawableObject is JellyFish)
                {
                    return [new(font, $"Jelly{Environment.NewLine}fish")];
                }
                else if (ModManager.MSC && module.drawableObject is BigJellyFish bigJelly)
                {
                    List<FLabel> labels = [new(font, "Big Jellyfish"), new(font, "Core")];
                    for (int i = 0; i < bigJelly.tentacles.Length; i++)
                    {
                        foreach (var c in "Tentacle".ToCharArray())
                        {
                            labels.Add(new(font, c.ToString()));
                        }
                    }

                    return [.. labels];
                }
                else if (module.drawableObject is Ghost)
                {
                    return [new(font, "Echo")];
                }
                else if (module.drawableObject is OracleSwarmer || module.drawableObject is NSHSwarmer)
                {
                    return [new(font, "N")];
                }

                else if (module.drawableObject is DataPearl || module.drawableObject is DandelionPeach)
                {
                    return [new(font, "P")];
                }
                else if (module.drawableObject is BubbleGrass)
                {
                    return [new(font, $"Bubble{Environment.NewLine}Weed")];
                }
                else if (module.drawableObject is EggBugEgg || module.drawableObject is FireEgg)
                {
                    return [new(font, "Egg")];
                }
                else if (module.drawableObject is GlowWeed)
                {
                    return [new(font, $"Glow{Environment.NewLine}Weed")];
                }
                else if (module.drawableObject is GooieDuck)
                {
                    return [new(font, $"Gooie{Environment.NewLine}duck")];
                }
                else if (module.drawableObject is Lantern)
                {
                    return [new(font, "L")];
                }
                else if (module.drawableObject is LillyPuck)
                {
                    return [new(font, "Lillypuck")];
                }
                else if (module.drawableObject is MoonCloak)
                {
                    return [new(font, "Cloak")];
                }
                else if (module.drawableObject is Rock)
                {
                    return [new(font, "R")];
                }
                else if (module.drawableObject is ScavengerBomb || module.drawableObject is Bullet)
                {
                    return [new(font, "B")];
                }
                else if (module.drawableObject is SlimeMold slime)
                {
                    if (slime.JellyfishMode)
                        return [new(font, "Jelly")];
                    else if (ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed)
                        return [new(font, "Seed")];
                    else
                        return [new(font, "Mold")];
                }
                /*else if (module.drawableObject is VultureMask)
                {
                    return [new(font, "Mask")];
                }*/
                else
                {
                    return null;
                }
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
