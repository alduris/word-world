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
        private static readonly Regex pascalRegex = new(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");
        
        private static readonly ConditionalWeakTable<RoomCamera.SpriteLeaser, FLabel[]> graphicsCWT = new();
        public static FLabel[] GetLabels(this RoomCamera.SpriteLeaser module) => graphicsCWT.GetValue(module, self => {
            if (!Plugin.DoThings) return null;
            try
            {
                var font = Custom.GetFont();

                // Test API stuff first
                if (WordAPI.RegisteredClasses.Count > 0 && WordAPI.RegisteredClasses.TryGetValue(self.drawableObject.GetType(), out var funcs))
                {
                    var strs = funcs.Item1.Invoke(self.drawableObject);
                    return strs.Select(x => new FLabel(font, x)).ToArray();
                }

                // Built-in stuff
                if ((self.drawableObject as GraphicsModule)?.owner is Creature)
                {
                    var type = ((self.drawableObject as GraphicsModule).owner as Creature).abstractCreature.creatureTemplate.type;

                    if (self.drawableObject is VultureGraphics vultureGraf)
                    {
                        // Vultures get extra sprites
                        List<FLabel> list = new() {
                            new(font, pascalRegex.Replace(type.value, Environment.NewLine)),
                            new(font, "Mask")
                        };
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
                        return list.ToArray();
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
                        
                        List<FLabel> list = new();
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

                        return list.ToArray();
                    }
                    else if (self.drawableObject is EggBugGraphics)
                    {
                        string str;
                        if (type == CreatureTemplate.Type.EggBug) str = "Eggbug";
                        else if (ModManager.MSC && type == MoreSlugcatsEnums.CreatureTemplateType.FireBug) str = "Firebug";
                        else str = pascalRegex.Replace(type.value, Environment.NewLine);

                        List<FLabel> list = new() { new(font, str) };
                        
                        // Eggs
                        for (int i = 0; i < 6; i++)
                            list.Add(new(font, "Egg"));
                        
                        return list.ToArray();
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
                        List<FLabel> list = new() { new(font, shortname) };

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
                        return list.ToArray();
                    }
                    else if (self.drawableObject is DeerGraphics)
                    {
                        return new FLabel[] { new(font, "Deer"), new(font, "Antlers") };
                    }
                    else if (self.drawableObject is MirosBirdGraphics)
                    {
                        return new FLabel[] { new(font, $"Miros{Environment.NewLine}Bird"), new(font, "Eye") };
                    }
                    else if (self.drawableObject is LizardGraphics lizGraf)
                    {
                        string name = pascalRegex.Replace(type.value, " ");
                        if (lizGraf.tongue != null)
                        {
                            return new FLabel[]
                            {
                                new(font, name),
                                new(font, "T"),
                                new(font, "o"),
                                new(font, "n"),
                                new(font, "g"),
                                new(font, "u"),
                                new(font, "e"),
                            };
                        }
                        return new FLabel[] { new(font, name) };
                    }
                    else if (ModManager.MSC && self.drawableObject is InspectorGraphics inspGraf)
                    {
                        List<FLabel> list = new() { new(font, "Inspector") };
                        for (int i = 0; i < inspGraf.myInspector.heads.Length; i++)
                            list.Add(new(font, "Head"));
                        return list.ToArray();
                    }
                    else if (ModManager.MSC && self.drawableObject is StowawayBugGraphics stowawayGraf)
                    {
                        List<FLabel> list = new() { new(font, "Stowaway") };
                        for (int i = 0; i < stowawayGraf.myBug.heads.Length; i++)
                        {
                            foreach (var c in "Tentacle")
                            {
                                list.Add(new(font, c.ToString()));
                            }
                        }
                        return list.ToArray();
                    }
                    else if (
                        type == CreatureTemplate.Type.BigNeedleWorm ||
                        type == CreatureTemplate.Type.SmallNeedleWorm ||
                        type == CreatureTemplate.Type.PoleMimic ||
                        type == CreatureTemplate.Type.TentaclePlant ||
                        type == CreatureTemplate.Type.GarbageWorm ||
                        type == CreatureTemplate.Type.Overseer ||
                        type == CreatureTemplate.Type.Snail ||
                        type == CreatureTemplate.Type.BigEel
                    )
                    {
                        // Long bendy creature; create many FLabels for individual chars
                        var chars = type.value.ToCharArray();
                        if (type == CreatureTemplate.Type.BigEel)
                            chars = "Leviathan".ToCharArray();

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
                        else if (self.drawableObject is LeechGraphics)
                            str = pascalRegex.Replace(str, " ");
                        else
                            str = pascalRegex.Replace(str, Environment.NewLine);
                        
                        return new FLabel[] { new(font, str) };
                    }
                }
                else if (module.drawableObject is OracleGraphics)
                {
                    var str = "Iterator";

                    // Figure out the name of the iterator
                    var id = (module.drawableObject as OracleGraphics).oracle.ID;
                    if (id == Oracle.OracleID.SL) str = "Looks to the Moon";
                    else if (id == Oracle.OracleID.SS) str = "Five Pebbles";
                    else if (ModManager.MSC) {
                        if (id == MoreSlugcatsEnums.OracleID.CL) str = "Five Pebbles";
                        else if (id == MoreSlugcatsEnums.OracleID.DM) str = "Five Pebbles";
                        else if (id == MoreSlugcatsEnums.OracleID.ST) str = "Sliver of Straw";
                    }

                    // Iterators have 4 arm segments
                    return new FLabel[] { new(font, str), new(font, "Arm"), new(font, "Arm"), new(font, "Arm"), new(font, "Arm") };
                }
                else if (module.drawableObject is JellyFish)
                {
                    return new FLabel[] { new(font, "Jellyfish") };
                }
                else if (ModManager.MSC && module.drawableObject is BigJellyFish)
                {
                    return new FLabel[] { new(font, "Big Jellyfish") };
                }
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
