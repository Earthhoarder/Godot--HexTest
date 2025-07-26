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
    //Each imported Hex is 128 tall by 64 wide
    //Lets have 5 units of distance between each one vertical and horizontal
    //128+5 = 132
    //64 + 5 = 69
    public const float hexInitialXOffset = (float)100f; //34.5f;
    public const float hexInitialYOffset = (float)66f; //66.0f;
    public const float hexXOffset = (float)100f; //69f;
    public const float hexYOffset = (float)132f; //132f;
    public static Vector2 DefaultMapSize = new Vector2(7, 7);
    #endregion

    //Constants for Hexes
    #region Hex
    public static Color DefaultHexColor = new Color(0, 1, 1); //Teal
    public static Color HoverHexColor = new Color(1, 0, 1); //Purple
    public static Color PathHexColor = new Color(1, 1, 0); //Green
    public static Color SelectedHexColor = new Color(0, 0, 1); //Blue
    public static Color EndingHexColor = new Color(1, 0.3125f, 0.3125f); //Pale green
    public static Color StartingHexColor = new Color(0.199218755f, 0.4f, 0.59765f); //Navy Blue
    public static Color NormalTerrainHexColor = new Color(0.1992f, 0.4f, 0); //Dark Green
    public static Color DifficultTerrainHexColor = new Color(0.4f, 0.199218755f, 0f); // Dark Brown
    
    #endregion

}
