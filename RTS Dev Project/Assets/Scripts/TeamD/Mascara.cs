using UnityEngine;
using System.Collections;

public class Mascara : MonoBehaviour {

    private int sizeTexture = 200;
    private int radio = 1;
    private float sizePlane = 100;
    private float proporcion;
    private Texture2D texture;

    // Use this for initialization
    void Start()
    {
        //Todo este codigo tendra que ir en el metodo update y calcularse a cada frame.
        // Create a new 200x200 texture ARGB32 (32 bit with alpha) and no mipmaps
        radio = 50;

        //inicializamos la textura todo negra.
        texture = new Texture2D(sizeTexture, sizeTexture, TextureFormat.ARGB32, false);
        for (int i = 0; i < sizeTexture; i++)
        {
            for (int j = 0; j < sizeTexture; j++)
            {
                texture.SetPixel(i, j, new Color(1.0f, 1.0f, 1.0f, 1.0f));
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
        
        //Ponemos todo el mapa visto en el ultimo frame como visitado sin visión
        int i, j,k;
        for (i = 0; i < sizeTexture; i++)
        {
            for (j = 0; j < sizeTexture; j++)
            {

                if (texture.GetPixel(i, j)[3] < 1)
                {
                    texture.SetPixel(i, j, new Color(1.0f, 1.0f, 1.0f, 0.5f));

                }
            }
        }


        //cogemos todos los objetos aliados en el mapa

        Object[] objetos = GameObject.FindGameObjectsWithTag("Ally");
        GameObject g;

        proporcion = -2;//(float)sizeTexture / sizePlane;
        Vector3 posicion;
        float x, z;
        for (k = 0; k < objetos.Length; k++)
        {
            g = (GameObject)objetos[k];
            posicion = g.transform.position;
            x = posicion[0];
            z = posicion[2];
            // a ver que tal
            x = proporcion* (x - sizePlane/2);
            z = proporcion * (z - sizePlane/2);


            for (i = 0; i < sizeTexture; i++)
            {
                for (j = 0; j < sizeTexture; j++)
                {
                    
                    if    (  (Mathf.Pow((i-x),2)+Mathf.Pow((j-z),2) ) < radio)
                    {
                        texture.SetPixel(i, j, new Color(1.0f, 1.0f, 1.0f, 0.0f));
                    }

                }
            }
        }

        //Pasamos a mirar que objetos de la CPU estan visibles
        objetos = GameObject.FindGameObjectsWithTag("enemy_Unit");
        int xfloor, xceil, zfloor, zceil;
        for(k=0; k<objetos.Length; k++)
        {
            g = (GameObject)objetos[k];
            posicion = g.transform.position;
            x = posicion[0];
            z = posicion[2];
            x = proporcion * (x - sizePlane / 2);
            z = proporcion * (z - sizePlane / 2);
            xfloor = Mathf.FloorToInt(x);
            zfloor = Mathf.FloorToInt(z);
            xceil = Mathf.CeilToInt(x);
            zceil = Mathf.CeilToInt(z);

            g.GetComponent<Renderer>().enabled = (texture.GetPixel(xfloor, zfloor)[3] < 0.5) || (texture.GetPixel(xceil, zceil)[3] < 0.5) || g.GetComponent<Visible>().edificio;

        }


    
        // Apply all SetPixel calls
        texture.Apply();

        // connect texture to material of GameObject this script is attached to
        GetComponent<Renderer>().material.mainTexture = texture;

    }
}
