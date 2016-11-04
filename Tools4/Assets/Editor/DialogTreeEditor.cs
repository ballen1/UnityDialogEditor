using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DialogTreeEditor : EditorWindow {

	public DialogTree tree;
	private bool nodeView = true;
	private bool treeView = false;
	private List<bool> collapse = new List<bool> ();
	private List<bool> optionCollapse = new List<bool>();
	private int popupIndex = 0;
	private int editingIndex = 0;
	private List<int> optionIndices = new List<int> ();

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
			}

		}

		// Button to create new node
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button ("New Node")) {
			tree.addNewDialogNode ();
			collapse.Add (true);
		}

		EditorGUILayout.EndHorizontal ();

	}

	private void displayTreeView() {

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();

		string[] options = getPopupOptions (false);

		if (tree.treeNodes.Count != 0) {

			popupIndex = tree.treeNodes.FindIndex (
				delegate(DialogNode node)
				{
					return (node.GID == tree.root);
				}
			);
				
			if (popupIndex == -1) {
				popupIndex = 0;
			}
		} else {
			popupIndex = 0;
		}
			
		EditorGUILayout.LabelField ("Root Node", EditorStyles.boldLabel);
		popupIndex = EditorGUILayout.Popup (popupIndex, options);

		// Set root node
		if (tree.treeNodes.Count != 0) {
			tree.root = tree.treeNodes [popupIndex].GID;
		}

		EditorGUILayout.EndHorizontal ();

		displayTreeNavigation ();

	}

	private void displayTreeNavigation() {

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();

		string[] options = getPopupOptions (false);
		EditorGUILayout.LabelField ("Node Navigation", EditorStyles.boldLabel);

		if (tree.treeNodes.Count == 0) {
			editingIndex = 0;
		}

		int currentEditingIndex = editingIndex;
		editingIndex = EditorGUILayout.Popup (editingIndex, options);

		if (currentEditingIndex != editingIndex) {
			optionCollapse.Clear ();
			optionIndices.Clear ();
		}

		if (GUILayout.Button ("<<") && tree.treeNodes.Count != 0) {
			editingIndex = (--editingIndex + tree.treeNodes.Count) % tree.treeNodes.Count;
			GUI.FocusControl (null);
			optionCollapse.Clear ();
			optionIndices.Clear ();
		}

		if (GUILayout.Button (">>") && tree.treeNodes.Count != 0) {
			editingIndex = ++editingIndex % tree.treeNodes.Count;
			GUI.FocusControl (null);
			optionCollapse.Clear ();
			optionIndices.Clear ();
		}

		EditorGUILayout.EndHorizontal ();

		displayNodeEditor ();

	}

	private void displayNodeEditor() {

		if (tree.treeNodes.Count > 0) {

			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Edit Node Information", EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Node Name", GUILayout.MaxWidth (100));
			tree.treeNodes [editingIndex].name = EditorGUILayout.TextField (tree.treeNodes [editingIndex].name);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("String Key", GUILayout.MaxWidth (100));
			tree.treeNodes [editingIndex].textKey = EditorGUILayout.TextField (tree.treeNodes [editingIndex].textKey);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Dialog Options", EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal ();

			// Collapse values for foldouts
			while (optionCollapse.Count < tree.treeNodes[editingIndex].dialogOptions.Count) {
				optionCollapse.Add (true);
			}

			while (optionIndices.Count < tree.treeNodes [editingIndex].dialogOptions.Count) {
				optionIndices.Add (0);
			}

			int optionToDelete = -1;

			for (int i = 0; i < tree.treeNodes [editingIndex].dialogOptions.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				optionCollapse[i] = EditorGUILayout.Foldout (optionCollapse [i], "Option " + (i + 1));
				EditorGUILayout.EndHorizontal ();

				if (optionCollapse [i]) {
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("String Key", GUILayout.MaxWidth(100));
					tree.treeNodes [editingIndex].dialogOptions[i].textKey = EditorGUILayout.TextField (tree.treeNodes [editingIndex].dialogOptions[i].textKey);
					EditorGUILayout.EndHorizontal ();

					// Popup menu  for selecting the next node
					EditorGUILayout.BeginHorizontal();

					if (tree.treeNodes[editingIndex].dialogOptions[i].nextNode != null) {

						if (tree.treeNodes [editingIndex].dialogOptions [i].isEnd) {
							optionIndices [i] = tree.treeNodes.Count;
						} else {
							optionIndices [i] = tree.treeNodes.FindIndex (
								delegate(DialogNode obj) {
									return obj.GID == tree.treeNodes[editingIndex].dialogOptions[i].nextNode;	
								});
							if (optionIndices [i] == -1) {
								optionIndices [i] = 0;
							}
						}
					} else {
						optionIndices[i] = 0;
					}
						
					string[] options = getPopupOptions (true);
					optionIndices[i] = EditorGUILayout.Popup ("Next Node", optionIndices[i], options);

					// Special case for END node
					if (optionIndices [i] == tree.treeNodes.Count) {
						tree.treeNodes [editingIndex].dialogOptions [i].isEnd = true;
					} else {
						tree.treeNodes [editingIndex].dialogOptions [i].isEnd = false;
						tree.treeNodes [editingIndex].dialogOptions [i].nextNode = tree.treeNodes [optionIndices [i]].GID;
					}

					EditorGUILayout.EndHorizontal ();

					EditorGUILayout.BeginHorizontal ();
					GUILayout.FlexibleSpace ();

					if (GUILayout.Button ("Delete Dialog Option", EditorStyles.miniButtonRight, GUILayout.MaxWidth (120))) {
						optionToDelete = i; 
					}

					EditorGUILayout.EndHorizontal ();

					if (optionToDelete != -1) {
						tree.treeNodes [editingIndex].dialogOptions.RemoveAt (optionToDelete);
						optionCollapse.RemoveAt (optionToDelete);
					}

				}

			}

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add New Dialog Option")) {
				DialogOption newOption = new DialogOption ();
				tree.treeNodes [editingIndex].dialogOptions.Add (newOption);
				optionCollapse.Add (true);
			}
			EditorGUILayout.EndHorizontal ();
		
		}
	}

	private string[] getPopupOptions(bool includeEndOption) {

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
			if (includeEndOption) {
				optionList.Add ("END");
			}
			options = optionList.ToArray ();
		}


		return options;

	}

}
