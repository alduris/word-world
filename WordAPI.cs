using System;
using System.Collections.Generic;
using UnityEngine;

namespace WordWorld
{
    public class WordAPI
    {
        internal static Dictionary<Type, (Func<IDrawable, string[]>, Action<IDrawable, FLabel[]>, Action<IDrawable, FLabel[], Vector2>)> RegisteredClasses = new();

        /// <summary>
        /// Registers an IDrawable with the mod to replace with text.
        /// </summary>
        /// <param name="type">The type, which extends IDrawable.</param>
        /// <param name="createLabelsFunc">Function to return the strings to turn into FLabels. Parameter: the corresponding IDrawable.</param>
        /// <param name="initLabelsFunc">Action that will be called after InitializeSprites. Parameters: the IDrawable and the FLabels.</param>
        /// <param name="drawLabelsFunc">Action that will be called after DrawSprites. Parameters: the IDrawable, the FLabels, and camPos.</param>
        /// <exception cref="ArgumentException">Throws if the type passed into the function is not an IDrawable.</exception>
        public static void RegisterItem(Type type, Func<IDrawable, string[]> createLabelsFunc, Action<IDrawable, FLabel[]> initLabelsFunc, Action<IDrawable, FLabel[], Vector2> drawLabelsFunc)
        {
            if (!type.IsInstanceOfType(typeof(IDrawable)))
            {
                throw new ArgumentException("Type must implement IDrawable!");
            }
            RegisteredClasses.Add(type, (createLabelsFunc, initLabelsFunc, drawLabelsFunc));
        }

        /// <summary>
        /// Unregisters the item from the mod. Unsure why you'd need to do this but it's here if you need to.
        /// </summary>
        /// <param name="type">The type to unregister.</param>
        /// <returns>Whether or not it was actually unregistered.</returns>
        public static bool UnregisterItem(Type type)
        {
            return RegisteredClasses.Remove(type);
        }
    }
}
