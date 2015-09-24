using UnityEngine;
using System.Collections;

public class Mascara : MonoBehaviour {

    private int sizeTexture = 200;
    private int radio = 1;
    private int sizePlane = 10;
    private float proporcion;

    // Use this for initialization
    void Start () {
        //Todo este codigo tendra que ir en el metodo update y calcularse a cada frame.
        // Create a new 200x200 texture ARGB32 (32 bit with alpha) and no mipmaps
        sizeTexture = 200;
        radio = 1;
        sizePlane = 10;

    }
	
	// Update is called once per frame
	void Update () {

        proporcion = (float)sizeTexture / sizePlane;

        var texture = new Texture2D(sizeTexture, sizeTexture, TextureFormat.ARGB32, false);


        Vector3 posicion = GameObject.Find("Personaje").transform.position;

        float x = posicion[0];
        float z = posicion[2];

        print("Pues resulta que si que imprime");

        print(posicion);
        float zmax = (-20 * (z - 5)+20);
        float zmin = (-20 * (z - 5)-20);
        print("zmin: " + zmin + "/zmax: " + zmax);

        float xmax = (-20 * (x - 5) + 20);
        float xmin = (-20 * (x - 5) - 20);


        int i = 0;
        int j = 0;
        for (i = 0; i < sizeTexture; i++)
        {
            for (j = 0; j < sizeTexture; j++)
            {
                //el 5 es porque el plano esta centrado en el (0,0) por lo tanto va del -5 al 5
                //if (j >(-20 * (z - 5)-20) && j < (-20 * (z - 5)+20) && i > (-20 * (x - 5)+20) && i < (-20 * (x - 5)-20))
                if(i>xmin && i<xmax && j<zmax && j>zmin)
                {
                    texture.SetPixel(i, j, new Color(1.0f, 1.0f, 1.0f, 0.0f));
                }
                    else
                {
                    texture.SetPixel(i, j, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
            }
        }




        // Apply all SetPixel calls
        texture.Apply();

        // connect texture to material of GameObject this script is attached to
        GetComponent<Renderer>().material.mainTexture = texture;

    }
}
