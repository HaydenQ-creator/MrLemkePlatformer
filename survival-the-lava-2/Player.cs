using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Movement configurations using Export to allow editing in the Inspector
	[Export] public float Speed = 1.0f;
	[Export] public float JumpVelocity = 4.5f;
	[Export] public float SprintSpeed = 10.0f;

	// Get the gravity from the project settings so it matches the engine physics
	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();



	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// 1. Apply Gravity if the character is in the air
		if (!IsOnFloor())
		{
			velocity.Y -= Gravity * (float)delta;
		}

		// 2. Handle Jump input
		// Note: Default Godot action "ui_accept" maps to Spacebar/Enter
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		if (Input.IsActionJustPressed("sprint") && IsOnFloor())
		{
			Speed = SprintSpeed;
		}
		if (Input.IsActionJustReleased("sprint"))
		{
			Speed = 1;
		}


		// 3. Fetch direction vectors based on keyboard/gamepad inputs
		// Maps default UI actions to a 2D vector (X = Horizontal, Y = Vertical)
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		
		// 4. Translate 2D inputs into 3D world space relative to the player's orientation
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		// 5. Apply horizontal movement or decelerate smoothly
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			// Smoothly slow down the player when no keys are pressed
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		// 6. Assign the modified velocity and execute engine physics movement
		Velocity = velocity;
		MoveAndSlide();
	}
}
