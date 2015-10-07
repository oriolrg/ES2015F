using UnityEngine;
using System.Collections;

public class RectDrawer : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Returns the rect.
	public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		screenPosition1.y = Screen.height - screenPosition1.y;
		screenPosition2.y = Screen.height - screenPosition2.y;

		var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
		var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );

		return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
	}

	//This returns the boudns to detect units inside the rect.
	public static Bounds GetViewportBounds( Camera camera, Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		var v1 = camera.ScreenToViewportPoint( screenPosition1 );
		var v2 = camera.ScreenToViewportPoint( screenPosition2 );
		var min = Vector3.Min( v1, v2 );
		var max = Vector3.Max( v1, v2 );
		min.z = camera.nearClipPlane;
		max.z = camera.farClipPlane;
		
		var bounds = new Bounds();
		bounds.SetMinMax( min, max );
		return bounds;
	}

	//Draws the rect in the GUI.
	public static void DrawScreenRect( Rect rect, Color color )
	{
		GUI.color = color;
		GUI.DrawTexture( rect, new Texture2D( 1, 1 ));
		GUI.color = Color.white;
	}

	public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )  
	{
		// Top
		DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
		// Left
		DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
		// Right
		DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
		// Bottom
		DrawScreenRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
	}
}
