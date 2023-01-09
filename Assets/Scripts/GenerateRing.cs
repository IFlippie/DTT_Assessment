using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerateRing : MonoBehaviour
{
    Mesh me;
    MeshFilter mf;
    Vector3[] ringVertices;
    int[] triangles;

    [Header("Ring Variables")]
    public int verticesPerPoint;
    public int layers;
    //Distance between the vertices in each layer
    public float ringRadius;
    public float curveRadius;
    public float yRotate;
    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        me = new Mesh()
        {
            name = "Ring"
        };

        mf.mesh = me;
        me.Clear();       
    }

    // Update is called once per frame
    void Update()
    {
        SmoothSpawnPoints();
        RotateRing();
    }

    public void SmoothSpawnPoints() 
    {
        //applying -1 here is a minor adjustment to not use an extra ring of vertices, the alternative solution would be to stitch the first and last vertice rings together
        float uStep = (2f * Mathf.PI) / (layers - 1);
        float vStep = (2f * Mathf.PI) / verticesPerPoint;
        ringVertices = new Vector3[verticesPerPoint * layers];
        
        for (int k = 0, j = 0; j < layers; j++)
        {
            for (int o = 0; o < verticesPerPoint; o++, k++)
            {
                Vector3 p;
                float r = curveRadius + ringRadius * Mathf.Cos(o * vStep);
                p.x = (r * Mathf.Sin(j * uStep));
                p.y = (r * Mathf.Cos(j * uStep));
                p.z = (ringRadius * Mathf.Sin(o * vStep));
                var vPos = p;
                ringVertices[k] = vPos;
            }
        }
        me.vertices = ringVertices;

        triangles = new int[verticesPerPoint * layers * 6];
        for (int ti = 0, vi = 0, z = 0; z < layers - 1; z++, vi++)
        {
            for (int x = 0; x < verticesPerPoint; x++, ti += 6)
            {
                if (x < verticesPerPoint - 1)
                {
                    //so 2/3 and 1/4 switch to properly show the triangles
                    triangles[ti] = vi;
                    triangles[ti + 1] = triangles[ti + 4] = vi + 1;
                    triangles[ti + 2] = triangles[ti + 3] = vi + verticesPerPoint;
                    triangles[ti + 5] = vi + verticesPerPoint + 1;
                    vi++;
                    me.triangles = triangles;
                }
                else
                {
                    triangles[ti] = vi;
                    triangles[ti + 1] = vi - verticesPerPoint + 1;
                    triangles[ti + 2] = vi + verticesPerPoint;
                    triangles[ti + 3] = vi + verticesPerPoint;
                    triangles[ti + 4] = vi - verticesPerPoint + 1;
                    triangles[ti + 5] = vi + 1;
                    me.triangles = triangles;
                }
            }
        }
        me.triangles = triangles;
        me.RecalculateNormals();
    }

    private void RotateRing() 
    {
            Quaternion q = Quaternion.Euler(0f, yRotate, 0f);
            transform.rotation *= q;
    }
}
