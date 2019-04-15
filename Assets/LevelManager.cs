using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public Text mytext;
	public GameObject player;
	ConcurrentQueue<Vector2Int> spawnqueue = new ConcurrentQueue<Vector2Int>();
	ConcurrentQueue<Vector2Int> deletequeue = new ConcurrentQueue<Vector2Int>();
	ConcurrentQueue<Transform> meshpool = new ConcurrentQueue<Transform>();
		
	public Dictionary<Vector2Int, int> _chunks = new Dictionary<Vector2Int, int>();
	private float deletedistance = 10.0f;
	public GameObject chunk;
	public Transform Map;
	private Vector2Int tchunkprops;
	private Transform tchunk;
	private Vector2Int tmpdelete;
	Thread deletethread;
	Thread spawnthread;

	public  Vector2 pt;
	void Start() {
		
		pt = new Vector2(player.transform.position.x/6.4f,player.transform.position.z/6.4f);
		spawnthread = new Thread(spawn);	
		spawnthread.Start();
		deletethread = new Thread(deleter);	
		deletethread.Start();

	}
	void Update() {
		mytext.text = Map.childCount.ToString();
		pt = new Vector2(player.transform.position.x/6.4f,player.transform.position.z/6.4f);
		while(deletequeue.TryDequeue(out tmpdelete)){
			lock(_chunks){
				if(_chunks.ContainsKey(tmpdelete)){
					Transform temp = Map.Find(_chunks[tmpdelete].ToString());
					temp.gameObject.SetActive(false);
					meshpool.Enqueue(temp);
					_chunks.Remove(tmpdelete);
				}
			}
		}
		while(spawnqueue.TryDequeue(out tchunkprops)){
			if(!_chunks.ContainsKey(new Vector2Int(tchunkprops.x,tchunkprops.y))){
				if(meshpool.TryDequeue(out tchunk)){
					tchunk.gameObject.SetActive(true);
				}else{
					tchunk = Instantiate(chunk).transform;
					tchunk.transform.name = tchunk.GetInstanceID().ToString();
				}
				tchunk.position = new Vector3(6.4f*tchunkprops.x,0,6.4f*tchunkprops.y);
				tchunk.GetComponent<MeshGenerator>().ReGenerate();
				lock(_chunks){
					_chunks.Add(new Vector2Int(tchunkprops.x,tchunkprops.y),tchunk.GetInstanceID());
				}
				return;
				
			}
		};	
	}
	
	void deleter(){
		
		//ArrayList deletables = new ArrayList();
		while(true){
			lock(_chunks){
				foreach(Vector2Int key in _chunks.Keys){
					float dist = Vector2.Distance(key, pt);
					if(dist>deletedistance){
						deletequeue.Enqueue(key);
					}
				}
			}
			/* Vector2Int[] _deletables = deletables.ToArray(typeof(Vector2Int)) as Vector2Int[];
			foreach (Vector2Int item in _deletables)
			{
				deletequeue.Enqueue(item);
			}*/	
			//deletables.Clear();
			Thread.Sleep(10);
			
			
			 
		}
	}
	public void spawn(){
		while(true){
			int x = (int) pt.x;
			int y = (int) pt.y;
			for(int i=-5;i<5;i++){
				for(int j=-5;j<5;j++){
					if(!_chunks.ContainsKey(new Vector2Int(x+i,y+j))){
						spawnqueue.Enqueue(new Vector2Int(x+i,y+j)); 
					}				
				}					
			}
			Thread.Sleep(10);
		}
	}
}
