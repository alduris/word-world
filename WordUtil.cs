﻿using System;
using Menu.Remix.MixedUI;
using RWCustom;
using UnityEngine;

namespace WordWorld
{
    public class WordUtil
    {
        public const float FontSize = 20f;
        public static float TextWidth(string text) => LabelTest.GetWidth(text, false);


        /// <summary>
        /// Finds a in Lerp(a, b, t) = x
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="b">b</param>
        /// <param name="t">t</param>
        /// <returns>a</returns>
        public static float UndoLerp(float x, float b, float t) => (x - b * t) / (1 - t);

        /// <summary>
        /// Finds a in Lerp(a, b, t) = x but with colors
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="b">b</param>
        /// <param name="t">t</param>
        /// <returns>a</returns>
        public static Color UndoColorLerp(Color x, Color b, float t) => new(UndoLerp(x.r, b.r, t), UndoLerp(x.g, b.g, t), UndoLerp(x.b, b.b, t));


        /// <summary>
        /// Makes an angle (degrees) between 0 and 180
        /// </summary>
        /// <param name="rot">Angle</param>
        /// <returns>The angle between 0 and 180</returns>
        public static float FixRotation(float rot) => (rot < 0f ? rot + 180f * (Mathf.FloorToInt(rot) / 180 + 1) : rot) % 180f;


        /// <summary>
        /// Returns the position of a body chunk lerped according to the time stacker
        /// </summary>
        /// <param name="c">The body chunk</param>
        /// <param name="timeStacker">The current timeStacker</param>
        /// <returns>The lerped chunk position</returns>
        public static Vector2 GetPos(BodyChunk c, float timeStacker) => Vector2.LerpUnclamped(c.lastPos, c.pos, timeStacker);

        /// <summary>
        /// Returns the position of a body part lerped according to the time stacker
        /// </summary>
        /// <param name="p">The body part</param>
        /// <param name="timeStacker">The current timeStacker</param>
        /// <returns>The lerped body part position</returns>
        public static Vector2 GetPos(BodyPart p, float timeStacker) => Vector2.LerpUnclamped(p.lastPos, p.pos, timeStacker);

        /// <summary>
        /// Returns the position of a tentacle chunk lerped according to the time stacker
        /// </summary>
        /// <param name="p">The tentacle chunk</param>
        /// <param name="timeStacker">The current timeStacker</param>
        /// <returns>The lerped chunk position</returns>
        public static Vector2 GetPos(Tentacle.TentacleChunk p, float timeStacker) => Vector2.LerpUnclamped(p.lastPos, p.pos, timeStacker);


        /// <summary>
        /// Averages two vectors
        /// </summary>
        /// <param name="a">Vector 1</param>
        /// <param name="b">Vector 2</param>
        /// <returns>The averaged positions of the two vectors</returns>
        public static Vector2 AvgVectors(Vector2 a, Vector2 b) => new((a.x + b.x) / 2, (a.y + b.y) / 2);

        /// <summary>
        /// Averages the positions of two body chunks
        /// </summary>
        /// <param name="a">Body chunk 1</param>
        /// <param name="b">Body chunk 2</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The averaged position of the two body chunks</returns>
        public static Vector2 AvgBodyChunkPos(BodyChunk a, BodyChunk b, float timeStacker) =>
            AvgVectors(
                GetPos(a, timeStacker),
                GetPos(b, timeStacker)
            );


        /// <summary>
        /// Gets the angle between two vectors. This is just a shorter way to type Custom.AimFromOneVectorToAnother(a, b)
        /// </summary>
        /// <param name="a">Vector 1</param>
        /// <param name="b">Vector 2</param>
        /// <returns>The angle between two vectors</returns>
        public static float AngleBtwn(Vector2 a, Vector2 b) => Custom.AimFromOneVectorToAnother(a, b);

        /// <summary>
        /// Gets the angle between two body chunks.
        /// </summary>
        /// <param name="a">Body chunk 1</param>
        /// <param name="b">Body chunk 2</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The angle between the two body chunks</returns>
        public static float AngleBtwnChunks(BodyChunk a, BodyChunk b, float timeStacker) => AngleBtwn(GetPos(a, timeStacker), GetPos(b, timeStacker));

        /// <summary>
        /// Gets the angle between two body parts.
        /// </summary>
        /// <param name="a">Body part 1</param>
        /// <param name="b">Body part 2</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The angle between the two body parts</returns>
        public static float AngleBtwnParts(BodyPart a, BodyPart b, float timeStacker) => AngleBtwn(GetPos(a, timeStacker), GetPos(b, timeStacker));


        /// <summary>
        /// Lerps between a bunch of body chunks. Intended to be used for lerping individual letters between chunks.
        /// I genuinely don't know how to explain this well. See how I use it in the source code.
        /// </summary>
        /// <param name="i">The i-th item in the array you're trying to lerp</param>
        /// <param name="len">The length of the array you're trying to lerp</param>
        /// <param name="chunks">The body chunks</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The input array position mapped to a position between two body chunks.</returns>
        public static Vector2 PointAlongChunks(int i, int len, BodyChunk[] chunks, float timeStacker)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, chunks.Length - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return Vector2.LerpUnclamped(chunks[(int)Mathf.Round(x)].lastPos, chunks[(int)Mathf.Round(x)].pos, timeStacker);
            }
            else
            {
                return Vector2.LerpUnclamped(
                    Vector2.LerpUnclamped(chunks[(int)x].lastPos, chunks[(int)x].pos, timeStacker),
                    Vector2.LerpUnclamped(chunks[(int)x + 1].lastPos, chunks[(int)x + 1].pos, timeStacker),
                    x % 1f);
            }
        }

        /// <summary>
        /// Lerps between a bunch of body parts. Intended to be used for lerping individual letters between parts.
        /// I genuinely don't know how to explain this well. See how I use it in the source code.
        /// </summary>
        /// <param name="i">The i-th item in the array you're trying to lerp</param>
        /// <param name="len">The length of the array you're trying to lerp</param>
        /// <param name="parts">The body parts</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The input array position mapped to a position between two body parts.</returns>
        public static Vector2 PointAlongParts(int i, int len, BodyPart[] parts, float timeStacker)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, parts.Length - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return Vector2.LerpUnclamped(parts[(int)x].lastPos, parts[(int)x].pos, timeStacker);
            }
            else
            {
                return Vector2.LerpUnclamped(
                    Vector2.LerpUnclamped(parts[(int)x].lastPos, parts[(int)x].pos, timeStacker),
                    Vector2.LerpUnclamped(parts[(int)x + 1].lastPos, parts[(int)x + 1].pos, timeStacker),
                    x % 1f);
            }
        }

        /// <summary>
        /// Lerps between a bunch of rope segments. Intended to be used for lerping individual letters between segments.
        /// I genuinely don't know how to explain this well. See how I use it in the source code.
        /// </summary>
        /// <param name="i">The i-th item in the array you're trying to lerp</param>
        /// <param name="len">The length of the array you're trying to lerp</param>
        /// <param name="rope">The rope</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The input array position mapped to a position between two rope segments.</returns>
        public static Vector2 PointAlongRope(int i, int len, RopeGraphic rope, float timeStacker)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, rope.segments.Length - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return Vector2.LerpUnclamped(rope.segments[(int)x].lastPos, rope.segments[(int)x].pos, timeStacker);
            }
            else
            {
                return Vector2.LerpUnclamped(
                    Vector2.LerpUnclamped(rope.segments[(int)x].lastPos, rope.segments[(int)x].pos, timeStacker),
                    Vector2.LerpUnclamped(rope.segments[(int)x + 1].lastPos, rope.segments[(int)x + 1].pos, timeStacker),
                    x % 1f
                );
            }
        }

        /// <summary>
        /// Lerps between a bunch of tentacle chunks. Intended to be used for lerping individual letters between chunks.
        /// I genuinely don't know how to explain this well. See how I use it in the source code.
        /// </summary>
        /// <param name="i">The i-th item in the array you're trying to lerp</param>
        /// <param name="len">The length of the array you're trying to lerp</param>
        /// <param name="tentacle">The tentacle</param>
        /// <param name="timeStacker">The current time stacker</param>
        /// <returns>The input array position mapped to a position between the tentacle chunks.</returns>
        public static Vector2 PointAlongTentacle(int i, int len, Tentacle tentacle, float timeStacker)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, tentacle.tChunks.Length - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return Vector2.LerpUnclamped(tentacle.tChunks[(int)x].lastPos, tentacle.tChunks[(int)x].pos, timeStacker);
            }
            else
            {
                return Vector2.LerpUnclamped(
                    Vector2.LerpUnclamped(tentacle.tChunks[(int)x].lastPos, tentacle.tChunks[(int)x].pos, timeStacker),
                    Vector2.LerpUnclamped(tentacle.tChunks[(int)x + 1].lastPos, tentacle.tChunks[(int)x + 1].pos, timeStacker),
                    x % 1f
                );
            }
        }


        /// <summary>
        /// Lerps between a bunch of sprites and points to the next one. Intended to be used for lerping individual letters between sprites.
        /// This one is probably like the most confusing to explain out of all out of the functions I included here. See how I use it in the source code.
        /// </summary>
        /// <param name="i">The i-th item in the array you're trying to lerp</param>
        /// <param name="len">The length of the array you're trying to lerp</param>
        /// <param name="numSprites">The number of sprites to consider</param>
        /// <param name="sprites">All the sprites</param>
        /// <param name="lambda">A function mapping a number between 0 and numSprites to a specific sprite index</param>
        /// <returns>A position pointing from one sprite to the other</returns>
        public static float RotationAlongSprites(int i, int len, int numSprites, FSprite[] sprites, Func<int, int> lambda)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, numSprites - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return sprites[lambda((int)Mathf.Round(x))].rotation;
            }
            else
            {
                return Mathf.LerpUnclamped(sprites[lambda((int)x)].rotation, sprites[lambda((int)x + 1)].rotation, x % 1f);
            }
        }
    }
}
