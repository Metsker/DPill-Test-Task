using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";
        private const string BattleFieldCornerTag = "BattleFieldCorner";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                levelData.levelKey = SceneManager.GetActiveScene().name;
                levelData.initialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;

                List<GameObject> corners = GameObject.FindGameObjectsWithTag(BattleFieldCornerTag).ToList();

                levelData.battleFieldData = new BattleFieldData(
                    corners.Min(c => c.transform.position.x),
                    corners.Max(c => c.transform.position.x),
                    corners.Min(c => c.transform.position.z),
                    corners.Max(c => c.transform.position.z));
            }

            EditorUtility.SetDirty(target);
        }
    }
}
