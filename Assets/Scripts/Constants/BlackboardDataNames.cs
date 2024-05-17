namespace Constants {
    public static class BlackboardDataNames {
        public static string PlayerCharacterType(int id) => $"Player{id}CharacterType";
        
        public const string EntityIdP1 = "EntityIdP1";
        public const string EntityIdP2 = "EntityIdP2";
        public const string EntityIdBall = "EntityIdBall";
        public const string HitCountP1= "HitCountP1";
        public const string HitCountP2= "HitCountP2";
        public const string BallChangedSide= "BallChangedSide";
        // public const string ScoreOffset = "ScoreOffset";
        public const string WinnerName= "WinnerName";
        public const string BallVelocity = "BallVelocity";
    }
}