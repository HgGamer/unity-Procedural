using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {


		public int x;
		public int y;
		public GameObject instance;
		public Chunk(int x, int y , ref GameObject instance){
			this.x = x;
			this.y = y;
			this.instance = instance;
		}
		public int getX(){
			return x;
		}
	
		public int getY(){
			return y;
		}
}
