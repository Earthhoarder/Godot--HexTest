using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using System.Linq;



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

	Vector2 DefaultMapSize = new Vector2(5, 5);


	Godot.PackedScene hexObject;

	List<List<Hex>> map;

	List<List<Hex>> MAP
	{
		get { return map; }
	}

	private bool updateAdjacent;
	private bool firstFrameIncomplete;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		firstFrameIncomplete = true;
		updateAdjacent = true;
		hexObject = GD.Load<PackedScene>("res://Objects/Square.tscn");
		map = new List<List<Hex>>();
		GenerateEmptyMap((int)DefaultMapSize.X, (int)DefaultMapSize.Y);
		GD.Print(" ");
		//GD.Print(map[0][4].ID.ToString());

		LoadMap();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

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
		GD.Print("LoadMap called...");
		Node parent = GetNode("Map");
		GD.Print("number of children @ Start: " + GetChildCount().ToString());
		for (int c = 0; c < map.Count; c++)
		{
			for (int r = 0; r < map[0].Count; r++)
			{
				Node2D instance = (Node2D)SquareScene.Instantiate();
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

	private Vector2 GetPositionOfHex(int row, int column)
	{
		if (column % 2 == 0) //if on odds (first column, third column, etc.)
					{
						if (row % 2 == 0) // if on odds (first row, third row, etc.)
						{
							float x = xOffset * (float)column;
							float y = yOffset * (float)row;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							return new Vector2(x, y);
						}
						else //on even rows
						{
							float x = xOffset * (float)column;
							float y = yOffset * (float)row;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							return new Vector2(x, y);
						}

					}
					else //on even columns
					{
						if (row % 2 == 0) // if on odds (first row, third row, etc.)
						{
							float x = xOffset * (float)column;
							float y = yOffset * (float)row + initialOffset;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							return new Vector2(x, y);
						}
						else // on even rows and even columns
						{
							float x = xOffset * (float)column;
							float y = yOffset * (float)row + initialOffset;
							GD.Print(x.ToString());
							GD.Print(y.ToString() + "\n");
							return new Vector2(x, y);
						}
					}
	}

	public void UpdateAdjacentHexes()
	{
		GD.Print("UpdateAdjacentHexes called...");
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

	//public void _on_body_
}
