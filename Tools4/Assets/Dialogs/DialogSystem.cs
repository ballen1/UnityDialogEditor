using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogSystem : MonoBehaviour {

	public Text nodeText;
	public Text opText1;
	public Text opText2;
	public Text opText3;

	public DialogTree tree;
	private DialogNode currentNode;

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

		if (Input.GetKey(KeyCode.Alpha1)) {
			
		}
		if (Input.GetKey (KeyCode.Alpha2)) {

		}
		if (Input.GetKey (KeyCode.Alpha3)) {

		}

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
