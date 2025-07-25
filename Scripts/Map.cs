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

	[Export]
	public PackedScene SquareScene { get; set; }

	//Properties
	//Each imported Square is 64.2 wide
	//Lets have 10 units of distance between each one vertical and horizontal
	//64.2 + 10 = 74.2
	const float initialOffset = (float)32.1;
	const float xOffset = (float)74.2;
	const float yOffset = (float)74.2;


	Godot.PackedScene hexObject;

	List<List<Hex>> map;

	List<List<Hex>> MAP
	{
		get { return map; }
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hexObject = GD.Load<PackedScene>("res://Objects/Square.tscn");
		map = new List<List<Hex>>();
		GenerateEmptyMap(1, 1);
		GD.Print(" ");
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
		GD.Print("LoadMap called");
		Node parent = GetNode("Map");

		//GD.Print("number of children @ start: " + parent.GetChildCount().ToString());
		GD.Print("LoadMap called 2");

		//GD.Print(map.Count);
		//GD.Print(map[0].Count);

		for (int c = 0; c < map.Count; c++)
		{
			for (int r = 0; r < map[0].Count; r++)
			{
				Node2D instance = (Node2D)SquareScene.Instantiate();
				if (instance is Node2D instance2D)
				{
					AddChild(instance2D);

					//figure out location of objects:
					if (c % 2 == 0) //if on odds (first column, third column, etc.)
					{
						if (r % 2 == 0) // if on odds (first row, third row, etc.)
						{
							float x = xOffset * (float)c;
							float y = yOffset * (float)r;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							instance.GlobalPosition = new Vector2(x, y);
						}
						else //on even rows
						{
							float x = xOffset * (float)c;
							float y = yOffset * (float)r;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							instance.GlobalPosition = new Vector2(x, y);
						}

					}
					else //on even columns
					{
						if (r % 2 == 0) // if on odds (first row, third row, etc.)
						{
							float x = xOffset * (float)c;
							float y = yOffset * (float)r + initialOffset;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							instance.GlobalPosition = new Vector2(x, y);
						}
						else // on even rows and even columns
						{
							float x = xOffset * (float)c;
							float y = yOffset * (float)r + initialOffset;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							instance.GlobalPosition = new Vector2(x, y);
						}
					}

					// Debug: await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
				}
				else
				{
					GD.Print("Cannot Convert Node to Node2D");
				}
			}


		}
		GD.Print("number of children @ end: " + GetChildCount().ToString());

		Godot.Collections.Array<Node> children = GetChildren();
		//GD.Print(children.name);
		foreach (Node child in children)
		{
			Godot.Collections.Array<Node> subChildren = GetChildren();
			foreach (Node subChild in subChildren)
			{
				GD.Print(subChild.GetName());
			}
		}
		

	}
	
	//public void _on_body_
}
