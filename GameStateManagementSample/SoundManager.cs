using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameStateManagement
{
    class SoundManager
    {
        private static SoundManager soundManager;
        //public Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public List<Song> songs = new List<Song>();
        public Dictionary<string, SoundEffectInstance> soundEffects = new Dictionary<string, SoundEffectInstance>();

        public static SoundManager Audio()
        {
            if (soundManager == null)
                soundManager = new SoundManager();
            
            return soundManager;
        }

        private SoundManager()
        {
        }
            
        public void LoadContent(ContentManager Content)
        {
            Song song;
            SoundEffect effect;
            SoundEffectInstance effectInstance;

            MediaPlayer.IsRepeating = true;

            songs.Clear();
            soundEffects.Clear();

            foreach (string songName in new string[] { "Silhouette", "Just For You", "128 Heartbeats", "DST-AngryMod", "DST-Flak", "DST-FlexT", "DST-TempleElectric", "DST-XToFly", "DST-ClubNight" })
            {
                song = Content.Load<Song>(songName);
                songs.Add(song);
            }

            foreach (string effectName in new string[] { "laser_shooting_sfx" })
            {
                effect = Content.Load<SoundEffect>(effectName);
                effectInstance = effect.CreateInstance();
                soundEffects.Add(effectName, effectInstance);
            }
        }

        public void PlaySong(int song)
        {
            MediaPlayer.Play(songs[song]);
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        public void SetLooping(bool isRepeating)
        {
            MediaPlayer.IsRepeating = isRepeating;
        }

        public void PlayEffect(string effectName)
        {
            soundEffects[effectName].Play();
        }

        public void StopEffect(string effectName)
        {
            soundEffects[effectName].Stop();
        }
    }
}