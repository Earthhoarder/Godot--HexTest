using Godot;
using System;
using System.Collections.Generic;
public partial class Hex : Node2D
{
	public string ID { get; set; }
	public int Row { get; set; }
	public int Col { get; set; }
	public List<Hex> ListAdjacent { get; set; } //list of all adjacent hexes
	public Terrain KindOfTerrain { get; set; }
	public List<Node2D> ObjectsOnHex { get; set; }

	//Initialiser with no variables
	public Hex()
	{

	}

	public Hex(string newID, Terrain terrain)
	{
		this.ID = newID;
		this.KindOfTerrain = terrain;
	}

	//Initialiser with immediately known variables filled
	public Hex(string newId, int ROW, int COL, Terrain terrain)
	{
		this.ID = newId;
		this.Row = ROW;
		this.Col = COL;
		this.KindOfTerrain = terrain;
	}

	//Initialiser with most variables filled
	public Hex(string newId, int ROW, int COL, List<Hex> list, Terrain terrain)
	{
		this.ID = newId;
		this.Row = ROW;
		this.Col = COL;
		this.ListAdjacent = list;
		this.KindOfTerrain = terrain;
	}
}
