using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour {

	[SerializeField] private GameObject sceneMap; // Map that will be replaced by GameData.map
	[SerializeField] private GameObject minimap; // To replace variable Ground
	[SerializeField] private List<GameObject> townCenters; // Town centers that will be erased when Initializing

	void Awake() {
		GameDataEditorPicker gmep = GetComponent<GameDataEditorPicker>();
		if (gmep != null) 
			gmep.AddFakeGameData();

		if (!GameData.sceneFromMenu) {
			// Don't instantiate anything; only do that when coming from the menu.
			this.enabled = false;
			return;
		}

		// Reallocate mainCamera wherever map tells us
		Transform newCameraTransform = GameData.map.transform.Find ("Main Camera");
		Camera.main.transform.position = newCameraTransform.position;
		Camera.main.transform.rotation = newCameraTransform.rotation;
	}

	void Start() {
		// Replace current scene map by GameData.map
		Destroy(sceneMap);
		GameObject map = (GameObject) Instantiate (
			GameData.map, 
			Vector3.zero, //GameData.map.GetComponent<TerrainCollider>().bounds.extents / 2f, 
			Quaternion.identity
		);

		MinimapCamera minimapCamera = minimap.GetComponent<MinimapCamera>();
		minimapCamera.ground = map;
		minimapCamera.updateCameraAttributes();
		//minimapCamera.updateViewport();
		
		// Replace current Town Centers by those detailed by GameData and GameData.map
	}
}