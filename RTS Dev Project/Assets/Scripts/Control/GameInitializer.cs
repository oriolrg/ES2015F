using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour {

	[SerializeField] private GameObject sceneMap; // Map that will be replaced by GameData.map
	[SerializeField] private GameObject minimap; // To replace variable Ground
	[SerializeField] private GameObject buildings;

	private LOSManager terrainLOSManager;
	private bool firstUpdate = true; // used to activate FoW when first Update

	void Awake() {
		GameDataEditorPicker gmep = GetComponent<GameDataEditorPicker>();
		if (!GameData.sceneFromMenu){
			if (gmep != null){
				// Only use those values when not coming from the menu
				gmep.AddFakeGameData();
			}

			if (!GameData.sceneFromMenu){ // could have changed with AddFake
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
		// throw new UnityException("GameInitializer not implemented yet"); // TODO: Delete this

		// Change scene map
		Destroy(sceneMap);

		terrainLOSManager = GameData.map.GetComponent<LOSManager>();
		terrainLOSManager.enabled = false;

		GameObject map = (GameObject) Instantiate (
			GameData.map, 
			Vector3.zero,
			Quaternion.identity
		);

		MapInfo mapInfo = map.GetComponent<MapInfo>();

		terrainLOSManager = map.GetComponent<LOSManager>();
		// Remember to activate LOSManager when everything is set

		// Change minimap settings
		MinimapCamera minimapCamera = minimap.GetComponent<MinimapCamera>();
		minimapCamera.ground = map;
		minimapCamera.updateCameraAttributes();

		// Replace current Town Centers by those detailed by GameData and GameData.map
		// Delete old Town Centers
		foreach (Transform t in buildings.transform) {
			if (t.gameObject.name.Contains("TownCenter"))
				Destroy(t.gameObject);
		}

		// Instantiate new Town Centers according to the number of players

		// Player
		GameObject townCenterPrefab = DataManager.Instance.civilizationDatas[
			Utils.GetEnumValue<Civilization>(GameData.player.civ.ToString())
		].units[UnitType.TownCenter];

		GameObject townCenter = (GameObject) Instantiate (
			townCenterPrefab, 
			mapInfo.towncenter1.transform.position,
			mapInfo.towncenter1.transform.rotation
		);

		townCenter.name = "PlayerTownCenter";
		townCenter.transform.SetParent(buildings.transform);

		townCenter.tag = "Ally";
		GameController.Instance.addSelectedPrefab(townCenter);

		int cpus = 0;
		Transform townCenterTransform;
		foreach (GameData.CPUData cpu in GameData.cpus) {
			cpus++;

			townCenterPrefab = DataManager.Instance.civilizationDatas[
				Utils.GetEnumValue<Civilization>(cpu.civ.ToString())
			].units[UnitType.TownCenter];

			if (cpus == 1)
				townCenterTransform = mapInfo.towncenter2.transform;
			else if (cpus == 2)
				townCenterTransform = mapInfo.towncenter3.transform;
			else if (cpus == 3)
				townCenterTransform = mapInfo.towncenter4.transform;
			else
				continue;

			townCenter = (GameObject) Instantiate (
				townCenterPrefab, 
				townCenterTransform.position,
				townCenterTransform.rotation
			);

			townCenter.name = "CPU" + cpus.ToString() + "TownCenter";
			townCenter.transform.SetParent(buildings.transform);
		}
	}

	void Update() {
		if (firstUpdate){
			terrainLOSManager.enabled = true;
			firstUpdate = false;
		}
	}
}