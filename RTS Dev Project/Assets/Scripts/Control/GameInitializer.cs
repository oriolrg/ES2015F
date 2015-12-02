using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour {

	[SerializeField] private GameObject sceneMap; // Map that will be replaced by GameData.map
	[SerializeField] private GameObject minimap; // To replace variable Ground
	[SerializeField] private GameObject buildings;
	[SerializeField] private GameObject units;

	[SerializeField] private HUD hud;

	private LOSManager terrainLOSManager;
	private GameObject createdMap;
	private List<GameObject> townCenters;
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
		Camera.main.transform.position = newCameraTransform.localPosition;
		Camera.main.transform.rotation = newCameraTransform.localRotation;
	}

	void Start() {
		// Replace current scene map by GameData.map
		// throw new UnityException("GameInitializer not implemented yet"); // TODO: Delete this

		// Change scene map
		if (sceneMap != null)
			Destroy(sceneMap);

		// Deactive LOSManager before Instantiating
		terrainLOSManager = GameData.map.GetComponent<LOSManager>();
		terrainLOSManager.enabled = false; // remember to restore prefab later

		// Deactivate A* too
		foreach (Transform t in GameData.map.transform){
			if (t.gameObject.name.Equals ("A*"))
				t.gameObject.SetActive(false); // remember to reactivate it later in prefab
		}

		createdMap = (GameObject) Instantiate (
			GameData.map, 
			Vector3.zero,
			Quaternion.identity
		);

		terrainLOSManager.enabled = true; // restore prefab

		// Reactivate A* too
		foreach (Transform t in GameData.map.transform){
			if (t.gameObject.name.Equals ("A*"))
				t.gameObject.SetActive(true);
		}

		MapInfo mapInfo = createdMap.GetComponent<MapInfo>();

		terrainLOSManager = createdMap.GetComponent<LOSManager>();
		// Remember to activate LOSManager when everything is set

		// Change minimap settings
		MinimapCamera minimapCamera = minimap.GetComponent<MinimapCamera>();
		minimapCamera.ground = createdMap;
		minimapCamera.updateCameraAttributes();

		// Replace current Town Centers by those detailed by GameData and GameData.map
		// Delete old Town Centers
		foreach (Transform t in buildings.transform) {
			Destroy(t.gameObject);
		}

		// Delete old Units
		foreach (Transform t in units.transform) {
			Destroy(t.gameObject);
		}

		// Instantiate new Town Centers according to the number of players
		LOSEntity townCenterLOSEntity;
		townCenters = new List<GameObject>();

		// Player
		Civilization playerCiv = Utils.GetEnumValue<Civilization>(GameData.player.civ.ToString());

		GameObject townCenterPrefab = DataManager.Instance.civilizationDatas[
			playerCiv
		].units[UnitType.TownCenter];

		hud.setCivilization(playerCiv);

		// Instantiate player town Center
		GameObject townCenter = (GameObject) Instantiate (
			townCenterPrefab, 
			mapInfo.towncenter1.transform.position,
			mapInfo.towncenter1.transform.rotation
		);

        Identity iden = townCenter.GetComponent<Identity>();
        if (iden != null) iden.player = Player.Player;

		townCenter.name = "PlayerTownCenter";
		townCenter.transform.SetParent(buildings.transform);

		townCenter.tag = "Ally";
		GameController.Instance.addSelectedPrefab(townCenter);

		townCenters.Add(townCenter);

		int cpus = 0;
		Transform townCenterTransform;
        Player player;
        Identity idenCPU;
        foreach (GameData.CPUData cpu in GameData.cpus) {
			cpus++;

			townCenterPrefab = DataManager.Instance.civilizationDatas[cpu.civ].units[UnitType.TownCenter];

            if (cpus == 1)
            {
                townCenterTransform = mapInfo.towncenter2.transform;
                player = Player.CPU1;
            }
            else if (cpus == 2)
            {
                townCenterTransform = mapInfo.towncenter3.transform;
                player = Player.CPU2;
            }
            else if (cpus == 3)
            {
                townCenterTransform = mapInfo.towncenter4.transform;
                player = Player.CPU3;
            }
            else
                continue;

			townCenter = (GameObject) Instantiate (
				townCenterPrefab, 
				townCenterTransform.position,
				townCenterTransform.rotation
			);

            idenCPU = townCenter.GetComponent<Identity>();
            if (idenCPU != null)
            {
                idenCPU.player = player;
            }

            townCenter.name = "CPU" + cpus.ToString() + "TownCenter";
			townCenter.transform.SetParent(buildings.transform);
			townCenter.tag = "Enemy";
			townCenters.Add(townCenter);
		}
		GameController.Instance.hud.hideRightPanel();
        GameController.Instance.addSelectedPrefabstoCurrentUnits();
        GameController.Instance.addTeamCirclePrefabstoCurrentUnits();
    }

	void Update() {
		if (firstUpdate){
			terrainLOSManager.enabled = true;

			foreach (Transform t in createdMap.transform){
				if (t.gameObject.name.Equals ("A*"))
				    t.gameObject.SetActive(true);
			}

			bool townCenterPlayer = true;
			LOSEntity townCenterLOSEntity;
			foreach (GameObject townCenter in townCenters) {
				if (townCenterPlayer){
					townCenterLOSEntity = townCenter.GetComponent<LOSEntity>();
					if (townCenterLOSEntity != null){
						townCenterLOSEntity.IsRevealer = true;
						townCenterLOSEntity.RevealState = LOSEntity.RevealStates.Unfogged;

						// To apply those changes, we need to restart it
						townCenterLOSEntity.enabled = false;
						townCenterLOSEntity.enabled = true;
					}

					townCenterPlayer = false;
				} else {
					townCenterLOSEntity = townCenter.GetComponent<LOSEntity>();
					if (townCenterLOSEntity != null){
						townCenterLOSEntity.IsRevealer = false;
						townCenterLOSEntity.RevealState = LOSEntity.RevealStates.Fogged;

						// To apply those changes, we need to restart it
						townCenterLOSEntity.enabled = false;
						townCenterLOSEntity.enabled = true;
					}
				}
			}

            GameController.Instance.spawnRandomObjectives();

            firstUpdate = false;

        }
	}
}