namespace WOBH
{
    public struct LevelResult
    {
        public readonly int level;
        public readonly int totalEnemies;
        public readonly int enemiesDead;
        public readonly int bonus;
        public readonly Exit exit;
        public readonly int currentlevel;
        public readonly LevelRecipie nextLevel;
        public readonly int score;

        public LevelResult(int level,
                           int totalEnemies,
                           int enemiesDead,
                           Exit exit,
                           int bonus,
                           int currentlevel,
                           LevelRecipie nextLevel,
                           int score)
        {
            this.level = level;
            this.totalEnemies = totalEnemies;
            this.enemiesDead = enemiesDead;
            this.exit = exit;
            this.bonus = bonus;
            this.currentlevel = currentlevel;
            this.nextLevel = nextLevel;
            this.score = score;
        }
    }
}