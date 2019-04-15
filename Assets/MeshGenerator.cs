using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class MeshGenerator : MonoBehaviour {
	
	const int chunksize = 64;
	public float random;
	Mesh mesh;
	public ArrayList _vertices = new ArrayList();

	public ArrayList _triangles = new ArrayList();
	float t_x;
	float t_z;
	float seed = 200;
	float offsetter = 11112312;
	Thread thread;
	MeshFilter meshFilter;
	bool finished = false;
	bool run = false;
	public void ReGenerate(){
		t_x = transform.position.x;
		t_z = transform.position.z;
		offsetter= (int)offsetter*global.rand;
		mesh.Clear();
		meshFilter.mesh = mesh;
		
		thread = new Thread(CreateShape);
		finished = false;
		run = true;
		thread.Start();
	}
	// Use this for initialization
	void Awake () {
		mesh = new Mesh();
		_vertices = new ArrayList();
		_triangles = new ArrayList();
		meshFilter = GetComponent<MeshFilter>();
		transform.parent = GameObject.Find("Map").transform;
	}
	void Update () {
		
		if(run && !thread.IsAlive && !finished){
			mesh.Clear();
			mesh.vertices = _vertices.ToArray(typeof(Vector3)) as Vector3[];
			mesh.triangles = _triangles.ToArray(typeof(int)) as int[];
			mesh.RecalculateNormals();
			_vertices.Clear();
			_triangles.Clear();
			GetComponent<MeshCollider>().sharedMesh = mesh;
			finished = true;
		}
	}
	int  GetHight(int x, int y){
		float sampleX = (x+t_x*10+offsetter)/seed;
		float sampleY = (y+t_z*10+offsetter)/seed;
		return (int) (Mathf.PerlinNoise(sampleX, sampleY)* 64);
	}

	void CreateQuadFB(int x1, int y1, int x2, int y2){ // hightmap coordinates
		int h1 = GetHight(x1,y1);
		int h2 = GetHight(x2,y2);
		if(h1==h2){
			return;
		}
		if(h1>h2){
			for(int h=0;h<h1-h2;h++){
				int firstpoint  = _vertices.Count;
				_vertices.Add(new Vector3(x1+1,h1-h-1,y1));
				_vertices.Add(new Vector3(x1+1,h1-h,y1));
				_vertices.Add(new Vector3(x1+1,h1-h-1,y1+1));
				_vertices.Add(new Vector3(x1+1,h1-h,y1+1));
				_triangles.Add(firstpoint);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+3);
			}
		}else{
			for(int h=0;h<h2-h1;h++){
				int firstpoint  = _vertices.Count;
				_vertices.Add(new Vector3(x1+1,h2-h-1,y1+1));
				_vertices.Add(new Vector3(x1+1,h2-h,y1+1));
				_vertices.Add(new Vector3(x1+1,h2-h-1,y1));
				_vertices.Add(new Vector3(x1+1,h2-h,y1));	
				_triangles.Add(firstpoint);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+3);
			} 

		}
	}
	void CreateQuadLR(int x1, int y1, int x2, int y2){ // hightmap coordinates
		int h1 = GetHight(x1,y1);
		int h2 = GetHight(x2,y2);
		if(h1==h2){
			return;
		}
		if(h1>h2){
			for(int h=0;h<h1-h2;h++){
				int firstpoint  = _vertices.Count;
				_vertices.Add(new Vector3(x1,h1-h,y1+1));
				_vertices.Add(new Vector3(x1,h1-h-1,y1+1));
				_vertices.Add(new Vector3(x1+1,h1-h,y1+1));
				_vertices.Add(new Vector3(x1+1,h1-h-1,y1+1));
				_triangles.Add(firstpoint);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+3);
			}
		}else{
			for(int h=0;h<h2-h1;h++){
				int firstpoint  = _vertices.Count;
				_vertices.Add(new Vector3(x1,h2-h-1,y1+1));
				_vertices.Add(new Vector3(x1,h2-h,y1+1));
				_vertices.Add(new Vector3(x1+1,h2-h-1,y1+1));
				_vertices.Add(new Vector3(x1+1,h2-h,y1+1));
				_triangles.Add(firstpoint);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+2);
				_triangles.Add(firstpoint+1);
				_triangles.Add(firstpoint+3);
			}
		}
	}
	void CreateQuadTop(int x, int z){
		int hight = GetHight(x,z);
		int firstpoint  = _vertices.Count;
		_vertices.Add(new Vector3(x,hight,z));
		_vertices.Add(new Vector3(x,hight,z+1));
		_vertices.Add(new Vector3(x+1,hight,z));
		_vertices.Add(new Vector3(x+1,hight,z+1));
		_triangles.Add(firstpoint);
		_triangles.Add(firstpoint+1);
		_triangles.Add(firstpoint+2);
		_triangles.Add(firstpoint+2);
		_triangles.Add(firstpoint+1);
		_triangles.Add(firstpoint+3);
	}
	void CreateShape(){
		
		for(int j=0;j<=chunksize*chunksize;j++){
			int x = j/chunksize;
			int y = j%chunksize;
			CreateQuadTop(x,y);
			CreateQuadLR(x,y,x,y+1);
			CreateQuadFB(x,y,x+1,y);
		}
		
	
	}
}
