using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontTest : MonoBehaviour {

    public UIFont m_font;
    public Material material;
    public string text = "";
    private Matrix4x4 uvMatrix;
    Vector3[] myuvs;
    // Use this for initialization
    void Start () {
        //material = m_font.material;

        //NGUIText.Print("1", vertexs, uvs, colors);

        uvMatrix = Matrix4x4.identity;
        uvMatrix.m11 = -1;
        uvMatrix.m13 = 1;
        for (int i = 0; i < text.Length; i++)
        {
            int chr = text[i];
            myuvs = getUVbyChar(chr);
        }
        
    }
    private Vector3[] getUVbyChar(int index)
    {
        int texWidth = m_font.bmFont.texWidth;
        int texHeight = m_font.bmFont.texHeight;
        BMGlyph bgph = m_font.bmFont.GetGlyph(index, false);
        float x = (float)bgph.x / (float)texWidth;
        float y = (float)bgph.y / (float)texHeight;
        float width = (float)bgph.width / (float)texWidth;
        float height = (float)bgph.height / (float)texHeight;
        Vector3[] uvs = new Vector3[4];
        uvs[0] = uvMatrix * new Vector3(x, height + y, 0);
        uvs[1] = uvMatrix * new Vector3(x, y, 0);
        uvs[2] = uvMatrix * new Vector3(x + width, y, 0);
        uvs[3] = uvMatrix * new Vector3(x + width, height + y, 0);
        return uvs;
    }
	// Update is called once per frame
	void Update () {
		
	}
    private void OnPostRender()
    {
        GL.PushMatrix();
        material.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);

        GL.Color(Color.red);

        GL.TexCoord(new Vector3(0,0,0));
        GL.Vertex3(0, 0, 0);
        GL.TexCoord(new Vector3(0, 1, 0));
        GL.Vertex3(0, 1, 0);
        GL.TexCoord(new Vector3(1, 1, 0));
        GL.Vertex3(0.5f, 1, 0);
        GL.TexCoord(new Vector3(1, 0, 0));
        GL.Vertex3(0.5f, 0, 0);



        //GL.TexCoord(new Vector3(0.5f, 0.6875f, 0));
        //GL.Vertex3(0.5f,0,0);
        //GL.TexCoord(new Vector3(0.5f, 1, 0));
        //GL.Vertex3(0.5f,1,0);
        //GL.TexCoord(new Vector3(0.71875f, 1, 0));
        //GL.Vertex3(1,1, 0);
        //GL.TexCoord(new Vector3(0.71875f, 0.6875f, 0));
        //GL.Vertex3(1, 0, 0);

        Vector3[] vertexs = { new Vector3(0.5f, 0, 0), new Vector3(0.5f, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
        //Vector3[] vertexs = { new Vector3(0.5f, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(0.5f, 0, 0) };
        //0
        //Vector3[] uvs = { new Vector3(0.5f, 0.6875f, 0), new Vector3(0.5f, 1, 0), new Vector3(0.71875f, 1, 0), new Vector3(0.71875f, 0.6875f, 0) };
        //2
        //Vector3[] uvs = { new Vector3(0.734375f, 0.6875f, 0), new Vector3(0.734375f, 1, 0), new Vector3(0.953125f, 1, 0), new Vector3(0.953125f, 0.6875f, 0) };
        Vector3[] uvs = { new Vector3(0.734375f, 0.6875f, 0), new Vector3(0.734375f, 0, 0), new Vector3(0.953125f, 0, 0), new Vector3(0.953125f, 0.6875f, 0) };
        //1
        //Vector3[] uvs = { new Vector3(0.21875f, 0.34375f, 0), new Vector3(0.21875f, 0, 0), new Vector3(0.515625f, 0, 0), new Vector3(0.515625f, 0.34375f, 0) };
        //Vector3[] uvs = { new Vector3(0.21875f, 0, 0), new Vector3(0.21875f, 0.34375f, 0), new Vector3(0.515625f, 0.34375f, 0), new Vector3(0.515625f, 0, 0) };
        //Vector3[] uvs = { new Vector3(0.21875f, 0.34375f, 0), new Vector3(0.21875f, 0.65625f, 0), new Vector3(0.515625f, 0.96875f, 0), new Vector3(0.515625f, 0.34375f, 0) };
        DrawWords(vertexs, myuvs);

        GL.End();
        GL.PopMatrix();
    }



    private void DrawWords(Vector3[] vertexs,Vector3[] uvs)
    {
        Debug.AssertFormat(vertexs.Length == 4 || uvs.Length == 4,"faile {0},{1}", vertexs.Length, uvs.Length);
        for (int i = 0; i < 4; i++)
        {
            GL.TexCoord(uvs[i]);
            GL.Vertex(vertexs[i]);
        }
    }

}
