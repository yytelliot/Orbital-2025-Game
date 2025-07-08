// Assets/Scripts/Editor/GameEventIDAssigner.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameEventIDAssigner : AssetPostprocessor
{
    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var path in importedAssets)
        {
            if (!path.EndsWith(".asset")) continue;
            var evt = AssetDatabase.LoadAssetAtPath<GameEvent>(path);
            if (evt == null) continue;

            if (evt.eventId < 0)
            {
                // 1) Gather all used IDs
                var usedIds = new HashSet<int>();
                foreach (var guid in AssetDatabase.FindAssets("t:GameEvent"))
                {
                    var p = AssetDatabase.GUIDToAssetPath(guid);
                    var other = AssetDatabase.LoadAssetAtPath<GameEvent>(p);
                    if (other != null && other.eventId >= 0)
                        usedIds.Add(other.eventId);
                }

                // 2) Find the first free integer (starting at 0)
                int freeId = 0;
                while (usedIds.Contains(freeId))
                    freeId++;

                // 3) Assign it
                evt.eventId = freeId;
                EditorUtility.SetDirty(evt);
            }
        }
    }
}
