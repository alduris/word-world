using System;
using System.Collections.Generic;
using UnityEngine;

namespace WordWorld
{
    public abstract class Wordify<T> : IWordify where T : IDrawable
    {
        private WeakReference<IDrawable> _drawableRef;
        protected T Drawable
        {
            get
            {
                if (_drawableRef.TryGetTarget(out var target) && target != null)
                {
                    return (T) target;
                }
                return default;
            }
        }

        public readonly List<FLabel> labels = [];

        public virtual void Init(IDrawable drawable, RoomCamera.SpriteLeaser sLeaser)
        {
            if (drawable is not T)
            {
                throw new ArgumentException($"Cannot create a `{GetType().Name}` without a `{typeof(T).Name}`!");
            }
            _drawableRef = new WeakReference<IDrawable>(drawable);
            Init(sLeaser);
        }

        public void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser) => AddToContainer(rCam, sLeaser, null);

        protected virtual void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser, FContainer container)
        {
            container ??= sLeaser.sprites[0].container ?? rCam.ReturnFContainer("Midground");
            foreach (var label in labels)
            {
                label.alignment = FLabelAlignment.Center;
                container.AddChild(label);
            }
        }

        public void RemoveFromContainer()
        {
            foreach (var label in labels)
            {
                label.RemoveFromContainer();
            }
        }

        public abstract void Init(RoomCamera.SpriteLeaser sLeaser);
        public abstract void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos);
    }
}
