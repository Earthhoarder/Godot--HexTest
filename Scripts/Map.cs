using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// The types of terrain that a hex map can contain
/// </summary>
public enum Terrain
{
	Normal,
	Difficult,
	Paved
}

public partial class Map : Node2D
{
	/// <summary>
	/// 1 Tile on our hex grid
	/// </summary>


	//Properties
	const float initialOffset = (float)37.5;
	const float xOffset = (float)75.0;
	const float yOffset = (float)75.0;


	Godot.PackedScene hexObject;

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
		GD.Print(" ");
		GD.Print(map[0][4].ID.ToString());
		GD.Print(map.Count);
		GD.Print(map[0].Count);
		hexObject = GD.Load<PackedScene>("res://Objects/Square.tscn");
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
	public void LoadMap()
	{
		Node parent = this.GetParent();
		for (int c = 0; c <= map.Count; c++)
		{
			for (int r = 0; r <= map[0].Count; r++)
			{
				Godot.Area2D instance = (Area2D)hexObject.Instantiate();

				parent.AddChild(instance);
				//Godot.Node2D instance = new Godot.Node2D();
				//figure out location of objects:
				if (c % 2 == 0) //if on odds (first column, third column, etc.)
				{
					if (r % 2 == 0) // if on odds (first row, third row, etc.)
					{
						float x = xOffset * (float)c;
						float y = yOffset * (float)r;
						instance.GlobalPosition = new Vector2(x, y);
					}
					else //on even rows
					{
						float x = xOffset * (float)c + initialOffset;
						float y = yOffset * (float)r;
						instance.GlobalPosition = new Vector2(x, y);
					}

				}
				else //on even columns
				{
					if (r % 2 == 0) // if on odds (first row, third row, etc.)
					{
						float x = xOffset * (float)c;
						float y = yOffset * (float)r + initialOffset;
						instance.GlobalPosition = new Vector2(x, y);
					}
					else // on even rows and even columns
					{
						float x = xOffset * (float)c + initialOffset;
						float y = yOffset * (float)r + initialOffset;
						instance.GlobalPosition = new Vector2(x, y);
					}
				}

			}
		}
	}
}
