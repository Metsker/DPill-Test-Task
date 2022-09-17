using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMovement : MonoBehaviour, IDisabledOnDeath
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float speed = 4;
        private Camera _camera;

        private IInputService _inputService;

        private void Awake()
        {
            _inputService = AllServices.container.Single<IInputService>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.axis);
                movementVector.y = 0;
                movementVector.Normalize();
                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            characterController.Move(movementVector * (speed * Time.deltaTime));
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}
