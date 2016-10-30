using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DialogTreeEditor : EditorWindow {

	public DialogTree tree;
	private bool nodeView = true;
	private bool treeView = false;
	private List<bool> collapse = new List<bool> ();
	private int popupIndex = 0;
	private int editingIndex = 0;

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

			if (tree.treeNodes.Count != 0) {
				popupIndex = tree.treeNodes.IndexOf (tree.root);
				if (popupIndex == -1) {
					popupIndex = 0;
				}
			} else {
				popupIndex = 0;
			}


			// Collapse values for foldouts
			while (collapse.Count < tree.treeNodes.Count) {
				collapse.Add (false);
			}
			
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

			EditorUtility.SetDirty (tree);

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

			int nodeToDelete = -1;

			for (int i = 0; i < tree.treeNodes.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				collapse [i] = EditorGUILayout.Foldout (collapse [i], tree.treeNodes[i].name);
				EditorGUILayout.EndHorizontal ();

				if (collapse [i]) {
					EditorGUILayout.Space ();
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Node Name", GUILayout.MaxWidth(100));
					tree.treeNodes [i].name = EditorGUILayout.TextField (tree.treeNodes [i].name);
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("String Key", GUILayout.MaxWidth(100));
					tree.treeNodes [i].textKey = EditorGUILayout.TextField (tree.treeNodes [i].textKey);
					EditorGUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					GUILayout.FlexibleSpace ();
					if (GUILayout.Button ("Delete Node", EditorStyles.miniButtonRight, GUILayout.MaxWidth (100))) {
						nodeToDelete = i; 
					}
					GUILayout.EndHorizontal ();
					EditorGUILayout.Space ();
				}
			}

			if (nodeToDelete != -1) {
				tree.treeNodes.RemoveAt (nodeToDelete);
				collapse.RemoveAt (nodeToDelete);
				tree.treeNodes.TrimExcess ();
				collapse.TrimExcess ();
			}

		}

		// Button to create new node
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button ("New Node")) {
			DialogNode newNode = new DialogNode();
			tree.treeNodes.Add (newNode);
			collapse.Add (true);
		}

		EditorGUILayout.EndHorizontal ();

	}

	private void displayTreeView() {

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();

		string[] options = getPopupOptions ();

		EditorGUILayout.LabelField ("Root Node", EditorStyles.boldLabel);
		popupIndex = EditorGUILayout.Popup (popupIndex, options);

		// Set root node
		if (tree.treeNodes.Count != 0) {
			tree.root = tree.treeNodes [popupIndex];
		}

		EditorGUILayout.EndHorizontal ();

		displayTreeNavigation ();

	}

	private void displayTreeNavigation() {

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();

		string[] options = getPopupOptions ();
		EditorGUILayout.LabelField ("Node Navigation", EditorStyles.boldLabel);

		if (tree.treeNodes.Count == 0) {
			editingIndex = 0;
		}

		editingIndex = EditorGUILayout.Popup (editingIndex, options);

		if (GUILayout.Button ("<<") && tree.treeNodes.Count != 0) {
			editingIndex = (--editingIndex + tree.treeNodes.Count) % tree.treeNodes.Count;
		}

		if (GUILayout.Button (">>") && tree.treeNodes.Count != 0) {
			editingIndex = ++editingIndex % tree.treeNodes.Count;
		}

		EditorGUILayout.EndHorizontal ();

	}

	private string[] getPopupOptions() {

		string[] options;

		if (tree.treeNodes.Count == 0) {
			options = new string[] {"No Nodes"};
		} else {
			List<string> optionList = new List<string> ();
			for (int i = 0; i < tree.treeNodes.Count; i++) {
				if (optionList.Contains (tree.treeNodes [i].name)) {
					optionList.Add (tree.treeNodes [i].name + "_" + i +" (duplicate name)");
				} else {
					optionList.Add(tree.treeNodes [i].name);
				}
			}
			options = optionList.ToArray ();
		}


		return options;

	}

}
