using System.Collections.Generic;
using UnityEngine;

namespace WOBH
{
    public class AmmunitionSpawner : MonoBehaviour
    {
        [SerializeField] private Ammunition ammunitionPrefab;
        [SerializeField] private Transform[] spawnPoints;

        [Header("Debug")]
        [SerializeField] private Color gizmosColor;

        Spawner<Ammunition> spawner;

        public void Recycle(Ammunition ammunition)
        {
            ammunition.gameObject.SetActive(false);
            spawner.Recycle(ammunition);
        }

        public List<Ammunition> Spawn(int amount)
        {
            var list = new List<Ammunition>(amount);

            for (int i = 0; i < amount; i++)
            {
                var ammunition = spawner.Spawn();

                ammunition.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                ammunition.gameObject.SetActive(true);
                list.Add(ammunition);
            }

            return list;
        }
        private void Awake()
        {
            spawner = new Spawner<Ammunition>(ammunitionPrefab);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var originColor = Gizmos.color;
            Gizmos.color = gizmosColor;
            foreach (var item in spawnPoints)
            {
                Gizmos.DrawSphere(item.position, .2f);
            }
            Gizmos.color = originColor;
        }
#endif
    }
}