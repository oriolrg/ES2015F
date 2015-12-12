using UnityEngine;
using System.Collections.Generic;
using System;

public class timerDeath : MonoBehaviour
{
	
	public List<GameObject> unitsGoingHere;
	public Vector3 [,] formationMatrix;

	public int formationMatrixSize;

	public Vector2 formationMatrixSizeTri;
	public int diagonal;
	public int direction;
	public int numberOfElements;

	public int unitNumber;

	private int row;
	private int changeRow;
	private Vector2 unitPosition;
	private int x;
	private int y;
	// Use this for initialization

	void Awake () 
	{
		unitsGoingHere = new List<GameObject> ();
		unitPosition = new Vector2();
		row = 0;
		unitNumber = 0;
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
		print (formationMatrix[x,y]);
		Vector3 newPosition = Vector3.zero;
		newPosition.x = gameObject.transform.position.x + formationMatrix[x,y].x;
		newPosition.z = gameObject.transform.position.z - formationMatrix[x,y].z;
		return newPosition;
	}

	public Vector3 AddUnitMouseSelectionTri (GameObject unit)
	{
		unitsGoingHere.Add(unit);
		diagonal = (int)(formationMatrixSizeTri.y - 1)/2;
		setUnitPositionTri(unit);
		x = (int)unitPosition.x;
		y = (int)unitPosition.y;
		fillFormationMatrixTri(unit);
		Vector3 newPosition = Vector3.zero;

		if(direction == 0){
			if(y > diagonal){
				newPosition.x = gameObject.transform.position.x + formationMatrix[x,2*diagonal - y].x;
				newPosition.z = gameObject.transform.position.z + formationMatrix[x,2*diagonal - y].z;
			} else {
				newPosition.x = gameObject.transform.position.x + formationMatrix[x,y].x;
				newPosition.z = gameObject.transform.position.z - formationMatrix[x,y].z;
			}
		} else if (direction == 1){
			if(y > diagonal){
				newPosition.x = gameObject.transform.position.x - formationMatrix[x,2*diagonal - y].x;
				newPosition.z = gameObject.transform.position.z + formationMatrix[x,2*diagonal - y].z;
			} else {
				newPosition.x = gameObject.transform.position.x - formationMatrix[x,y].x;
				newPosition.z = gameObject.transform.position.z - formationMatrix[x,y].z;
			}

		} else if (direction == 2) {
			if(y > diagonal){
				newPosition.z = gameObject.transform.position.z - formationMatrix[x,2*diagonal - y].x;
				newPosition.x = gameObject.transform.position.x - formationMatrix[x,2*diagonal - y].z;
			} else {
				newPosition.z = gameObject.transform.position.z - formationMatrix[x,y].x;
				newPosition.x = gameObject.transform.position.x + formationMatrix[x,y].z;
			}
		} else {
			if(y > diagonal){
				newPosition.z = gameObject.transform.position.z + formationMatrix[x,2*diagonal - y].x;
				newPosition.x = gameObject.transform.position.x - formationMatrix[x,2*diagonal - y].z;
			} else {
				newPosition.z = gameObject.transform.position.z + formationMatrix[x,y].x;
				newPosition.x = gameObject.transform.position.x + formationMatrix[x,y].z;
			}
		}
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

	public void setDirection(Vector3 direction){

		float absValueZ = Mathf.Abs(direction.z);
		float absValueX = Mathf.Abs(direction.x);
		float valueX = direction.x;
		float valueZ = direction.z;

		if(absValueZ < 10 && valueX > 0){
			this.direction = 0;
		} else if(absValueZ < 10 && valueX < 0){
			this.direction = 1;
		} else if(absValueX < 10 && valueZ < 0){
			this.direction = 2;
		} else if(absValueX < 10 && valueZ > 0){
			this.direction = 3;
		}
	}
	

	public void setUnitPositionTri(GameObject unit){
		numberOfElements = elementsInRow(row);
		int index = unitsGoingHere.IndexOf(unit);

		unitPosition.x = row;
		unitPosition.y = unitNumber + diagonal - row;

		if(unitNumber + 1 == numberOfElements){
			unitNumber = 0;
			row++;
		} else {
			unitNumber++;
		}
	}
	

	public int elementsInRow(int actual_row){
		return 2*actual_row + 1;
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

	private void fillFormationMatrixTri(GameObject unit){
		Vector3 unitOffSet = getUnitOffSet(unit);
		if(x == 0){
			formationMatrix[x + 1,y - 1] = formationMatrix[x,y] + new Vector3(unitOffSet.x,0f,unitOffSet.z);
			formationMatrix[x + 1,y] = formationMatrix[x,y] + new Vector3(unitOffSet.x,0f,0f);
		} else if (y + 1 == diagonal && x != formationMatrixSizeTri.x - 1) {
			formationMatrix[x + 1,y - 1] = formationMatrix[x,y] + new Vector3(unitOffSet.x,0f,unitOffSet.z);
			formationMatrix[x + 1,y] = formationMatrix[x,y] + new Vector3(unitOffSet.x,0f,0f);
		} else if (y + 1 < diagonal && x != formationMatrixSizeTri.x - 1) {
			formationMatrix[x + 1,y - 1] = formationMatrix[x,y] + new Vector3(unitOffSet.x,0f,unitOffSet.z);
		} else if (y == diagonal &&  x != formationMatrixSizeTri.x - 1 && x != 0){
			formationMatrix[x + 1,y] = formationMatrix[x,y] + new Vector3(unitOffSet.x,0f,0f);
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

	public void setFormationMatrixTri(Vector2 fMSize){
		formationMatrixSizeTri = fMSize;
		//print ("Creating Formation Matrix with size: " + formationMatrixSizeTri.x.ToString()+"x"+formationMatrixSizeTri.y.ToString());
		formationMatrix = new Vector3[(int)formationMatrixSizeTri.x,(int)formationMatrixSizeTri.y];
		for(int i = 0; i < formationMatrixSizeTri.x; i++){
			for(int j = 0; j < formationMatrixSizeTri.y; j++){
				formationMatrix[i,j] = Vector3.zero;
			}
		}
	}
}

