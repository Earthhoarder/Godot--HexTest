using Godot;
using Godot.NativeInterop;
using System;

public partial class HexInterior : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Connect our collision functions to collision events:
		this.Connect("mouse_entered", new Callable(this, MethodName.OnMouseEnter));
		this.Connect("mouse_exited", new Callable(this, MethodName.OnMouseExit));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//Code for when the mouse hovers over the square
	private void OnMouseEnter()
	{
		GD.Print(this.Name + " mouse ENTER");
	}

	private void OnMouseExit()
	{
		GD.Print(this.Name + " mouse EXIT");
	}
	
}
