using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineGenerator : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        LineRenderer outline = gameObject.AddComponent<LineRenderer>();
        outline.SetVertexCount(sprite.vertices.Length);
        outline.loop = true;
        bool[] visited = new bool[sprite.vertices.Length];
        int startVertex = 0;
        outline.SetPosition(startVertex, sprite.vertices[startVertex] + new Vector2(transform.position.x, transform.position.y));
        visited[startVertex] = true;
        for (int count = 1; count < sprite.vertices.Length; count++) {
            int i1 = 0;
            float minDist = 999f;
            for (int j = 0; j < sprite.vertices.Length; j++) {
                
                if (startVertex == j || visited[j]) continue;
                if (Vector2.Distance(sprite.vertices[startVertex], sprite.vertices[j]) < minDist) {
                    minDist = Vector2.Distance(sprite.vertices[startVertex], sprite.vertices[j]);
                    i1 = j;
                }
            }
            visited[i1] = true;
            outline.SetPosition(count, sprite.vertices[i1]*transform.localScale.x + new Vector2(transform.position.x,transform.position.y)); ;
            startVertex = i1;
        }
        outline.material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
