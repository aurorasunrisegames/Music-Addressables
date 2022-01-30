using UnityEditor;
using UnityEngine;

namespace MusicAddressables.Editor
{
    [CustomEditor(typeof(PlaylistSO))]
    [CanEditMultipleObjects]
    public class PlaylistEditor : UnityEditor.Editor
    {
        private int notFilledCount = 0;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PlaylistSO playlistSo = (PlaylistSO) target;
        
            if (GUILayout.Button("fill Names list"))
            {
                notFilledCount = 0;
                playlistSo.names.Clear();
                foreach (var t in playlistSo.tracks)
                {
                    if(t.editorAsset != null)
                        playlistSo.names.Add(t.editorAsset.name);
                    else
                    {
                        notFilledCount++;
                    }
                }
            }

            if (notFilledCount > 0)
            {
                GUILayout.Label(notFilledCount + " objects need to assign");
                GUILayout.Label( "! works only with the full name list !");
                playlistSo.names.Clear();
            }
            
        }
    }
}
