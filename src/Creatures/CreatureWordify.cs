using System;

namespace WordWorld.Creatures
{
    public abstract class CreatureWordify<T> : Wordify<T> where T : GraphicsModule
    {
        protected CreatureTemplate.Type Type = null;

        public override void Init(T drawable, RoomCamera.SpriteLeaser sLeaser)
        {
            if (drawable.owner is not Creature)
            {
                throw new ArgumentException("`CreatureWordify` must be used with a `GraphicsModule` that has a `Creature` owner!");
            }
            Type = (drawable.owner as Creature).abstractCreature.creatureTemplate.type;
        }
    }
}
