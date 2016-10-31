using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogNode {

	public int GID;
	public string name = "Node";
	public string textKey;
	public List<DialogOption> dialogOptions;

}
