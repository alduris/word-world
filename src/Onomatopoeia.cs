using System;
using RWCustom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WordWorld
{
    /// <summary>
    /// A floating text that slowly moves and fades away
    /// </summary>
    public class Onomatopoeia : UpdatableAndDeletable, IDrawable
    {
        public FLabel label;
        public OnomatopoeiaData data;

        public Vector2 Vel => data.Vel;
        public float RotVel => data.RotVel;

        public Vector2 lastPos;
        public Vector2 pos;
        public float lastRot;
        public float rot;
        public float lastLife;
        public float life;

        public Onomatopoeia(Room room, string text, Vector2 pos) =>
            new Onomatopoeia(room, text, pos, new(Color.white, Custom.RNV() * 20f, Random.value - 0.5f, 1f, Random.Range(1f, 3f)));

        public Onomatopoeia(Room room, string text, Vector2 pos, OnomatopoeiaData data)
        {
            this.room = room;
            label = new FLabel(WordUtil.Font, text)
            {
                x = pos.x, y = pos.y,
                scale = data.Scale
            };
            lastRot = rot = 0f;
            lastPos = this.pos = pos;
        }

        public override void Update(bool eu)
        {
            base.Update(eu);
            
            lastPos = pos;
            pos += Vel;

            lastRot = rot;
            rot += RotVel;

            lastLife = life;
            life -= 1f / 40f;

            if (life < 0f)
            {
                Destroy();
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            label.RemoveFromContainer();
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = [];
            AddToContainer(sLeaser, rCam, null);
        }

        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(Vector2.Lerp(lastPos, pos, timeStacker) - camPos);
            label.rotation = Mathf.Lerp(lastRot, rot, timeStacker);
            label.alpha = Mathf.InverseLerp(0f, data.MaxLife, Mathf.Lerp(lastLife, lastLife, timeStacker));
        }

        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette) { }

        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            label.RemoveFromContainer();
            newContatiner ??= rCam.ReturnFContainer("Bloom");
            newContatiner.AddChild(label);
        }

        /// <summary>
        /// Data for an Onomatopoeia
        /// </summary>
        /// <param name="color">The color</param>
        /// <param name="vel">The velocity in terms of pixels per cycle (1/40 of a second)</param>
        /// <param name="rotVel">The rotational velocity in terms of degrees per cycle (1/40 of a second)</param>
        /// <param name="scale">The scale of the label (1f = 20px, hard to read below 0.6f)</param>
        /// <param name="life">How long (in seconds) the label will last before it is completely faded</param>
        public readonly struct OnomatopoeiaData(Color color, Vector2 vel, float rotVel, float scale, float life)
        {
            public readonly Color Color = color;
            public readonly Vector2 Vel = vel;
            public readonly float RotVel = rotVel;
            public readonly float Scale = scale;
            public readonly float MaxLife = life;
        }
    }
}
