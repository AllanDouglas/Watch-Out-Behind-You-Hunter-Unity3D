using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WOBH
{
    public class JoystickInputController : InputControllerAdapter
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private Button reloadButton;
        [SerializeField] private EventTrigger fireEventTrigger;

        private void Awake()
        {
            reloadButton.onClick.AddListener(DispatchOnReload);

            var triggerEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            triggerEntry.callback.AddListener(d => DispatchOnFire());

            fireEventTrigger.triggers.Add(triggerEntry);

        }

        private void Update()
        {
            if (joystick.Direction.sqrMagnitude <= joystick.DeadZone * joystick.DeadZone) return;

            Vector2 direction = joystick.Direction;

            var angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

            base.DispatchOnMovement(new Movement(
                direction: direction.sqrMagnitude * -1,
                rotation: angle
            ));


        }

    }
}