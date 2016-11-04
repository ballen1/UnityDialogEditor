using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This is for demonstrating the dialogue tree editor only

public class DialogSystem : MonoBehaviour {

	public Text nodeText;
	public Text opText1;
	public Text opText2;
	public Text opText3;

	public DialogTree tree;
	private DialogNode currentNode;

	private bool dialogEnded = false;

	// Use this for initialization
	void Start () {

		DialogNode rootNode = getNodeFromGID (tree.root);
		currentNode = rootNode;

		if (!string.IsNullOrEmpty (currentNode.textKey)) {
			nodeText.text = LocalizationManager.Get (currentNode.textKey);

			if (currentNode.dialogOptions.Count >= 1) {
				opText1.text = "1. " + LocalizationManager.Get(currentNode.dialogOptions[0].textKey);
			}
			if (currentNode.dialogOptions.Count >= 2) {
				opText2.text = "2. " + LocalizationManager.Get(currentNode.dialogOptions[1].textKey);
			}
			if (currentNode.dialogOptions.Count >= 3) {
				opText3.text = "3. " + LocalizationManager.Get(currentNode.dialogOptions[2].textKey);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (!dialogEnded) {
			if (!string.IsNullOrEmpty (currentNode.textKey)) {
				nodeText.text = LocalizationManager.Get (currentNode.textKey);

				if (currentNode.dialogOptions.Count >= 1) {
					opText1.text = "1. " + LocalizationManager.Get (currentNode.dialogOptions [0].textKey);
				}
				if (currentNode.dialogOptions.Count >= 2) {
					opText2.text = "2. " + LocalizationManager.Get (currentNode.dialogOptions [1].textKey);
				} else {
					opText2.text = "";
				}
				if (currentNode.dialogOptions.Count >= 3) {
					opText3.text = "3. " + LocalizationManager.Get (currentNode.dialogOptions [2].textKey);
				} else {
					opText3.text = "";
				}
			}

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (currentNode.dialogOptions [0].isEnd) {
					theEnd ();
				}
				currentNode = getNodeFromGID (currentNode.dialogOptions [0].nextNode);
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (currentNode.dialogOptions [1].isEnd) {
					theEnd ();
				}
				currentNode = getNodeFromGID (currentNode.dialogOptions [1].nextNode);

			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				if (currentNode.dialogOptions [2].isEnd) {
					theEnd ();
				}
				currentNode = getNodeFromGID (currentNode.dialogOptions [2].nextNode);
			}
		}

	}

	private void theEnd() {
		nodeText.text = "THE END";
		opText1.text = "";
		opText2.text = "";
		opText3.text = "";
		dialogEnded = true;
	}

	private DialogNode getNodeFromGID(int GID) {

		for (int i = 0; i < tree.treeNodes.Count; i++) {
			if (tree.treeNodes [i].GID == GID) {
				return tree.treeNodes [i];
			}
		}

		return null;

	}

}
