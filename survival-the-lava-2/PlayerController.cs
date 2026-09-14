using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	// --- Camera Settings ---
	[Export] public float MouseSensitivity = 0.002f;
	[Export] public float TiltLowerLimit = Mathf.DegToRad(-80f);
	[Export] public float TiltUpperLimit = Mathf.DegToRad(80f);

	// --- Movement Settings ---
	[Export] public float Speed = 5.0f;
	[Export] public float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Node3D _head;

	public override void _Ready()
	{
		_head = GetNode<Node3D>("Head");
		
		// Capture the mouse cursor inside the game window
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Handle mouse look
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(-mouseMotion.Relative.X * MouseSensitivity);
			_head.RotateX(-mouseMotion.Relative.Y * MouseSensitivity);

			Vector3 headRotation = _head.Rotation;
			headRotation.X = Mathf.Clamp(headRotation.X, TiltLowerLimit, TiltUpperLimit);
			_head.Rotation = headRotation;
		}

		// Toggle mouse capture with the Escape key
		if (@event.IsActionPressed("ui_cancel"))
		{
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
				Input.MouseMode = Input.MouseModeEnum.Visible;
			else
				Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// 1. Add gravity if the player is in the air
		if (!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;
		}

		// 2. Handle Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// 3. Get input direction based on the player's current horizontal heading
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		// Transform the 2D input direction relative to the Node3D's global forward/right direction
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		// 4. Smoothly apply horizontal movement (or friction if no keys are pressed)
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		// Apply everything to Godot's built-in physics engine
		Velocity = velocity;
		MoveAndSlide();
	}
}
