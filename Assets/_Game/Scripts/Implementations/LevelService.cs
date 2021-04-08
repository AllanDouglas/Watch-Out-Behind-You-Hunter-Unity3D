using UnityEngine;

namespace WOBH
{
    public class LevelService
    {

        public LevelRecipie GetLevelRecipie(int levelNumber)
        {
            int enemies = Mathf.Min(levelNumber, 9);
            int cartridgesChance = Random.Range(0, 9);
            int cartridges = (enemies > 1 ? cartridgesChance % 3 : ((enemies > 8) ? 1 : 0));
            return new LevelRecipie(enemies: enemies,
                                    foliages: enemies + Random.Range(1, 3),
                                    stones: Random.Range(0, 2),
                                    bushes: Random.Range(0, 2),
                                    cartridges: cartridges);
        }
    }

    public struct LevelRecipie
    {
        public readonly int enemies;
        public readonly int foliages;
        public readonly int stones;
        public readonly int bushes;
        public readonly int cartridges;

        public LevelRecipie(int enemies, int foliages, int stones, int bushes, int cartridges)
        {
            this.enemies = enemies;
            this.foliages = foliages;
            this.stones = stones;
            this.bushes = bushes;
            this.cartridges = cartridges;
        }
    }
}