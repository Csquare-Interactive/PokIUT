using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PokeIUTData))]
public class PokeIUTDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PokeIUTData pokeIUTData = (PokeIUTData)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Réinitialiser les Données"))
        {
            Undo.RecordObject(pokeIUTData, "Réinitialiser les Données");
            pokeIUTData.ResetPokeIUT();
            EditorUtility.SetDirty(pokeIUTData);
        }
    }
}
