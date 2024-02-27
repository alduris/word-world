using System;
using System.Collections.Generic;
using UnityEngine;

namespace WordWorld
{
    internal struct CustomCase
    {
        public Func<IDrawable, string[]> CreateLabels;
        public Action<IDrawable, FLabel[]> StyleLabels;
        public Action<IDrawable, FLabel[], RoomCamera.SpriteLeaser, float, Vector2> DrawLabels;
    }

    public class WordAPI
    {
        internal static Dictionary<Type, CustomCase> RegisteredClasses = [];
        internal static Dictionary<Oracle.OracleID, string> RegisteredIterators = [];

        /// <summary>
        /// Registers an IDrawable with the mod to replace with text.
        /// </summary>
        /// <param name="type">The type, which extends IDrawable.</param>
        /// <param name="createLabelsFunc">Function to return the strings to turn into FLabels. Parameter: the corresponding IDrawable.</param>
        /// <param name="styleLabelsFunc">Action that will be called after InitializeSprites, meant to style the labels. Parameters: the IDrawable and the FLabels.</param>
        /// <param name="drawLabelsFunc">Action that will be called after DrawSprites, meant to move/rotate the labels. Parameters: the IDrawable, the FLabels, the sprite leaser, timeStacker, and camPos.</param>
        /// <exception cref="ArgumentException">Throws if the type passed into the function is not an IDrawable.</exception>
        public static void RegisterItem(Type type, Func<IDrawable, string[]> createLabelsFunc, Action<IDrawable, FLabel[]> styleLabelsFunc, Action<IDrawable, FLabel[], RoomCamera.SpriteLeaser, float, Vector2> drawLabelsFunc)
        {
            if (!typeof(IDrawable).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type must implement IDrawable!");
            }
            RegisteredClasses.Add(type, new CustomCase { CreateLabels = createLabelsFunc, StyleLabels = styleLabelsFunc, DrawLabels = drawLabelsFunc });
        }

        /// <summary>
        /// Unregisters the item from the mod. Unsure why you'd need to do this but it's here if you need to.
        /// </summary>
        /// <param name="type">The type to unregister.</param>
        /// <returns>If it was fond and successfully removed</returns>
        public static bool UnregisterItem(Type type)
        {
            return RegisteredClasses.Remove(type);
        }

        /// <summary>
        /// Registers an iterator name with the mod.
        /// </summary>
        /// <param name="id">The id of the iterator</param>
        /// <param name="name">The name of the iterator</param>
        public static void RegisterIterator(Oracle.OracleID id, string name)
        {
            RegisteredIterators.Add(id, name);
        }

        /// <summary>
        /// Unregisters an iterator name from the mod.
        /// </summary>
        /// <param name="id">The iterator id</param>
        /// <returns>If it was found and successfully removed</returns>
        public static bool UnregisterIterator(Oracle.OracleID id)
        {
            return RegisteredIterators.Remove(id);
        }
    }
}
