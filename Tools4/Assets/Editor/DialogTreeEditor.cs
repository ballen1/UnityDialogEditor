using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogTreeEditor : EditorWindow {

	public DialogTree tree;

	[MenuItem("Dialog/Tree Editor")]
	static void Init() {
		EditorWindow.GetWindow<DialogTreeEditor>("Dialog Tree Editor");
	}

	void OnGUI() {

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Dialog Tree Editor", EditorStyles.boldLabel);

		if (GUILayout.Button("Open Dialog Tree")) {
			OpenDialogTree ();
		}

		if (GUILayout.Button ("New Dialog Tree")) {
			CreateDialogTree ();
		}

		GUILayout.EndHorizontal ();

	}

	private void OpenDialogTree() {

		string path = EditorUtility.OpenFilePanel ("Choose Dialog Tree", "", "");

		if (path.Length > 0) {

			if (path.StartsWith (Application.dataPath)) {
				path = pathRelToAssetsDir(path);
				tree = AssetDatabase.LoadAssetAtPath<DialogTree> (path);
			}

		}

	}

	private void CreateDialogTree() {

		string path = EditorUtility.OpenFolderPanel ("Choose folder for Dialog Tree", "", "");

		if (path.Length > 0) {
			tree = ScriptableObject.CreateInstance<DialogTree> ();

			if (path.StartsWith(Application.dataPath)) {

				path = pathRelToAssetsDir(path);

				AssetDatabase.CreateAsset (tree, path + System.IO.Path.DirectorySeparatorChar + "DialogTree.asset");
				AssetDatabase.SaveAssets ();

			}
		}

	}

	private string pathRelToAssetsDir(string path) {
		return path.Substring (Application.dataPath.Length - "Assets".Length);
	}

}
