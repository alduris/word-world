using System;
using System.Collections.Generic;
using UnityEngine;

namespace WordWorld
{
    public abstract class Wordify<T> : IWordify where T : IDrawable
    {
        private WeakReference<IDrawable> _drawableRef;
        protected T Obj
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

        public void Init(IDrawable drawable, RoomCamera.SpriteLeaser sLeaser)
        {
            _drawableRef = new WeakReference<IDrawable>(drawable);
            Init(sLeaser);
        }

        public virtual void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser)
        {
            var container = sLeaser.sprites[0].container ?? rCam.ReturnFContainer("Midground");
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
