using Godot;
using System;

public partial class FloatingLand : StaticBody3D
{
	// The boolean that the Area3D will trigger
	public bool is_touched { get; set; } = true;

	[Export]
	public float DescentSpeed { get; set; } = 0.7f;

	public override void _PhysicsProcess(double delta)
	{
		// Only move down if the Area3D has detected the player
		if (is_touched)
		{
			Vector3 movement = new Vector3(0, -DescentSpeed * (float)delta, 0);
			GlobalPosition += movement;
		}
	}
}
