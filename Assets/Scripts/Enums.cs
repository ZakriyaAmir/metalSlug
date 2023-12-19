namespace RunAndGun.Space
{
    public class Enums
    {

    }

    public enum GameResultValueType
    {
        Points,
        EnemiesKilled,
        CurrentHealthPoints,
        CurrentAmmoRounds,
        TimeSpent,
    }

    public enum GameState
    {
        OnMainMenu,
        InGamePaused,
        InGameActive,
        PlayerDead,
        LevelVictory,
        LevelGameOver
    }

    public enum DamagerType
    {
        Melee,
        Projectile
    }

    public enum EnemySpotState
    {
        NoTarget,
        TargetIsVisible,
        AlertedOnTarget,
        TargetLost
    }

    public enum AudioType
    {
        SoundEffectType,
        MusicType,
    }
}