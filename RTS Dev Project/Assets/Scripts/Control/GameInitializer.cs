using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour {

	[SerializeField] private GameObject sceneMap; // Map that will be replaced by GameData.map
	[SerializeField] private GameObject minimap; // To replace variable Ground
	[SerializeField] private List<GameObject> townCenters; // Town centers that will be erased when Initializing

	void Awake() {
		GameDataEditorPicker gmep = GetComponent<GameDataEditorPicker>();
		if (!GameData.sceneFromMenu){
			if (gmep != null && gmep.isActiveAndEnabled)
				// Only use those values when not coming from the menu
				gmep.AddFakeGameData();
			else {
				// Don't instantiate anything
				// Only do that when coming from the menu or when GameDataEditorPicker is enabled
				this.enabled = false; // this ensures that this.Start isn't called
				return;
			}
		}

		// If we get here, it means we have to change the scene.
		// Do the first changes

		// Reallocate mainCamera wherever map tells us
		Transform newCameraTransform = GameData.map.transform.Find ("Main Camera");
		Camera.main.transform.position = newCameraTransform.position;
		Camera.main.transform.rotation = newCameraTransform.rotation;
	}

	void Start() {
		// Replace current scene map by GameData.map
		throw new UnityException("GameInitializer not implemented yet"); // TODO: Delete this

		Destroy(sceneMap);
		GameObject map = (GameObject) Instantiate (
			GameData.map, 
			Vector3.zero, //GameData.map.GetComponent<TerrainCollider>().bounds.extents / 2f, 
			Quaternion.identity
		);

		MinimapCamera minimapCamera = minimap.GetComponent<MinimapCamera>();
		minimapCamera.ground = map;
		minimapCamera.updateCameraAttributes();
		
		// Replace current Town Centers by those detailed by GameData and GameData.map
		// Delete old Town Centers

		// Instantiate new Town Centers according to the number of players
	}
}