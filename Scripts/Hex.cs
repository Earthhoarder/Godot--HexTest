using Godot;
using System;
using System.Collections.Generic;
public partial class Hex : Node
{
	string id;
	int row;
	int col;
	//A list containing all adjacent hexes
	List<Hex> listAdjacent;
	Terrain kindOfTerrain;
	public string ID { get; set; }
	public int Row { get; set; }
	public int Col { get; set; }
	public List<Hex> ListAdjacent { get; set; }
	public Terrain KindOfTerrain { get; set; }

	//Initialiser with no variables
	public Hex()
	{

	}

	//Initialiser with all variables filled
	public Hex(string newId, int ROW, int COL, List<Hex> list, Terrain terrain)
	{
		this.ID = newId;
		this.Row = ROW;
		this.Col = COL;
		this.ListAdjacent = list;
		this.KindOfTerrain = terrain;
	}
}
