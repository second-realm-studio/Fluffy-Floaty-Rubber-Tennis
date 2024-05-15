namespace Constants {
    public static class BlackboardDataNames {
        public static string PlayerCharacterType(int id) => $"Player{id}CharacterType";
        
        public const string HitCountP1= "HitCountP1";
        public const string HitCountP2= "HitCountP2";
        public const string BallChangedSide= "BallChangedSide";
        // public const string ScoreOffset = "ScoreOffset";
        public const string WinnerName= "WinnerName";
    }
}