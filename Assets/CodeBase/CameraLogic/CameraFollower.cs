using UnityEngine;
namespace CodeBase.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private float rotationAngleX = 55;
        [SerializeField] private float distance = 10;
        [SerializeField] private float offsetY = 0.5f;

        private Transform _following;

        private void LateUpdate()
        {
            if (_following == null)
                return;

            Quaternion rotation = Quaternion.Euler(rotationAngleX, 0, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + GetFollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject followingObject)
        {
            _following = followingObject.transform;
        }

        private Vector3 GetFollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += offsetY;
            return followingPosition;
        }
    }
}
