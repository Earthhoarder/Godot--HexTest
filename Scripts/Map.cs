using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using System.Linq;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;



/// <summary>
/// Larger data structure containing many hexes that make up a map
/// </summary>
public partial class Map : Node2D
{
	//data structure for pathfinding of nodes
	private class Location
	{
		public string ID;
		public int x;
		public int y;
		public float F;
		public int G;
		public float H;
		public Location Parent;
		public Hex HexReference;
	}

	/// <summary>
	/// travel Weights for the terrain types
	/// </summary>


	[Export]
	public PackedScene HexScene { get; set; }


	public List<Hex> SelectedHexes { get; set; }

	List<List<Hex>> map;

	public List<List<Hex>> MAP
	{
		get { return map; }
	}

	private bool updateAdjacent;
	private bool firstFrameIncomplete;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Set default values for the class

		SelectedHexes = new List<Hex>();
		map = new List<List<Hex>>();
		updateAdjacent = true;
		firstFrameIncomplete = true;

		LoadEmptyMap((int)Constants.DefaultMapSize.X, (int)Constants.DefaultMapSize.Y);
		//GenerateEmptyMap((int)DefaultMapSize.X, (int)DefaultMapSize.Y);
		//LoadMap();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Pathfind"))
		{
			PathfindFromSelectedHexes();
		}
		if (Input.IsActionJustPressed("Load Empty Map"))
		{
			if (map.Count > 0)
			{
				GD.Print("Cannot Instantiate a new Map, old one exists. Please clear the old map before continuing");
			}
			else
			{
				LoadEmptyMap((int)Constants.DefaultMapSize.X, (int)Constants.DefaultMapSize.Y);
				updateAdjacent = true;
				firstFrameIncomplete = true;
			}
		}
		if (Input.IsActionJustPressed("Clear Map"))
		{
			ClearMap();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (updateAdjacent && firstFrameIncomplete == false)
		{
			UpdateAdjacentHexes();
			updateAdjacent = false;
		}
		else if (firstFrameIncomplete)
		{
			firstFrameIncomplete = false;
		}
	}

	/// <summary>
	/// Creates and loads an empty map into the world, with all variables filled for each hex
	/// </summary>
	/// <param name="columns"></param>
	/// <param name="rows"></param>
	public void LoadEmptyMap(int columns, int rows)
	{
		ClearSelected();
		map.Clear();
		for (int c = 0; c < columns; c++)
		{
			List<Hex> hexColumn = new List<Hex>();
			for (int r = 0; r < rows; r++)
			{
				Hex instance = (Hex)HexScene.Instantiate();
				if (instance is Node2D instance2D)
				{
					AddChild(instance);
					//figure out location of objects:
					instance.GlobalPosition = GetPositionOfHex(r, c);
					//Set the hex variables for easier ID
					instance.ID = r.ToString() + "." + c.ToString();
					instance.Row = r;
					instance.Col = c;
					instance.ListAdjacent = new List<Hex>();
					instance.KindOfTerrain = Terrain.Normal;
					hexColumn.Add(instance);
				}
				else
				{
					GD.Print("Cannot Convert Node to Node2D");
				}
			}
			map.Add(hexColumn);
		}
		ClearMapColoration();
	}

	/// <summary>
	/// Generates an empty map, using rows and columns
	/// </summary>
	public void GenerateEmptyMap(int columns, int rows)
	{
		map.Clear();
		//Assignes IDs in Columns
		for (int c = 0; c < columns; c++)
		{
			List<Hex> hexColumn = new List<Hex>();
			for (int r = 0; r < rows; r++)
			{
				Hex newHex = new Hex(r.ToString() + "." + c.ToString(), r, c, new List<Hex>(), Terrain.Normal);
				hexColumn.Add(newHex);
				GD.Print(newHex.ID);
			}
			map.Add(hexColumn);
		}
	}
	/// <summary>
	/// Loads a map
	/// </summary>
	public void LoadMap()
	{
		GD.Print("LoadMap called...");
		GD.Print("number of children @ Start: " + GetChildCount().ToString());
		for (int c = 0; c < map.Count; c++)
		{
			for (int r = 0; r < map[0].Count; r++)
			{
				Node2D instance = (Node2D)HexScene.Instantiate();
				if (instance is Node2D instance2D)
				{
					AddChild(instance2D);
					//figure out location of objects:
					instance.GlobalPosition = GetPositionOfHex(r, c);
				}
				else
				{
					GD.Print("Cannot Convert Node to Node2D");
				}
			}


		}
		GD.Print("number of children @ end: " + GetChildCount().ToString());
		GD.Print("LoadMap End!");
	}

	/// <summary>
	/// Takes the row and column of the current hex, outputs the in-world coordinates for the hex's location
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <returns>Vector2 containing in-world coordinates for the hex</returns>
	private Vector2 GetPositionOfHex(int row, int column)
	{
		if (column % 2 == 0) //if on odds (first column, third column, etc.)
		{
			if (row % 2 == 0) // if on odds (first row, third row, etc.)
			{
				float x = Constants.hexXOffset * (float)column;
				float y = Constants.hexYOffset * (float)row;
				GD.Print(x.ToString());
				GD.Print(y.ToString() + "\n");
				return new Vector2(x, y);
			}
			else //on even rows
			{
				float x = Constants.hexXOffset * (float)column;
				float y = Constants.hexYOffset * (float)row;
				GD.Print(x.ToString());
				GD.Print(y.ToString() + "\n");
				return new Vector2(x, y);
			}

		}
		else //on even columns
		{
			if (row % 2 == 0) // if on odds (first row, third row, etc.)
			{
				float x = Constants.hexXOffset * (float)column;
				float y = Constants.hexYOffset * (float)row + Constants.hexInitialYOffset;
				GD.Print(x.ToString());
				GD.Print(y.ToString() + "\n");
				return new Vector2(x, y);
			}
			else // on even rows and even columns
			{
				float x = Constants.hexXOffset * (float)column;
				float y = Constants.hexYOffset * (float)row + Constants.hexInitialYOffset;
				GD.Print(x.ToString());
				GD.Print(y.ToString() + "\n");
				return new Vector2(x, y);
			}
		}
	}

	public void UpdateAdjacentHexes()
	{
		GD.Print("UpdateAdjacentHexes called...");
		if (map.Count == 0)
		{
			GD.Print("Map is empty, nothing to check...");
			return;
		}

		Godot.Collections.Array<Node> children = GetChildren();
		foreach (Node child in children) //gets the SquareHex node
		{
			//GD.Print(child.Name);
			List<Hex> adjacent = new List<Hex>();
			Godot.Collections.Array<Node> subChildren = child.GetChildren();
			foreach (Node SpriteNode in subChildren) //gets the spriteNodes
			{
				Godot.Collections.Array<Node> spriteChildren = SpriteNode.GetChildren();
				foreach (Node AreaNode in spriteChildren) //gets the Area2D nodes
				{
					if (AreaNode.GetName() == "Circle_Around") //make sure we only check for collisions with our hex-specific collider
					{
						Godot.Collections.Array<Area2D> overlappingAreas = ((Area2D)AreaNode).GetOverlappingAreas();
						foreach (Area2D OA in overlappingAreas.Where(area => area.GetParent() != AreaNode.GetParent())) // for each overlap that isn't this collider's own hex...
						{
							adjacent.Add((Hex)((Node2D)OA.GetParent().GetParent()));
						}

					}

				}
			}
			((Hex)child).ListAdjacent = adjacent;
		}
		foreach (Node child in children)
		{
			string pMessage = "";
			pMessage += child.Name + " adjacent to hexes:\n";
			foreach (Hex adjacentHex in ((Hex)child).ListAdjacent)
			{
				pMessage += "\t" + adjacentHex.Name + "\n";
			}
			GD.Print(pMessage);
		}
		GD.Print("UpdateAdjacentHexes End!");
	}

	public List<Hex> FindPathFromAToB(Hex Start, Hex Goal) //A* algorithm basically
	{
		GD.Print("FindPathFromAToB called...");
		Location current = null;
		List<Location> openList = new List<Location>();
		List<Location> closedList = new List<Location>();
		int g = 0;
		openList.Add(ConvertHexToLocation(Start));

		List<Hex> visitedHexes = new List<Hex>();

		HelperFunctions.ColorHex(Start, Constants.StartingHexColor);

		GD.Print("good");
		while (openList.Count > 0)
		{
			var lowest = openList.Min(l => l.F);
			current = openList.FirstOrDefault(l => l.F == lowest);

			closedList.Add(current);
			openList.Remove(current);

			//Show in progress
			visitedHexes.Add(current.HexReference);
			if (current.HexReference != Start)
			{
				HelperFunctions.ColorHexPath(current.HexReference);
			}

			//Continue with the search
			if (closedList.FirstOrDefault(l => l.ID == Goal.ID) != null)
			{
				HelperFunctions.ColorHex(closedList.FirstOrDefault(l => l.ID == Goal.ID).HexReference, Constants.EndingHexColor);
				break; //found our goal location

			}

			List<Location> adjacentHexes = GetAdjacentHexesAsLocations(current);

			foreach (Location adjacentHex in adjacentHexes)
			{
				if (closedList.FirstOrDefault(l => l.ID == adjacentHex.ID) != null)
				{
					continue; //already in closedList, ignore
				}
				if (openList.FirstOrDefault(l => l.ID == adjacentHex.ID) == null)
				{
					//Not in open list, do stuff
					adjacentHex.G = g + Constants.TerrainWeight[adjacentHex.HexReference.KindOfTerrain];
					adjacentHex.H = ComputeHScore(adjacentHex.HexReference, Goal);
					adjacentHex.F = adjacentHex.G + adjacentHex.H;
					adjacentHex.Parent = current;

					openList.Insert(0, adjacentHex);
				}
				else // it is in the open list
				{
					int proposedG = (g + Constants.TerrainWeight[adjacentHex.HexReference.KindOfTerrain]);
					if (proposedG + adjacentHex.H < adjacentHex.F) //if we can get this hex cheaper than we have recorded
					{
						adjacentHex.G = proposedG;
						adjacentHex.F = adjacentHex.G + adjacentHex.H;
						adjacentHex.Parent = current;
					}
				}
			}
		}
		GD.Print("FindPathFromAToB End!");
		return visitedHexes;
	}

	private int GetCurrentPathWeight(List<Hex> TraversedPath)
	{
		int totalCost = 0;
		foreach (Hex hex in TraversedPath)
		{
			totalCost += Constants.TerrainWeight[hex.KindOfTerrain];
		}
		return totalCost;
	}

	static float ComputeHScore(Hex current, Hex goal)
	{
		return MathF.Abs(goal.Position.X - current.Position.X) + Mathf.Abs(goal.Position.Y - current.Position.Y);
		//return Mathf.Abs(goal.Row - current.Row) + Mathf.Abs(goal.Col - current.Col);
		//Turn into actual position

	}

	static Location ConvertHexToLocation(Hex hex)
	{
		Location returnedLocation = new Location();
		returnedLocation.ID = hex.ID;
		returnedLocation.x = hex.Col;
		returnedLocation.y = hex.Row;
		returnedLocation.F = int.MaxValue;
		returnedLocation.G = int.MaxValue;
		returnedLocation.HexReference = hex;
		return returnedLocation;
	}

	static List<Location> GetAdjacentHexesAsLocations(Location start)
	{
		List<Location> returnedLocations = new List<Location>();
		foreach (Hex adjacentHex in start.HexReference.ListAdjacent)
		{
			returnedLocations.Add(ConvertHexToLocation(adjacentHex));
		}
		return returnedLocations;
	}

	private Hex GetReferenceOfIndex(int row, int col)
	{
		GD.Print("GetReferenceOfIndex called...");
		Godot.Collections.Array<Node> children = GetChildren();
		foreach (Hex child in children)
		{
			if (child.Row == row && child.Col == col)
			{
				GD.Print("GetReferenceOfIndex End!");
				return child;
			}
		}
		GD.Print("GetReferenceOfIndex End! with null");
		return null;
	}

	public void GetPathFromIndex(int x1, int y1, int x2, int y2)
	{
		GD.Print("GetPathFromIndex called...");
		var traversedPath = FindPathFromAToB(GetReferenceOfIndex(x1, y1), GetReferenceOfIndex(x2, y2));

		GD.Print("Path from index: (" + x1.ToString() + "," + y1.ToString() + ") TO index: (" + x2.ToString() + "," + y2.ToString() + ") is:");
		foreach (var hex in traversedPath)
		{
			//GD.Print("\t" + hex.ID);
			GD.Print("\t" + hex.Row + "," + hex.Col);
		}
		GD.Print("GetPathFromIndex End!");
	}

	//Clears all coloration on the map, resets all hexes to default hex color
	public void ClearMapColoration()
	{
		Godot.Collections.Array<Node> children = GetChildren();
		foreach (Hex child in children)
		{
			HelperFunctions.ColorHexDefault(child);
		}
	}

	public void PathfindFromSelectedHexes()
	{
		if (SelectedHexes.Count < 2)
		{
			GD.Print("Not enough hexes selected(" + SelectedHexes.Count + ") to pathfind...");

		}
		else if (SelectedHexes.Count > 2)
		{
			GD.Print("Too many hexes selected(" + SelectedHexes.Count + ") to pathfind...");
		}
		else //exactly two 
		{
			var StartAndEnd = ReturnHexesAndClearSelected();
			ClearMapColoration();
			var traversedPath = FindPathFromAToB(StartAndEnd[0], StartAndEnd[1]);
			GD.Print("Path from index: (" + StartAndEnd[0].Row.ToString() + "," + StartAndEnd[0].Col.ToString() + ") TO index: (" + StartAndEnd[1].Row.ToString() + "," + StartAndEnd[1].Col.ToString() + ") is:");
			foreach (var hex in traversedPath)
			{
				//GD.Print("\t" + hex.ID);
				GD.Print("\t" + hex.Row + "," + hex.Col);
			}
			GD.Print("GetPathFromIndex End!");
		}
	}

	/// <summary>
	/// Gets the references in SelectedHexes and returns them, while removing dependencies in said hexes for selection purposes
	/// </summary>
	/// <returns>List<Hex> of the hexes in SelectedHexes </returns>
	public List<Hex> ReturnHexesAndClearSelected()
	{
		List<Hex> returnedList = new List<Hex>();
		foreach (Hex hex in SelectedHexes)
		{
			hex.GetChild<Sprite2D>(0).GetChild<HexInterior>(0).HexSelected = false;
			returnedList.Add(hex);
		}
		SelectedHexes.Clear();
		return returnedList;
	}

	//Clears SelectedHexes, including HexInterior script dependencies
	public void ClearSelected()
	{
		foreach (Hex hex in SelectedHexes)
		{
			hex.GetChild<Sprite2D>(0).GetChild<HexInterior>(0).HexSelected = false;
		}
		SelectedHexes.Clear();
	}

	public void ClearMap()
	{
		ClearSelected();
		foreach (List<Hex> hexColumn in map)
		{
			foreach (Hex hex in hexColumn)
			{

				hex.QueueFree();
			}
		}
		map.Clear();
	}

}
