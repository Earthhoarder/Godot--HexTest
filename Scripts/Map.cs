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
		int row;
		int col;
		//A list containing all adjacent hexes
		List<Hex> listAdjacent;
		Terrain kindOfTerrain;

		public string ID
		{
			get { return ID; }
			set { id = value; }
		}

		public int Row
		{
			get { return row; }
			set { row = value; }
		}

		public int Col
		{
			get { return col; }
			set { col = value; }
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
	double initialOffset = 75.0;
	double xOffset = 75.0;
	double yOffset = 75.0;

	
	 Godot.PackedScene hexObject = GD.Load<PackedScene>("res://Objects/defaultSquare.tscn"); 

	List<List<Hex>> map;

	List<List<Hex>> MAP
	{
		get { return map; }
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = new List<List<Hex>>();
		GenerateEmptyMap(10, 10);
		//GD.Print(map[0][4].ID.ToString());
		LoadMap();
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
				newHex.ID = r.ToString() + " " + c.ToString();
				newHex.Row = r;
				newHex.Col = c;
				newHex.KindOfTerrain = Terrain.Normal;
				hexColumn.Add(newHex);
				GD.Print(newHex.ID);
			}
			map.Add(hexColumn);
		}
	}
	public void LoadMap()
	{
		for (int r = 0; r <= map.Count; r++)
		{
			for (int c = 0; c <= map[0].Count; c++)
			{
				Godot.StaticBody2D instance = (StaticBody2D)hexObject.Instantiate(); 
				AddChild(instance);
				instance.GlobalPosition =  new Vector2(0, 0);
			}
		}
	}
}
