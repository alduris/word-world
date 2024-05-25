using System;

namespace WordWorld.Creatures
{
    public abstract class CreatureWordify<T> : Wordify<T> where T : GraphicsModule
    {
        protected CreatureTemplate.Type Type = null;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            if (Obj.owner is not Creature)
            {
                throw new ArgumentException("`CreatureWordify` must be used with a `GraphicsModule` that has a `Creature` owner!");
            }
            Type = (Obj.owner as Creature).abstractCreature.creatureTemplate.type;
        }
    }
}
