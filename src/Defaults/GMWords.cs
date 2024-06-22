using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    /// <summary>
    /// For generic GraphicsModule objects
    /// </summary>
    public class GMWords : Wordify<GraphicsModule>
    {
        protected FLabel Label
        {
            get => labels?[0];
            set
            {
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
            var name = Drawable.owner is Creature creature ? creature.abstractCreature.creatureTemplate.type.value : Drawable.owner.abstractPhysicalObject.type.value;
            Label = new FLabel(Font, name);

            Label.scale = Drawable.owner.bodyChunks.Max(chunk => chunk.rad) * 3f / TextWidth(Label.text);
            Label.color = sLeaser.sprites[0].color;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var pos = GetPos(Drawable.owner.bodyChunks[0], timeStacker) - camPos;
            var rot = sLeaser.sprites[0].rotation;

            Label.SetPosition(pos);
            Label.rotation = rot;
        }
    }
}
