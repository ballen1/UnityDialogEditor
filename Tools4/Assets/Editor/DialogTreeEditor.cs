using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DialogTreeEditor : EditorWindow {

	public DialogTree tree;
	private bool nodeView = true;
	private bool treeView = false;

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

		if (tree != null) {
			
			GUILayout.BeginHorizontal ();

			// Node View
			if (GUILayout.Button ("Node View")) {
				nodeView = true;
				treeView = false;
			}
			if (GUILayout.Button ("Tree View")) {
				nodeView = false;
				treeView = true;
			}

			GUILayout.EndHorizontal ();

			if (nodeView) {
				displayNodeView ();
			}

			if (treeView) {
				displayTreeView ();
			}

		}
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

				tree.treeNodes = new List<DialogNode> ();

				AssetDatabase.CreateAsset (tree, path + System.IO.Path.DirectorySeparatorChar + "DialogTree.asset");
				AssetDatabase.SaveAssets ();

			}
		}

	}

	private string pathRelToAssetsDir(string path) {
		return path.Substring (Application.dataPath.Length - "Assets".Length);
	}


	private void displayNodeView() {

		// No nodes to display
		if (tree.treeNodes.Count == 0) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("No nodes in dialog tree.");
			EditorGUILayout.EndHorizontal ();
		}
		// Display nodes
		else {
			for (int i = 0; i < tree.treeNodes.Count; i++) {

			}
		}

		// Button to create new node
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button ("New Node")) {
			tree.treeNodes.Add (new DialogNode ());
		}

		EditorGUILayout.EndHorizontal ();

	}

	private void displayTreeView() {

	}

}
