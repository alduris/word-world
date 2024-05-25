using UnityEngine;

namespace WordWorld
{
    public interface IWordify
    {
        /// <summary>
        /// Initialize and style your FLabels here
        /// </summary>
        /// <param name="drawable">The IDrawable</param>
        /// <param name="sLeaser">The sprite leaser</param>
        public void Init(IDrawable drawable, RoomCamera.SpriteLeaser sLeaser);

        /// <summary>
        /// Move and etc your FLabels here
        /// </summary>
        /// <param name="sLeaser">The IDrawable</param>
        /// <param name="timeStacker">How far through the current update cycle we are from 0 to 1</param>
        /// <param name="camPos">Camera position offset</param>
        public void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos);

        /// <summary>
        /// What containers to add your labels to
        /// </summary>
        /// <param name="rCam">The room camera</param>
        /// <param name="sLeaser">The sprite leaser</param>
        public void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser);
        public void RemoveFromContainer();
    }
}
