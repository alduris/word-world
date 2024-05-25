using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    public class POWords : Wordify<IDrawable>
    {
        public POWords() { }
        public POWords(string text)
        {
            this.text = text;
        }

        public string text = null;
        public new PhysicalObject Obj;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            if (base.Obj is not PhysicalObject)
            {
                throw new ArgumentException("Cannot create a `POWords` without a `PhysicalObject`!");
            }
            Obj = base.Obj as PhysicalObject;

            text ??= (Obj is Creature ? (Obj as Creature).abstractCreature.creatureTemplate.type.value : Obj.abstractPhysicalObject.type.value);
            var label = new FLabel(Font, text)
            {
                scale = Obj.firstChunk.rad * 3f / FontSize
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(Obj.firstChunk, timeStacker) - camPos);

            if (Obj is PlayerCarryableItem item)
                labels[0].color = item.blink > 1 && UnityEngine.Random.value > 0.5f ? item.blinkColor : item.color;
        }
    }
}
