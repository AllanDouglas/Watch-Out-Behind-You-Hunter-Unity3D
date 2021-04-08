using UnityEngine;

namespace WOBH
{
    public struct EntryMapData
    {
        public readonly Exit lastExit;
        public readonly Exit currentExit;

        public EntryMapData(Exit currentExit, Exit lastExit) : this()
        {
            this.currentExit = currentExit;
            this.lastExit = lastExit;
        }
    }

    public class ExitController : MonoBehaviour
    {
        private Exit[] exits;

        private void Awake()
        {
            exits = GetComponentsInChildren<Exit>();
        }

        internal void OpenAll()
        {
            foreach (var exit in exits)
            {
                exit.Open();
            }
        }

        internal bool HasEntry(Exit exit, out Exit entry)
        {
            entry = null;
            foreach (var door in exits)
            {
                var dot = Vector2.Dot(exit.transform.right, door.transform.right);

                if (Mathf.Approximately(dot, -1))
                {
                    entry = door;
                    return true;
                }
            }
            return false;
        }
    }
}