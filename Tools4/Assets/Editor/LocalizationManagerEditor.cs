using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor {

	SerializedObject lm;

	SerializedProperty translationUnit;
	SerializedProperty languageUnit;

	private int selectedIndex = 0;

	void OnEnable() {
		translationUnit = serializedObject.FindProperty ("selectedTranslationUnit");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update ();

		EditorGUILayout.PropertyField (translationUnit);

		TranslationUnit tu = (TranslationUnit)translationUnit.objectReferenceValue;
		
		if (tu != null) {
			string[] languages = tu.getAvailableLanguages ();

			selectedIndex = EditorGUILayout.Popup (selectedIndex, languages);
			LocalizationManager.SetLanguageIndex (selectedIndex);
		}

		serializedObject.ApplyModifiedProperties ();

	}

}
