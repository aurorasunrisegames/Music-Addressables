using UnityEngine;
using UnityEngine.Video;

namespace MusicAddressables
{
    [CreateAssetMenu(fileName = "newTrack", menuName = "new Track to Playlist")] 
    public class TrackSO : ScriptableObject
    {
        [Tooltip("Make sure all audioClip.preloadAudioData = false")]
        public AudioClip audioClip;
        public Sprite sprite;
        public VideoClip videoClip;
    
        [HideInInspector]
        public bool videoIncluded;
        public float size; 
    }
}
