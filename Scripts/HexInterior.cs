using Godot;
using Godot.NativeInterop;
using System;

public partial class HexInterior : Area2D
{
	// Called when the node enters the scene tree for the first time.

	//Properties
	public Color NormalColor { get; set; }

	private bool MouseInside = false;

	private bool HexSelected = false;

	public override void _Ready()
	{
		//Connect our collision functions to collision events:
		this.Connect("mouse_entered", new Callable(this, MethodName.OnMouseEnter));
		this.Connect("mouse_exited", new Callable(this, MethodName.OnMouseExit));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Select Hex") && MouseInside)
		{
			GD.Print("Hex has been selected");
			HexSelected = true;
			HelperFunctions.ColorHexSelected(this.GetParent<Sprite2D>().GetParent<Hex>());

		}
		else if (Input.IsActionJustPressed("Unselect Hex") && MouseInside)
		{
			GD.Print("Hex has been unselected");
			HexSelected = false;
			HelperFunctions.ColorHexNormal(this.GetParent<Sprite2D>().GetParent<Hex>());
		}
	}

	//Code for when the mouse hovers over the square
	private void OnMouseEnter()
	{
		//GD.Print(this.Name + " mouse ENTER");
		if (!HexSelected)
		{
			Sprite2D parent = (Sprite2D)this.GetParent();
			parent.Modulate = Constants.HoverHexColor;
		}
		MouseInside = true;
	}

	private void OnMouseExit()
	{
		//GD.Print(this.Name + " mouse EXIT");
		if (!HexSelected)
		{
			Sprite2D parent = (Sprite2D)this.GetParent();
			parent.Modulate = NormalColor;
		}
		MouseInside = false;
	}

	
}
