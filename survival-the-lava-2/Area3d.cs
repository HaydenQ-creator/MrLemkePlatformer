using Godot;
using System;

public partial class Area3d : Area3D
{
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Corrected signal handler syntax and type matching
	private void OnBodyEntered(Node3D body)
	{
		if (body is CharacterBody3D)
		{
			//FloatingLand.is_touched = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
