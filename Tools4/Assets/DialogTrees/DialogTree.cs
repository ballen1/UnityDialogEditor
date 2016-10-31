using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogTree : ScriptableObject {

	[SerializeField]
	private int nextGID = 0;

	public List<DialogNode> treeNodes;
	public int root;

	public void addNewDialogNode() {

		DialogNode newNode = new DialogNode ();
		newNode.dialogOptions = new List<DialogOption> ();
		newNode.GID = nextGID++;
		treeNodes.Add (newNode);
	}
}
