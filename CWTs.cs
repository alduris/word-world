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
