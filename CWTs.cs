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
                else if (module.drawableObject is OracleSwarmer || module.drawableObject is NSHSwarmer)
                {
                    return [new(Font, "N")];
                }

                else if (module.drawableObject is BubbleGrass)
                {
                    return [new(Font, $"Bubble{Environment.NewLine}Weed")];
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
