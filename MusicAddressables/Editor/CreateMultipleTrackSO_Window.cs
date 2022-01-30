using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Video;

namespace MusicAddressables.Editor
{
    public class CreateMultipleTrackSO_Window : CreatorTrackSO
    {
        private List<AudioClip> _audioClips;
        private List<Sprite> _sprites;
        private List<VideoClip> _videoClips; 
        private AudioClip _tempAudioClip;
        private Sprite _tempSprite;
        private VideoClip _tempVideoClip;
        
        private AddressableAssetGroup _groupToCopy;

        private PlaylistSO _playlistSo;
        private List<TrackSO> _tracksSo;
    
        private Vector2 scrollVector2 = Vector2.zero;
        private bool checkbox;
    
        [MenuItem("Tools/Music Addressables/Create multiple TrackSO")]
        public static void ShowWindow()
        {
            GetWindow<CreateMultipleTrackSO_Window>("Create multiple TrackSO"); 
        }
        void OnGUI()
        {
            if (_audioClips == null)
                InitArrays();
            
            GUILayout.BeginHorizontal(); 
            GUILayout.Space(5);
            GUILayout.Label("add to playlist:");
            _playlistSo = (PlaylistSO) EditorGUILayout.ObjectField(_playlistSo, typeof(PlaylistSO));
            GUILayout.EndHorizontal();
         
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            
            scrollVector2 = GUILayout.BeginScrollView(scrollVector2);
            for (int i = 0; i < _audioClips.Count; i++)                                  //Input fields
                InLineTrackFields(i);
            GUILayout.Space(7);
            InLineTrackFields();
            GUILayout.EndScrollView();

            GUILayout.Label("Track files will be created in: ");
            GUILayout.Label( SupposedPath(), EditorStyles.label);
        
            if (_audioClips.All(a => a != null) && _audioClips.Count > 0)
            {
                if (GUILayout.Button("Create multiple Assets"))
                {
                    for (int i = 0; i < _audioClips.Count; i++)
                    {  
                        _tracksSo.Add(CreateInstance<TrackSO>());
                        CreateTrackSOAsset(_audioClips[i], _sprites[i], _videoClips[i], _tracksSo[i]);
                    }
                }
            }
            else
            {
                GUI.enabled = false; 
                GUILayout.Button(new GUIContent("Create multiple Assets", 
                    "Some of Tracks have no AudioClip assigned"));
                GUI.enabled = true;
            }
            
        
            GUILayout.Space(10);
            _groupToCopy = (AddressableAssetGroup) EditorGUILayout.ObjectField(_groupToCopy, typeof(AddressableAssetGroup));
            
            if (_groupToCopy != null)
            {
                if (GUILayout.Button("Create multiple Addressable Groups"))
                {
                    for (int i = 0; i < _audioClips.Count; i++)
                    {
                        if (_tracksSo[i] != null)
                        {
                            CreateAddressableGroup(_tracksSo[i], _groupToCopy);
                            if (_playlistSo != null) _playlistSo.SetNewTrack(_tracksSo[i]);
                        }
                    }
                }
            }
            else
            {
                GUI.enabled = false; 
                GUILayout.Button(new GUIContent("Create multiple Addressable Groups", 
                    "Choose existing Addressable Group to copy its settings to a new Group " +
                    "or create new Addressable Group manually from Window->Asset Management->Addressables->Groups"));
                GUI.enabled = true;
            }

            GUILayout.Space(10); 
            if (_tracksSo.All(t => t != null) && _tracksSo.Count > 0)
            {
                if (GUILayout.Button("Calculate multiple Mp3 Sizes"))
                {
                    foreach (var t in _tracksSo)
                    {
                        CalculateMp3Size(t);
                    }
                }
            }
            else
            {
                GUI.enabled = false; 
                GUILayout.Button(new GUIContent("Calculate multiple Mp3 Sizes", "each TrackSO must be not null"));
                GUI.enabled = true;
            }

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Refill Playlist"))
            {
                if(_playlistSo != null)
                    _playlistSo.RefillNamesList();
                else 
                    GUILayout.Label("assign the Playlist" );
            }
            checkbox = GUILayout.Toggle(checkbox,new GUIContent("checkbox", "expand output fields"));
            GUILayout.EndHorizontal();
 
            GUILayout.BeginScrollView(scrollVector2);
            int fieldsCounter = 0;
            if (checkbox) GUILayout.BeginHorizontal();
            for (int i = 0; i < _tracksSo.Count; i++)                  // Output fields 
            {
                if (fieldsCounter > 2 && checkbox)
                {
                    fieldsCounter = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                fieldsCounter++;
                _tracksSo[i] = (TrackSO) EditorGUILayout.ObjectField(_tracksSo[i], typeof(TrackSO));
            }
            GUILayout.EndScrollView();
        }

        private void InitArrays()
        {
            _audioClips = new List<AudioClip>();
            _sprites = new List<Sprite>();
            _videoClips = new List<VideoClip>();
            _tracksSo = new List<TrackSO>();
        }

        private void InLineTrackFields(int line)
        {
            GUILayout.BeginHorizontal();
            _audioClips[line] = (AudioClip) EditorGUILayout.ObjectField(_audioClips[line], typeof(AudioClip));
            _sprites[line] = (Sprite) EditorGUILayout.ObjectField(_sprites[line], typeof(Sprite));
            _videoClips[line] = (VideoClip) EditorGUILayout.ObjectField(_videoClips[line], typeof(VideoClip));
            GUILayout.EndHorizontal();
        }
        private void InLineTrackFields()
        {
            GUILayout.BeginHorizontal(); 
            _tempAudioClip = (AudioClip) EditorGUILayout.ObjectField(_tempAudioClip, typeof(AudioClip));
            _tempSprite = (Sprite) EditorGUILayout.ObjectField(_tempSprite, typeof(Sprite));
            _tempVideoClip = (VideoClip) EditorGUILayout.ObjectField(_tempVideoClip, typeof(VideoClip));
            if(_tempAudioClip != null)
            {
                _audioClips.Add(_tempAudioClip);
                _sprites.Add(_tempSprite);
                _videoClips.Add(_tempVideoClip);
                _tempAudioClip = null;
                _tempSprite = null;
                _tempVideoClip = null;
            }
            
            GUILayout.EndHorizontal();
        }
    }
}
