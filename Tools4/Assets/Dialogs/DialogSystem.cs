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

		if (!string.IsNullOrEmpty (rootNode.textKey)) {
			nodeText.text = LocalizationManager.Get (rootNode.textKey);

			if (currentNode.dialogOptions.Count >= 1) {
				opText1.text = LocalizationManager.Get(rootNode.dialogOptions[0].textKey);
			}
			if (currentNode.dialogOptions.Count >= 2) {
				opText1.text = LocalizationManager.Get(rootNode.dialogOptions[1].textKey);
			}
			if (currentNode.dialogOptions.Count >= 3) {
				opText1.text = LocalizationManager.Get(rootNode.dialogOptions[2].textKey);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
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
