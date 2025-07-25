using Godot;
using System;
using System.Collections.Generic;
using System.Net;


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


    //Constants for map creation
    #region Map Creation
	//Each imported Square is 64.2 wide
	//Lets have 10 units of distance between each one vertical and horizontal
	//64.2 + 10 = 74.2
    public const float hexInitialOffset = (float)37.1;
    public const float hexXOffset = (float)74.2;
    public const float hexYOffset = (float)74.2;
    public static Vector2 DefaultMapSize = new Vector2(7, 7);
    #endregion

    //Constants for Hexes
    #region Hex
    public static Color DefaultHexColor = new Color(0, 1, 1);
    public static Color HoverHexColor = new Color(1, 0, 1);
    public static Color PathHexColor = new Color(1, 1, 0);
    public static Color SelectedHexColor = new Color(0, 0, 1);
    #endregion

}
