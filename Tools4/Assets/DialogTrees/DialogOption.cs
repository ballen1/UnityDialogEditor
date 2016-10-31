using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogOption {

	public string textKey;
	public int nextNode;
	public bool isEnd = false;

}