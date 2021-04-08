using UnityEngine;

namespace WOBH
{
    public class KeyboardInputController : InputControllerAdapter
    {

        [SerializeField] private float sensibility = 10;

        private float horizontal = 180;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DispatchOnFire();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                DispatchOnReload();
                return;
            }

            var vertical = -Input.GetAxisRaw("Vertical");

            if (vertical != 0)
            {
                DispatchOnMovement(new Movement(vertical, horizontal));
                return;
            }

            horizontal += -Input.GetAxisRaw("Horizontal") * sensibility;

            if (horizontal != 0)
            {
                DispatchOnMovement(new Movement(0, horizontal));
                return;
            }
        }
    }
}