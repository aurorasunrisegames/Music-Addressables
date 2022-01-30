using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.Video;

namespace MusicAddressables.Editor
{
    public abstract class CreatorTrackSO  : EditorWindow
    {
        protected void CreateTrackSOAsset(AudioClip audioClip, Sprite sprite, VideoClip videoClip, TrackSO trackSo)
        {
            trackSo.audioClip = audioClip;
            trackSo.sprite = sprite;
            trackSo.videoClip = videoClip;
            if (videoClip != null) trackSo.videoIncluded = true;

            var soPath = SupposedPath() +  trackSo.audioClip.name + ".asset";

            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(soPath);
            AssetDatabase.CreateAsset(trackSo, uniqueFileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = trackSo; 
        }

        protected void CreateAddressableGroup(TrackSO track, AddressableAssetGroup groupToCopy)
        {
            var pathSO = AssetDatabase.GetAssetPath(track);
            var guidSO = AssetDatabase.AssetPathToGUID(pathSO);
            var pathClip = AssetDatabase.GetAssetPath(track.audioClip);
            var guidAudioClip = AssetDatabase.AssetPathToGUID(pathClip); 

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.CreateGroup(track.audioClip.name, false, false, true, groupToCopy.Schemas
                , typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
            var eSo = settings.CreateOrMoveEntry(guidSO, group, false, false);
            var eClip = settings.CreateOrMoveEntry(guidAudioClip, group, false, false); 
            var entriesAdded = new List<AddressableAssetEntry> {eClip, eSo};

            if (track.videoClip != null)
            {
                var guidVideo = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(track.videoClip));
                var eVideo = settings.CreateOrMoveEntry(guidVideo, group, false, false);
                entriesAdded.Add(eVideo);
            }
            
            if (track.sprite != null)
            {
                var guidSprite = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(track.sprite));
                var eSprite = settings.CreateOrMoveEntry(guidSprite, group, false, false);
                entriesAdded.Add(eSprite);
            }

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }

        protected void CalculateMp3Size(TrackSO track)
        {
            track.size = new FileInfo(AssetDatabase.GetAssetPath(track.audioClip)).Length;
            track.size /= 1048576f;
            track.size *= 10;
            track.size = Mathf.Round(track.size) / 10;
        }

        protected static string SupposedPath()
        {
            var aPath = AssetDatabase.GetAssetPath(Selection.activeObject);
         
            if (aPath == "") aPath = "Assets/";
            else
            {
                var directoryName = new FileInfo(aPath).Name;
                aPath = aPath.Substring(0, aPath.Length - directoryName.Length);
            }
             
            return aPath ; 
        }
    
    }
}
