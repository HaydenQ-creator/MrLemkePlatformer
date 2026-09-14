using Godot;
using System;

public partial class FloatingLand : CharacterBody3D
{
	public static FloatingLand Instance { get; private set; }
	
	public const float Speed = 5.0f;
	public static bool is_touched = true;

	public override void _Process(double delta)
	{
		GD.Print(is_touched);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		if (is_touched == true)
		{
			velocity.Y -= Speed;
		}
	}

}
