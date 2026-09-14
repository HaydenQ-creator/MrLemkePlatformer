using Godot;
using System;

// The class name must perfectly match your file name: SpringArm3d
public partial class SpringArm3d : SpringArm3D
{
	// Node Paths - Editable in the Inspector if your names differ
	[Export] public NodePath PlayerPath = "../Player";
	[Export] public NodePath PivotPath = "..";

	private Node3D _cameraPivot;
	private CharacterBody3D _playerBody;

	// Camera Configuration
	[Export] public float MouseSensitivity = 0.002f;
	[Export] public float MinPitch = Mathf.DegToRad(-35f);
	[Export] public float MaxPitch = Mathf.DegToRad(80f);

	public override void _Ready()
	{
		// 1. Fetch node references using the NodePaths
		_cameraPivot = GetNode<Node3D>(PivotPath);
		_playerBody = GetNode<CharacterBody3D>(PlayerPath);

		// 2. Capture mouse cursor inside the game window
		Input.MouseMode = Input.MouseModeEnum.Captured;

		// 3. Exclude the player from camera collisions
		if (_playerBody != null)
		{
			AddExcludedObject(_playerBody.GetRid());
		}
		else
		{
			GD.PrintErr("SpringArm3d: Player node not found! Check your PlayerPath in the Inspector.");
		}
	}

	public override void _Process(double delta)
	{
		// 4. Keep the camera pivot glued to the player's position every frame
		if (_playerBody != null && _cameraPivot != null)
		{
			_cameraPivot.GlobalPosition = _playerBody.GlobalPosition;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (_cameraPivot == null) return;

		// Handle mouse look behavior
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// Rotate the pivot horizontally (Yaw)
			_cameraPivot.RotateY(-mouseMotion.Relative.X * MouseSensitivity);

			// Rotate this SpringArm3D vertically (Pitch)
			Vector3 armRotation = Rotation;
			armRotation.X -= mouseMotion.Relative.Y * MouseSensitivity;
			
			// Clamp the vertical look angle so the camera cannot flip upside down
			armRotation.X = Mathf.Clamp(armRotation.X, MinPitch, MaxPitch);
			Rotation = armRotation;
		}

		// Toggle mouse capture mode with Escape key
		if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured 
				? Input.MouseModeEnum.Visible 
				: Input.MouseModeEnum.Captured;
		}
	}
}
