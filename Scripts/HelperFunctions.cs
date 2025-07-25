using Godot;
using System;

public static class HelperFunctions
{

    //Hex Functions
    #region Hex Functions
    //Colors the hex as a pathed hex
    public static void ColorHexPath(Hex hex)
    {
        ((Sprite2D)hex.GetChild(0)).Modulate = Constants.PathHexColor;
        hex.GetChild<Sprite2D>(0).GetChild<HexInterior>(0).NormalColor = Constants.PathHexColor;
    }

    //Colors the hex as the default hex color
    public static void ColorHexDefault(Hex hex)
    {
        ((Sprite2D)hex.GetChild(0)).Modulate = Constants.DefaultHexColor;
        hex.GetChild<Sprite2D>(0).GetChild<HexInterior>(0).NormalColor = Constants.DefaultHexColor;
    }

    static public void ColorHexNormal(Hex hex)
    {
        ((Sprite2D)hex.GetChild(0)).Modulate = hex.GetChild<Sprite2D>(0).GetChild<HexInterior>(0).NormalColor;
    }

    static public void ColorHexSelected(Hex hex)
    {
        ((Sprite2D)hex.GetChild(0)).Modulate = Constants.SelectedHexColor;
    }

    public static void ColorHex(Hex hex, Color color)
    {
        ((Sprite2D)hex.GetChild(0)).Modulate = color;
        hex.GetChild<Sprite2D>(0).GetChild<HexInterior>(0).NormalColor = color;
    }
    
    #endregion
}
