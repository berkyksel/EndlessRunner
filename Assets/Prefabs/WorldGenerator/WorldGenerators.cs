using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class WorldGenerators : MonoBehaviour
{
    [Header("Road Blocks")]
    [SerializeField] float EvnMoveSpeed = 4f;
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;
    [SerializeField] GameObject[] roadBlocks;

    [Header("Buildings")]
    [SerializeField] GameObject[] buildings;
    [SerializeField] Transform[] buildingSpawnPoints;
    [SerializeField] Vector2 BuildingSpawnScaleRange = new Vector2(0.6f, 0.8f);

    [Header("Street Lights")]
    [SerializeField] GameObject StreetLight;
    [SerializeField] Transform[] StreetSpawnPoints;

    [Header("Threats")]
    [SerializeField] Vector3 OccupationDetectionHalfExtend;
    [SerializeField] Threat[] Threats;
    [SerializeField] Transform[] Lanes;

    Vector3 MoveDirection;

    [Header("Pickups")]
    [SerializeField] Pickup[] pickups;

    private bool isMovementActive = false;

    bool GetRandomSpawnPoint(out Vector3 spawnPoint, string OccupationOnChecking)
    {
        // Müsait noktalarý alýyoruz (Liste dönüyor)
        Vector3[] spawnPoints = GetAvailableSpawnPoints(OccupationOnChecking);

        // HATA BURADAYDI: Lanes.Length yerine spawnPoints.Length kontrol edilmeli
        if (spawnPoints.Length == 0)
        {
            spawnPoint = Vector3.zero;
            return false;
        }

        // HATA BURADAYDI: Lanes.Length deðil, spawnPoints içinden rastgele seçmelisin
        int pick = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[pick];
        return true;
    }

    Vector3[] GetAvailableSpawnPoints(string OccupationOnChecking)
    {
        List<Vector3> AvailableSpawnPoints = new List<Vector3>();
        foreach (Transform spawnTrans in Lanes)
        {
            Vector3 spawnPoint = spawnTrans.position + new Vector3(0, 0, StartPoint.position.z);
            if (!GamePlayStatics.IsPositionOccupied(spawnPoint, OccupationDetectionHalfExtend , OccupationOnChecking))
            {
                AvailableSpawnPoints.Add(spawnPoint);
            }
        }
        return AvailableSpawnPoints.ToArray();
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 nextBlockPosition = StartPoint.position;
        float EndPointDistance = Vector3.Distance(StartPoint.position, EndPoint.position);
        MoveDirection = (EndPoint.position - StartPoint.position).normalized;
        while (Vector3.Distance(StartPoint.position, nextBlockPosition) < EndPointDistance)
        {
            GameObject newBlock = SpawnNewBlock(nextBlockPosition, MoveDirection);

            float blockLength = newBlock.GetComponent<Renderer>().bounds.size.z;
            nextBlockPosition += MoveDirection * blockLength;
        }

        StartSpawnElements();

        Pickup newPickup = Instantiate(pickups[0], StartPoint.position, Quaternion.identity);
        newPickup.GetComponent<MovementComp>().SetDestination(EndPoint.position);
        newPickup.GetComponent<MovementComp>().SetMoveDir(MoveDirection);
    }

    private void StartSpawnElements()
    {
        foreach (Threat threat in Threats)
        {
            StartCoroutine(SpawnElement(threat));
        }
        foreach (Pickup pickup in pickups)
        {
            StartCoroutine(SpawnElement(pickup));
        }
    }

    IEnumerator SpawnElement(Spawnable elementToSpawn)
    {
        while (true)
        {
            if (GetRandomSpawnPoint(out Vector3 spawnPoint, elementToSpawn.gameObject.tag))
            {
                Spawnable newThreat = Instantiate(elementToSpawn, spawnPoint, Quaternion.identity);

                newThreat.GetMovementComponent().SetDestination(EndPoint.position);
                newThreat.GetMovementComponent().SetMoveDir(MoveDirection);
            }

            yield return new WaitForSeconds(elementToSpawn.SpawnInterval);
        }
    }

    private void StartAllMovement()
    {
        isMovementActive = true;

        // Sahnedeki tüm "Road" tag'li nesneleri bul ve hareketlerini aç
        GameObject[] existingRoads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in existingRoads)
        {
            MovementComp mc = road.GetComponent<MovementComp>();
            if (mc != null) mc.enabled = true;
        }
        Debug.Log("Yol hareketi baþlatýldý!");
    }

    GameObject SpawnNewBlock(Vector3 SpawnPosition, Vector3 MoveDir)
    {
        int pick = Random.Range(0, roadBlocks.Length);
        GameObject newBlock = Instantiate(roadBlocks[pick]);
        newBlock.transform.position = SpawnPosition;

        // Bloklara "Road" tag'ini kodla veriyoruz (Unuttuysanýz garanti olur)
        newBlock.tag = "Road";

        MovementComp moveComp = newBlock.GetComponent<MovementComp>();
        if (moveComp != null)
        {
            moveComp.SetMoveSpeed(EvnMoveSpeed);
            moveComp.SetDestination(EndPoint.position);
            moveComp.SetMoveDir(MoveDir);

            // BAÞLANGIÇTA DURDUR: Eðer H'ye basýlmadýysa scripti kapat
            moveComp.enabled = isMovementActive;
        }

        SpawnBuildings(newBlock);
        SpawnStreetLights(newBlock);

        return newBlock;
    }

    private void SpawnStreetLights(GameObject ParentBlock)
    {
        foreach (Transform StreetLightSpawnPoint in StreetSpawnPoints)
        {
            Vector3 SpawnLoc = ParentBlock.transform.position + (StreetLightSpawnPoint.position - StartPoint.position);
            Quaternion SpawnRot = Quaternion.LookRotation((StartPoint.position - StreetLightSpawnPoint.position).normalized, Vector3.up);
            Quaternion SpawnRotOffset = Quaternion.Euler(0, -90, 0);
            GameObject newStreetLight = Instantiate(StreetLight, SpawnLoc, SpawnRot * SpawnRotOffset, ParentBlock.transform);
        }
    }

    private void SpawnBuildings(GameObject ParentBlock)
    {
        foreach (Transform BuildingSpawnPoint in buildingSpawnPoints)
        {
            Vector3 BuildingSpawnLoc = ParentBlock.transform.position + (BuildingSpawnPoint.position - StartPoint.position);
            int RotationOffsetBy90 = Random.Range(0, 3);
            Quaternion BuildingSpawnRoatation = Quaternion.Euler(0, RotationOffsetBy90 * 90, 0);
            Vector3 BuildingSpawnSize = Vector3.one * Random.Range(BuildingSpawnScaleRange.x, BuildingSpawnScaleRange.y);
            int BuildingPick = Random.Range(0, buildings.Length);

            GameObject newBuilding = Instantiate(buildings[BuildingPick], BuildingSpawnLoc, BuildingSpawnRoatation, ParentBlock.transform);
            newBuilding.transform.localScale = BuildingSpawnSize;
        }
    }



    // Update is called once per frame
    void Update()
    {
        // --- H TUÞU KONTROLÜ ---
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartAllMovement();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        MovementComp moveComp = other.GetComponent<MovementComp>();
        Renderer otherRenderer = other.GetComponent<Renderer>();

        // Road block deðilse çýk
        if (moveComp == null || otherRenderer == null)
            return;

        if (moveComp.spawnedNext == false)
        {
            moveComp.spawnedNext = true;

            GameObject newBlock = SpawnNewBlock(other.transform.position, MoveDirection);

            Renderer newRenderer = newBlock.GetComponent<Renderer>();
            if (newRenderer == null)
                return;

            float newBlockHalfWidth = newRenderer.bounds.size.z / 2f;
            float previousBlockHalfWidth = otherRenderer.bounds.size.z / 2f;

            Vector3 offset = -(newBlockHalfWidth + previousBlockHalfWidth) * MoveDirection;
            newBlock.transform.position += offset;
        }
    }


}
