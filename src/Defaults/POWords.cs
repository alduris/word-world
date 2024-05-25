using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    /// <summary>
    /// For generic PhysicalObject stuff. Contains a bunch of default behavior and can be extended for this purpose if needed.
    /// </summary>
    public class POWords : Wordify<IDrawable>
    {
        public POWords() { }
        public POWords(string text)
        {
            this.text = text;
        }

        public string text = null;
        public new PhysicalObject Obj;

        protected FLabel Label {
            get => labels?[0];
            set {
                if (labels.Count > 0)
                {
                    labels[0] = value; 
                }
                else
                {
                    labels.Add(value);
                }
            }
        }

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            if (base.Obj is not PhysicalObject)
            {
                throw new ArgumentException("Cannot create a `POWords` without a `PhysicalObject`!");
            }
            Obj = base.Obj as PhysicalObject;

            text ??= (Obj is Creature ? (Obj as Creature).abstractCreature.creatureTemplate.type.value : Obj.abstractPhysicalObject.type.value);
            Label = new FLabel(Font, text)
            {
                scale = Obj.firstChunk.rad * 3f / FontSize
            };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            Label.SetPosition(GetPos(Obj.firstChunk, timeStacker) - camPos);

            if (Obj is PlayerCarryableItem item)
                Label.color = item.blink > 1 && UnityEngine.Random.value > 0.5f ? item.blinkColor : item.color;
        }
    }
}
