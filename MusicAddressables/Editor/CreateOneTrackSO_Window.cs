using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.Video;

namespace MusicAddressables.Editor
{
    public  class CreateOneTrackSO_Window : CreatorTrackSO 
    {
        private AudioClip _audioClip;
        private Sprite _sprite;
        private VideoClip _videoClip;  
         
        private AddressableAssetGroup  _groupToCopy;

        private PlaylistSO _playlistSo;
        private TrackSO _trackSo; 
            
        [MenuItem("Tools/Music Addressables/Create one TrackSO")]
        public static void ShowWindow()
        {
            GetWindow<CreateOneTrackSO_Window>("Track Scriptable Object");
        }

        void OnGUI()
        {
            GUILayout.Label("add to playlist:");
            _playlistSo = (PlaylistSO) EditorGUILayout.ObjectField(_playlistSo, typeof(PlaylistSO));
            GUILayout.Space(10);
        
            _audioClip = (AudioClip) EditorGUILayout.ObjectField(_audioClip, typeof(AudioClip));
            _sprite = (Sprite) EditorGUILayout.ObjectField(_sprite, typeof(Sprite));
            _videoClip = (VideoClip) EditorGUILayout.ObjectField(_videoClip, typeof(VideoClip));
        
            GUILayout.Label("Track file will be created in: ");
            GUILayout.Label( SupposedPath(), EditorStyles.label);

            if (_audioClip != null)
            {
                if (GUILayout.Button("Create Asset"))
                { 
                    _trackSo = CreateInstance<TrackSO>();
                    CreateTrackSOAsset(_audioClip, _sprite, _videoClip, _trackSo);
                }
            }
            else
            {
                GUI.enabled = false; 
                GUILayout.Button(new GUIContent("Create Asset", 
                    "Track need AudioClip at least"));
                GUI.enabled = true;
            }

            GUILayout.Space(10); 
            _groupToCopy = (AddressableAssetGroup) EditorGUILayout.ObjectField(_groupToCopy, typeof(AddressableAssetGroup));

            if (_groupToCopy != null)
            {
                if (GUILayout.Button("Create Addressable Group") && _trackSo != null)
                {
                    CreateAddressableGroup(_trackSo, _groupToCopy);
                    if (_playlistSo != null) _playlistSo.SetNewTrack(_trackSo);
                }
            }
            else
            {
                GUI.enabled = false; 
                GUILayout.Button(new GUIContent("Create Addressable Group", 
                    "Choose existing Addressable Group to copy its settings to a new Group " +
                    "or create new Addressable Group manually from Window->Asset Management->Addressables->Groups"));
                GUI.enabled = true;
            }

            GUILayout.Space(10); 
            if (_trackSo != null)
            {
                if (GUILayout.Button("Calculate Mp3 Size"))
                {
                    CalculateMp3Size(_trackSo);
                }
            }
            else
            {
                GUI.enabled = false; 
                GUILayout.Button(new GUIContent("Calculate Mp3 Size", "no TrackSO assigned"));
                GUI.enabled = true;
            }
        
            GUILayout.Label("Output Scriptable Object");
            _trackSo = (TrackSO) EditorGUILayout.ObjectField(_trackSo, typeof(TrackSO));
        }
    }
}
