using Godot;
using System;
using System.Collections.Generic;


//Global Enums
public enum Terrain
{
    Normal,
    Difficult,
    Paved,
    Infinite_Distance
}

public static class Constants
{

    //Weighted values for each type of terrain available
    public static Dictionary<Terrain, int> TerrainWeight = new Dictionary<Terrain, int>
    {
        {Terrain.Normal, 5},
        {Terrain.Difficult, 10},
        {Terrain.Paved, 2},
        {Terrain.Infinite_Distance, int.MaxValue}
    };
    
    public const float hexInitialOffset = (float)32.1;
	public const float hexXOffset = (float)74.2;
	public const float hexYOffset = (float)74.2;

}
