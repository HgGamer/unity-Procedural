using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RestartScene : MonoBehaviour {

	public GameObject label;
	public void PRestartScene(){
		//Application.LoadLevel(Application.loadedLevel);
		global.rand = 0;
		global.vertexcount = 0;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	private void Update() {
//		label.GetComponent<Text>().text = "Vertex count: "+ global.vertexcount.ToString();
		//global.vertexcount;
	}

}
