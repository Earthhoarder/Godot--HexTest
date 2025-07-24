using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D
{

/// <summary>
/// The types of terrain that a hex map can contain
/// </summary>
	public enum Terrain
	{
		Normal,
		Difficult,
		Paved
	}

	/// <summary>
	/// 1 Tile on our hex grid
	/// </summary>
	public class Hex
	{
		string id;
		//A list containing all adjacent hexes
		List<Hex> listAdjacent;
		Terrain kindOfTerrain;

		public string ID
		{
			get { return ID; }
			set { id = value; }
		}
		public List<Hex> ListAdjacent
		{
			get { return listAdjacent; }
			set { listAdjacent = value; }
		}
		public Terrain KindOfTerrain
		{
			get { return kindOfTerrain; }
			set { kindOfTerrain = value; }
		}
	}

	//Properties

	List<List<Hex>> map;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = new List<List<Hex>>();
		GenerateEmptyMap(10, 10);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	/// <summary>
	/// Generates an empty map, using rows and columns
	/// </summary>
	public void GenerateEmptyMap(int columns, int rows)
	{
		map.Clear();
		//Assignes IDs in columns. IDs are relevant to hex location
		for (int r = 0; r <= rows; r++)
		{
			List<Hex> hexColumn = new List<Hex>();
			for (int c = 0; c <= columns; c++)
			{
				Hex newHex = new Hex();
				newHex.ID = r.ToString()+ "." + c.ToString();
				newHex.KindOfTerrain = Terrain.Normal;
				hexColumn.Add(newHex);
				GD.Print(newHex);
			}
			map.Add(hexColumn);
		}
	}
}
