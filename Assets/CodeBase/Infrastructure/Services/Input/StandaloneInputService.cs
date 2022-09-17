using UnityEngine;
namespace CodeBase.Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 axis
        {
            get
            {
                return GetSimpleInputAxis() == Vector2.zero ? GetUnityAxis() : GetSimpleInputAxis();
            }
        }

        private static Vector2 GetUnityAxis()
        {
            return new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
        }
    }
}
