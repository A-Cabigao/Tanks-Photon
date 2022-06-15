using UnityEngine;

namespace QLE
{
    [CreateAssetMenu(fileName = "Level Event Handler", menuName = "ScriptableObjects/Level Event Handler")]
    public class LevelEventHandler : LevelEventSO<Level>
    {
        public override void LoadCredits() => LevelManager.Instance.LoadMasterScene(Level.Lobby);
    }
}
