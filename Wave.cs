using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public int height;
    public int length;
    public float amps;
	public float frequency;
	public float speed;

	public float perlinSpeed;
	public float perlinStrength;
	public float perlinSpacing;

	public bool noiseWave;
	//float noiseWalk = 1f;
	//float noiseStrength = 2f;

    List<Vector3> vertList = new List<Vector3>();

	int counter;

    Vector3[] newVertices;
    int[] newTriangles;
	Mesh mesh;
	MeshCollider col;

    void MeshSetup()
    {
		counter = 0;

        newTriangles = new int[(int) (height*length*6)];
        newVertices = new Vector3[ (int) (length * height * 4)];
        
        for (int l = 0; l < length; l++) {
            for (int h = 0; h < height; h++) {
				newTriangles[((l * height) + h) * 6] = (((l * height) + h) * 4);//0
				newTriangles[((l * height) + h) * 6+1] = (((l * height) + h) * 4)+1;//1
				newTriangles[((l * height) + h) * 6+2] = (((l * height) + h) * 4)+2;//2
				newTriangles[((l * height) + h) * 6+3] = (((l * height) + h) * 4)+3;//3
				newTriangles[((l * height) + h) * 6+4] = (((l * height) + h) * 4)+1;//1
				newTriangles[((l * height) + h) * 6+5] = (((l * height) + h) * 4);//0
                
            }
            for (int h = 0; h < height; h++) {
				newVertices[((l * height) + h) * 4] = new Vector3(l, -1, h);
				newVertices[((l * height) + h) * 4+1] = new Vector3(l+1, -1, h+1);
				newVertices[((l * height) + h) * 4+2] = new Vector3(l+1, -1, h);
				newVertices[((l * height) + h) * 4+3] = new Vector3(l, -1, h+1);
            }
            
        }
		col.sharedMesh = mesh;
    }

	void waveSetup(){
		for(int i = 0; i < newVertices.Length; i++){
			if(newVertices[i].z % 3 == 0){
				newVertices [i].y = 1;
			}
		}
	}
    void Start()
    {

		col = GetComponent<MeshCollider>();
		mesh = GetComponent<MeshFilter>().mesh;
		MeshSetup();
		
		
		waveSetup ();
        
        
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;

        //mesh.uv = newUV;

    }
    
    void Update()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for(int i = 0; i < vertices.Length; i++)
        {
			
			//vertices[i] += new Vector3(vertices[i].x, (Mathf.Sin((vertices[i].z + vertices[i].x + speed * Time.deltaTime) * frequency) * amps), vertices[i].z);
			if (noiseWave) {
				vertices[i].y = Mathf.PerlinNoise(vertices[i].x * perlinSpacing + perlinSpeed * Time.time, vertices[i].z * perlinSpacing + perlinSpeed * Time.time) * perlinStrength;
			}
			else
			{
				vertices[i] = new Vector3(vertices[i].x, (Mathf.Sin((vertices[i].z + vertices[i].x + speed * Time.time) * frequency) * amps) + Mathf.PerlinNoise(vertices[i].x * perlinSpacing + perlinSpeed*Time.time, vertices[i].z * perlinSpacing + perlinSpeed * Time.time) * perlinStrength, vertices[i].z);
			}
		}
        mesh.RecalculateNormals();
        mesh.vertices = vertices;

		//counter += 10;
    }

}
