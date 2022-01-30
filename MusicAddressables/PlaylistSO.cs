using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MusicAddressables
{
    [CreateAssetMenu(fileName = "newPlaylist", menuName = "new Playlist of Tracks")] 
    public class PlaylistSO : ScriptableObject
    {
        public string description;
        public Sprite previewImage;
        public List<AssetReference> tracks; 
        public List<string> names;

#if UNITY_EDITOR
        public void SetNewTrack(TrackSO trackSo)
        {
            AssetReference ar = new AssetReference();
            ar.SetEditorAsset(trackSo);
            tracks.Add(ar);
            names.Add(trackSo.audioClip.name);
        }
        public void RefillNamesList()
        {
            names.Clear();
            foreach (var t in tracks)
            {
                if(t.editorAsset != null) 
                    names.Add(t.editorAsset.name);
                else
                    Debug.LogAssertion("some assetReferences are not assigned!");
            }
        }  
#endif
    
    }
}
