using UnityEngine;

namespace QLE
{
    public enum MusicId{
        MainMenu,
        Game,
        Credits
    }
    public class MusicPlayer : MusicPlayer<MusicId>
    {
        public override void PlayMusic()
        {
            if(audioSource.isPlaying) audioSource.Stop();
            audioSource.clip = audioClips[(int)LevelManager.MasterScene];
            audioSource.Play();
        }
    }
}
