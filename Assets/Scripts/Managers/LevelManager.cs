namespace QLE
{
    public enum Level{
        Error = -1, // error level to throw exception
        Lobby, // place where players enter their name before joining a lobby and wait for others to join before starting
        Gameplay,   // singleton and scripts that need to be preloaded before game scene starts
        Credits
    }
    public class LevelManager : LevelManager<Level>
    {
        public override Level GetLevelAfterCutscene(Level savedLevelBeforeCutscene) => Level.Error;
        public override bool IsInGame() => AdditiveScenes[0] == Level.Gameplay;
        public override bool IsLevelACutscene(Level level) => false;
        public override bool IsInMainMenu() => ActiveScenes[0].Level == Level.Lobby;
        protected override void DefineLevelProgression() => progression.Add(Level.Gameplay, Level.Lobby);
    }
}
