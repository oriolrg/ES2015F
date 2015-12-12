using UnityEngine;
using System.Collections.Generic;
using System;

public class timerDeath : MonoBehaviour
{
	
	public List<GameObject> unitsGoingHere;
	public Vector3 [,] formationMatrix;
	public int formationMatrixSize;
	private int row;
	private Vector2 unitPosition;
	private int x;
	private int y;
	// Use this for initialization
	void Awake () 
	{
		unitsGoingHere = new List<GameObject> ();
		unitPosition = new Vector2();
		row = 0;
	}
	
	public void AddUnit( GameObject unit )
	{
		unitsGoingHere.Add (unit);
	}

	public Vector3 AddUnitMouseSelection (GameObject unit)
	{
		unitsGoingHere.Add(unit);
		setUnitPosition(unit);
		x = (int)unitPosition.x;
		y = (int)unitPosition.y;
		fillFormationMatrix(unit);
		Vector3 newPosition = Vector3.zero;
		newPosition.x = gameObject.transform.position.x + formationMatrix[x,y].x;
		newPosition.z = gameObject.transform.position.z - formationMatrix[x,y].z;
		return newPosition;
	}


	public void UnitLostTarget( GameObject unit )
	{
		unitsGoingHere.Remove (unit);
		if( unitsGoingHere.Count == 0 )
			Destroy (gameObject);
	}

	public void setUnitPosition(GameObject unit){
		unitPosition.y = (unitsGoingHere.IndexOf(unit)) % formationMatrixSize;
		if(unitPosition.y == 0 && unitsGoingHere.IndexOf(unit) != 0) row++;
		unitPosition.x = row;
	}

	private void fillFormationMatrix(GameObject unit){
		Vector3 unitOffSet = getUnitOffSet(unit);

		//print ("x: " + x.ToString() + " y: " +y.ToString() + " M: " + formationMatrixSize.ToString());

		for(int i = y + 1; i<formationMatrixSize; i++){
			//print ("x: " + x.ToString() + " y: " +i.ToString());
			formationMatrix[x,i] += new Vector3(unitOffSet.x,0.0f,0.0f);
		}
		for(int j = x + 1; j<formationMatrixSize; j++){
			//print ("x: " + j.ToString() + " y: " +y.ToString());
			formationMatrix[j,y] += new Vector3(0.0f,0.0f,unitOffSet.z);
		}

	}

	private Vector3 getUnitOffSet(GameObject unit){
		Collider unitCollider = unit.GetComponent<Collider>();
		Vector3 offSet = new Vector3();

		if(unitCollider!=null){
			offSet.x = 2*unitCollider.bounds.extents.x + 1;
			offSet.y = 0.0f;
			offSet.z = 2*unitCollider.bounds.extents.z + 1;
			return offSet;
		} else {
			return Vector3.zero;
		}
	}

	public void setFormationMatrix(int fMSize){
		formationMatrixSize = fMSize;
		//print ("Creating Formation Matrix with size: " + formationMatrixSize.ToString());
		formationMatrix = new Vector3[formationMatrixSize,formationMatrixSize];
		for(int i = 0; i < formationMatrixSize; i++){
			for(int j = 0; j < formationMatrixSize; j++){
				formationMatrix[i,j] = Vector3.zero;
			}
		}
	}
}

