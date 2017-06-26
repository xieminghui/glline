using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joint
{
    public Vector3 org;
    public Vector3 end;
}
public class GLTest : MonoBehaviour {
    public Color lineColor = Color.red;
    public Shader fadeShader = null;
    private Material fadeMaterialred = null;
    private Material fadeMaterialgreen = null;
    private Vector3 directionx, directiony;
    private float redspace = 0.001f;
    private float greenspace = 0.1f;
    private float linewidth = 0.007f;
    void Awake()
    {
        // create the fade material
        fadeMaterialred = (fadeShader != null) ? new Material(fadeShader) : new Material(Shader.Find("Unlit/Color"));
        fadeMaterialred.color = Color.red;
        fadeMaterialgreen = (fadeShader != null) ? new Material(fadeShader) : new Material(Shader.Find("Unlit/Color"));
        fadeMaterialgreen.color = Color.green;
        directionx = (new Vector3(0.5f, 0.5f, 0) - Vector3.zero).normalized;
        directiony = (new Vector3(0.5f, 0.5f, 0) - new Vector3(1, 0, 0)).normalized;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPostRender()
    {

        fadeMaterialred.SetPass(0);
        GLDrawLineBox(Color.red, redspace);

        fadeMaterialgreen.SetPass(0);
        GLDrawLineBox(Color.green, greenspace);

    }

    void GLDrawLineBox(Color color,float space)
    {
        
        GL.PushMatrix();
        GL.LoadOrtho();

        //GL.Begin(GL.LINES);

        //GLDrawLine(new Vector3(0, 0, 0) + directionx * space, new Vector3(0, 1, 0) - directiony * space);
        //GLDrawLine(new Vector3(0, 0, 0) + directionx * space, new Vector3(1, 0, 0) + directiony * space);
        //GLDrawLine(new Vector3(0, 1, 0) - directiony * space, new Vector3(1, 1, 0) - directionx * space);
        //GLDrawLine(new Vector3(1, 0, 0) + directiony * space, new Vector3(1, 1, 0) - directionx * space);


        GL.Begin(GL.QUADS);

        //GLDrawLine(new Vector3(0, 0, 0) + directionx * space , new Vector3(0, 1, 0) - directiony * space);
        //GLDrawLine(new Vector3(0, 0, 0) + directionx * space , new Vector3(0, 0, 0) + directionx * space + Vector3.right* linewidth);
        //GLDrawLine(new Vector3(0, 0, 0) + directionx * space + Vector3.right * linewidth, new Vector3(0, 1, 0) - directiony * space + Vector3.right* linewidth);
        //GLDrawLine(new Vector3(0, 1, 0) - directiony * space + Vector3.right * linewidth, new Vector3(0, 1, 0) - directiony * space);
        //GLDrawLine(new Vector3(0, 0, 0) + directionx * space, new Vector3(1, 0, 0) + directiony * space);
        //GLDrawLine(new Vector3(0, 1, 0) - directiony * space, new Vector3(1, 1, 0) - directionx * space);
        //GLDrawLine(new Vector3(1, 0, 0) + directiony * space, new Vector3(1, 1, 0) - directionx * space);

        //GL.Vertex(new Vector3(0, 0, 0) + directionx * space);
        //GL.Vertex(new Vector3(0, 1, 0) - directiony * space);
        //GL.Vertex(new Vector3(0, 1, 0) - directiony * space + Vector3.right * linewidth);
        //GL.Vertex(new Vector3(0, 0, 0) + directiony * space + Vector3.right * linewidth);

        //left
        GL.Vertex3(space, space, 0);
        GL.Vertex3(space, 1- space, 0);
        GL.Vertex3(linewidth + space, 1 - space, 0);
        GL.Vertex3(linewidth + space,  space, 0);

        //right
        GL.Vertex3(1 - (linewidth + space),  space, 0);
        GL.Vertex3(1 - (linewidth + space), 1 -  space, 0);
        GL.Vertex3(1 - space, 1 - space, 0);
        GL.Vertex3(1- space,  space, 0);

        //botterm
        GL.Vertex3(space, space, 0);
        GL.Vertex3(space, linewidth + space, 0);
        GL.Vertex3(1 -  space, linewidth + space, 0);
        GL.Vertex3(1 -   space,  space, 0);

        //top
        GL.Vertex3(space, 1 - (linewidth + space), 0);
        GL.Vertex3(space, 1 - space, 0);
        GL.Vertex3(1-space, 1 - space, 0);
        GL.Vertex3(1-space, 1 - (linewidth + space), 0);

        GL.End();
        GL.PopMatrix();
    }

    void GLDrawLine(Vector3 beg, Vector3 end)
    {
        GL.Vertex3(beg.x, beg.y, beg.z);
        GL.Vertex3(end.x, end.y, end.z);
    }
}
