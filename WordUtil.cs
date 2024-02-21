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

        public static float UndoLerp(float x, float b, float t) => (x - b * t) / (1 - t);
        public static Color UndoColorLerp(Color x, Color b, float t) => new(UndoLerp(x.r, b.r, t), UndoLerp(x.g, b.g, t), UndoLerp(x.b, b.b, t));
        public static float FixRotation(float rot) => (rot < 0f ? rot + 180f * (Mathf.FloorToInt(rot) / 180 + 1) : rot) % 180f;
        public static Vector2 GetPos(BodyChunk c, float timeStacker) => Vector2.LerpUnclamped(c.lastPos, c.pos, timeStacker);
        public static Vector2 GetPos(BodyPart p, float timeStacker) => Vector2.LerpUnclamped(p.lastPos, p.pos, timeStacker);
        public static Vector2 GetPos(Tentacle.TentacleChunk p, float timeStacker) => Vector2.LerpUnclamped(p.lastPos, p.pos, timeStacker);
        public static Vector2 AvgVectors(Vector2 a, Vector2 b) => new((a.x + b.x) / 2, (a.y + b.y) / 2);
        public static Vector2 AvgBodyChunkPos(BodyChunk a, BodyChunk b, float timeStacker) =>
            AvgVectors(
                GetPos(a, timeStacker),
                GetPos(a, timeStacker)
            );
        public static float AngleBtwn(Vector2 a, Vector2 b) => Custom.AimFromOneVectorToAnother(a, b);
        public static float AngleBtwnChunks(BodyChunk a, BodyChunk b, float timeStacker) => AngleBtwn(GetPos(a, timeStacker), GetPos(b, timeStacker));
        public static float AngleBtwnParts(BodyPart a, BodyPart b, float timeStacker) => AngleBtwn(GetPos(a, timeStacker), GetPos(b, timeStacker));
        public static Vector2 LerpChunkPos(int i, int len, BodyChunk[] chunks, float timeStacker)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, chunks.Length - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return Vector2.LerpUnclamped(chunks[(int)x].lastPos, chunks[(int)x].pos, timeStacker);
            }
            else
            {
                return Vector2.LerpUnclamped(
                    Vector2.LerpUnclamped(chunks[(int)x].lastPos, chunks[(int)x].pos, timeStacker),
                    Vector2.LerpUnclamped(chunks[(int)x + 1].lastPos, chunks[(int)x + 1].pos, timeStacker),
                    x % 1f);
            }
        }
        public static Vector2 LerpPartPos(int i, int len, BodyPart[] parts, float timeStacker)
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
        public static Vector2 LerpRopePos(int i, int len, RopeGraphic rope, float timeStacker)
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
        public static Vector2 LerpTentaclePos(int i, int len, Tentacle tentacle, float timeStacker)
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
        public static float LerpRotation(int i, int len, int numSprites, FSprite[] sprites, Func<int, int> lambda)
        {
            float x = Custom.LerpMap(i, 0, len - 1, 0, numSprites - 1);
            if (Mathf.Approximately(x, Mathf.Round(x)))
            {
                return sprites[lambda((int)x)].rotation;
            }
            else
            {
                return Mathf.LerpUnclamped(sprites[lambda((int)x)].rotation, sprites[lambda((int)x + 1)].rotation, x % 1f);
            }
        }
    }
}
