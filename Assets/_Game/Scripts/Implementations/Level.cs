using System;
using UnityEngine;

namespace WOBH
{
    [RequireComponent(typeof(ExitController))]
    public class Level : MonoBehaviour
    {
        private ExitController exitController;

        public bool HasEntry(Exit exit, out Exit entry) => exitController.HasEntry(exit, out entry);

        private void Awake() => exitController = GetComponent<ExitController>();

        internal void OpenAllDoors()
        {
            exitController.OpenAll();
        }
    }
}