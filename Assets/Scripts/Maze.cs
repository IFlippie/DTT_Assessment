using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	[HideInInspector]
	public IntVector2 size;

	public MazeCell cellPrefab;
	public GameObject player, pickUp;

	public float generationStepDelay;

	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private MazeCell[,] cells;

	private int fillPercentage = 70;
	private int spawnPercentage = 5;

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}
	public IEnumerator GenerateAutomata()
	{
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		for (int x = 0; x < size.x; x++)
		{
			for (int z = 0; z < size.z; z++)
			{
				int randomValue = Random.Range(0,100);
				if (randomValue < fillPercentage) 
				{
					yield return delay;
					CreateCell(new IntVector2(x,z));
				}			
			}
		}
	}


	public IEnumerator Generate () 
	{
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) {
			yield return delay;
			DoNextGenerationStep(activeCells);
		}
		SpawnPlayerAndPickups();
	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells) 
	{
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) 
	{
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
            return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
				if (neighbor == null) 
				{
					neighbor = CreateCell(coordinates);
					CreatePassage(currentCell, neighbor, direction);
					activeCells.Add(neighbor);
				}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
	}

	private MazeCell CreateCell (IntVector2 coordinates) 
	{
		MazeCell newCell = Instantiate(cellPrefab);
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) 
	{
		MazePassage passage = Instantiate(passagePrefab);
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab);
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) 
	{
		MazeWall wall = Instantiate(wallPrefab);
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab);
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private void SpawnPlayerAndPickups() 
	{
		for (int x = 0; x < size.x; x++)
		{
			for (int z = 0; z < size.z; z++)
			{
				int randomValue = Random.Range(0, 100);
				if (randomValue < spawnPercentage)
				{
					Vector3 pickUpPos = GetCell(new IntVector2(x, z)).transform.position;
					GameObject newPickup = Instantiate(pickUp);
                    newPickup.transform.parent = null;
					newPickup.transform.position = new Vector3(pickUpPos.x, pickUpPos.y + 0.5f, pickUpPos.z);
				}
			}
		}
		Vector3 playerPos = GetCell(new IntVector2(0, 0)).transform.position;
		GameObject newPlayer = Instantiate(player);
		newPlayer.transform.parent = null;
		newPlayer.transform.position = new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z);
	}
}