using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(CapaciteData))]
public class CapaciteDataEditor : Editor
{
    private Type[] capacityTypes;
    private string[] capacityTypeNames;

    private void OnEnable()
    {
        capacityTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Capacity)) && !type.IsAbstract)
            .ToArray();

        capacityTypeNames = capacityTypes.Select(type => type.Name).Prepend("None").ToArray();
    }

    public override void OnInspectorGUI()
    {
        CapaciteData capaciteData = (CapaciteData)target;

        DrawDefaultInspector();

        EditorGUILayout.LabelField("Capacité Concrète", EditorStyles.boldLabel);

        int selectedIndex = 0;
        if (capaciteData.capacity != null)
        {
            Type currentType = capaciteData.capacity.GetType();
            selectedIndex = Array.IndexOf(capacityTypes, currentType) + 1;
        }

        int newIndex = EditorGUILayout.Popup("Type de Capacité", selectedIndex, capacityTypeNames);

        if (newIndex != selectedIndex)
        {
            if (newIndex == 0)
            {
                capaciteData.capacity = null;
            }
            else
            {
                Type selectedType = capacityTypes[newIndex - 1];
                var newCapacity = (Capacity)Activator.CreateInstance(selectedType);
                newCapacity.Initialize(capaciteData);
                capaciteData.capacity = newCapacity;
            }

            EditorUtility.SetDirty(target);
        }
    }
}
