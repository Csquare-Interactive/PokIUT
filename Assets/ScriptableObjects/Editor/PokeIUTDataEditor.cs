using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PokeIUTData))]
public class PokeIUTDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Affiche l'inspecteur par défaut
        DrawDefaultInspector();

        // Référence à l'objet cible
        PokeIUTData pokeIUTData = (PokeIUTData)target;

        // Ajoute un espace dans l'inspecteur
        GUILayout.Space(10);

        // Bouton pour réinitialiser les capacités
        if (GUILayout.Button("Réinitialiser les Capacités"))
        {
            // Enregistre l'état actuel pour l'Undo
            Undo.RecordObject(pokeIUTData, "Réinitialiser les Capacités");

            // Appelle la méthode de réinitialisation
            pokeIUTData.ResetPokeIUT();

            // Marque l'objet comme modifié pour que les changements soient enregistrés
            EditorUtility.SetDirty(pokeIUTData);
        }
    }
}
