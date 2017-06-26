using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo
{
    public string text;
    public Vector3[,] vertes;
    public Vector3[,] uvs;
}

public class GLBox : MonoBehaviour
{
    public Color lineColor = Color.red;
    public Shader fadeShader = null;
    private Material fadeMaterialred = null;
    private Material fontMaterialred = null;
    public UIFont m_font;
    [Range(0, 100)]
    public float space = 40f;
    public int boxCount = 50;
    public int linCount = 7;
    public float boxsize;
    private readonly static string SPACEKEY = "ids.ipd.space";
    private readonly static string BOXCOUNTKEY = "ids.ipd.boxcount";
    private readonly static string LINCOUTKEY = "ids.ipd.lincount";
    private ArrayList vertes = new ArrayList();
    private Dictionary<int, ArrayList> framevertes = new Dictionary<int, ArrayList>();
    private Matrix4x4 uvMatrix;
    private Dictionary<int, Vector3[]> numbersuvs = new Dictionary<int, Vector3[]>();
    void Awake()
    {
        // create the fade material
        fadeMaterialred = (fadeShader != null) ? new Material(fadeShader) : new Material(Shader.Find("Unlit/Color"));
        fadeMaterialred.color = lineColor;

        //fontMaterialred = (fadeShader != null) ? new Material(fadeShader) : new Material(Shader.Find("Unlit/Color"));
        fontMaterialred = m_font.material;

        uvMatrix = Matrix4x4.identity;
        uvMatrix.m11 = -1;
        uvMatrix.m13 = 1;

        string text = "0123456789";
        for (int i = 0; i < text.Length; i++)
        {
            int chr = text[i];
            numbersuvs.Add(chr, getUVbyChar(chr));
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if !(UNITY_EDITOR)
        space = float.Parse(SystemProperties.get(SPACEKEY, 40.ToString()));
        boxCount = int.Parse(SystemProperties.get(BOXCOUNTKEY, 50.ToString()));
        linCount = int.Parse(SystemProperties.get(LINCOUTKEY, 7.ToString()));
#endif
    }

    void OnPostRender()
    {
        GL.PushMatrix();
        GL.LoadOrtho();
        fadeMaterialred.SetPass(0);
        GLDrawBox();
        GL.End();
        GL.PopMatrix();


        GL.PushMatrix();
        GL.LoadOrtho();
        
        fontMaterialred.SetPass(0);

        GLDrawWord();

        GL.End();
        GL.PopMatrix();
    }


    void GLDrawLine(Vector3 beg, Vector3 end)
    {
        GL.Vertex3(beg.x, beg.y, beg.z);
        GL.Vertex3(end.x, end.y, end.z);
    }
    void GLDrawBox()
    {
        GL.Begin(GL.LINES);
        float spacesize = 1.0f / (2*boxCount-1) * (space / 100.0f);
        
        

        vertes.Clear();
        for (int j = 0; j < boxCount; j++)
        {
            boxsize = 1.0f / boxCount + spacesize / boxCount - spacesize;
            //int quareCount = (j + 1).ToString().Length;
            for (int i = 0; i < linCount + 1; i++)
            {
                
                GLDrawLine(new Vector3(j * (boxsize + spacesize), 0.5f - boxsize / 2 + boxsize / linCount * i, 0), new Vector3(j * (boxsize + spacesize) +boxsize, 0.5f - boxsize / 2 + boxsize / linCount * i, 0));
                GLDrawLine(new Vector3(j * (boxsize + spacesize)+ boxsize / linCount * i, 0.5f + boxsize / 2, 0), new Vector3(j * (boxsize + spacesize) + boxsize / linCount * i, 0.5f - boxsize / 2, 0));

               
            }
            float x = j * (boxsize + spacesize);
            float y = (0.5f - boxsize / 2) + boxsize;
            CharInfo charinfo = getVertesByPosition((j + 1).ToString(), x, y, boxsize, boxsize);

            //boxsize = 1.0f / (boxCount + 2) + spacesize / (boxCount + 2) - spacesize;

            //x = j * (boxsize + spacesize);
            //y = (0.5f - boxsize / 2) + boxsize;
            CharInfo charinfoUp = getVertesByPosition((j + 1).ToString(), x, 1 - boxsize, boxsize, boxsize);
            CharInfo charinfoDown = getVertesByPosition((j + 1).ToString(), x, 0, boxsize, boxsize);

            if (j != 0 && j != boxCount - 1)
            {
                float y1 = j * (boxsize + spacesize);
                CharInfo charinfoleft = getVertesByPosition((j + 1).ToString(), 0, y1, boxsize, boxsize);
                CharInfo charinforight = getVertesByPosition((j + 1).ToString(), 1 - boxsize, y1, boxsize, boxsize);

                vertes.Add(charinfoleft);
                vertes.Add(charinforight);

            }

            vertes.Add(charinfoUp);
            vertes.Add(charinfoDown);

            vertes.Add(charinfo);
            
            
        }

    }

    /// <summary>
    /// 输入文字计算出相应的顶点和UV
    /// </summary>
    /// <param name="text"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private CharInfo getVertesByPosition(string text,float x,float y,float width,float height)
    {
        CharInfo info = new CharInfo();
        info.text = text;
        int quareCount = text.Length;
        Vector3[,] avertes = new Vector3[quareCount, 4];
        Vector3[,] uvs = new Vector3[quareCount, 4];
        info.vertes = avertes;
        info.uvs = uvs;
        for (int i = 0; i < quareCount; i++)
        {
            avertes[i, 0] = new Vector3(x + (width / quareCount) * i, y, 0);
            avertes[i, 1] = new Vector3(x + (width / quareCount) * i, y + height, 0);
            avertes[i, 2] = new Vector3(x + (width / quareCount) * i + width / quareCount, y + height);
            avertes[i, 3] = new Vector3(x + (width / quareCount) * i + width / quareCount, y, 0);
            int chr = text[i];
            Vector3[] auv = numbersuvs[chr];
            uvs[i,0] = auv[0];
            uvs[i,1] = auv[1];
            uvs[i,2] = auv[2];
            uvs[i,3] = auv[3];
        }
        

        return info;
    }

    private Vector3[][] PrintUVs(string text)
    {
        List<Vector3[]> templist = new List<Vector3[]>();
        for (int i = 0; i < text.Length; i++)
        {
            int chr = text[i];
            templist.Add(numbersuvs[chr]);
        }
        return templist.ToArray();
    }
    private void GLDrawWord()
    {
        GL.Begin(GL.QUADS);
        GL.Color(Color.green);
        for (int i = 0; i < vertes.Count; i++)
        {
            CharInfo charInfo = (CharInfo)vertes[i];
            Vector3[,] avertes = charInfo.vertes;
            Vector3[,] wordUVs = charInfo.uvs;
            int chrlength = avertes.Length / 4;
            for (int j = 0; j < chrlength; j++)
            {
                Vector3[] finalvertes = new Vector3[4];
                finalvertes[0] = avertes[j,0];
                finalvertes[1] = avertes[j,1];
                finalvertes[2] = avertes[j,2];
                finalvertes[3] = avertes[j,3];

                Vector3[] finaluvs = new Vector3[4];
                finaluvs[0] = wordUVs[j,0];
                finaluvs[1] = wordUVs[j,1];
                finaluvs[2] = wordUVs[j,2];
                finaluvs[3] = wordUVs[j,3];
                DrawAWord(finalvertes, finaluvs);
            }

        }


    }
    private void DrawAWord(Vector3[] vertexs, Vector3[] uvs)
    {
        Debug.AssertFormat(vertexs.Length == 4 || uvs.Length == 4, "faile {0},{1}", vertexs.Length, uvs.Length);
        for (int i = 0; i < 4; i++)
        {
            GL.TexCoord(uvs[i]);
            GL.Vertex(vertexs[i]);
        }
    }
}
