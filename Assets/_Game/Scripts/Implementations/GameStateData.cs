namespace WOBH
{
    public struct GameStateData
    {
        public readonly int score;
        public readonly int bestScore;

        public GameStateData(int score, int bestScore)
        {
            this.score = score;
            this.bestScore = bestScore;
        }
    }
}