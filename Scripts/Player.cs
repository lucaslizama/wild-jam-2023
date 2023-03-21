using Godot;
using Godot.Collections;
using System.Linq;
using WildJam2023.Scripts.Extensions;

namespace WildJam2023.Scripts;

public partial class Player : Teleportable.TeleportableCharacterBody2D
{
	[ExportGroup("Movement")] 
	[Export] private float speed = 300.0f;
	[Export] private float jumpVelocity = 700.0f;
	[Export] private float gravityScale = 2f;

	[ExportGroup("Aim")] 
	[Export] private bool drawLine = false;
	[Export] private Color lineColor = new Color(1, 1, 1);
	[Export] private PackedScene bombScene;

	[ExportGroup("Status")] 
	[Export] private int hp = 3;
	[Export] private Dictionary<string, Texture2D> spriteSets;

	// Node References
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animState;
	private Sprite2D sprite;
	private Bomb bombInstance;
	private Node2D bombSpawn;

	// Constants
	private readonly float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite");
		bombSpawn = GetNode<Node2D>("BombSpawn");
		animTree = GetNode<AnimationTree>("AnimationTree");
		animState = animTree.GetStateMachinePlayback();
	}

	public override void _Draw()
	{
		if (!drawLine) return;
		var origin = ToLocal(GlobalPosition);
		var target = GetLocalMousePosition();
		DrawDashedLine(origin, (target - origin).Normalized() * 250f, lineColor, 10f, 20f, false);
	}

	public override void _Process(double delta)
	{
		ProcessBombThrow();
		if (Input.IsActionJustPressed("damage")) GetHurt();
		if (Input.IsActionJustPressed("recover")) Recover();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		ApplyGravity(delta);
		Jump();
		ApplyHorizontalSpeed();
		MoveAndSlide();
		ProcessAnimations();
	}

	private void ProcessAnimations()
	{
		ProcessRun();
		ProcessIdle();
		ProcessJump();
		ProcessLand();
		ProcessSpriteFlip();
	}

	private void ProcessIdle()
	{
		var validOriginStates = new[] {"run", "land"};
		var currentState = animState.GetCurrentNode().ToString();
		if (!validOriginStates.Contains(currentState)) return;
		if (Velocity.X == 0) animState.Travel("idle");
	}

	private void ProcessRun()
	{
		var validOriginStates = new[] {"idle", "land"};
		var currentState = animState.GetCurrentNode().ToString();
		if (!validOriginStates.Contains(currentState)) return;
		if (Velocity.X != 0) animState.Travel("run");
	}

	private void ProcessJump()
	{
		var validOriginStates = new[] {"idle", "run"};
		var currentState = animState.GetCurrentNode().ToString();
		if (!validOriginStates.Contains(currentState)) return;
		if (!IsOnFloor()) animState.Travel("jump");
	}

	private void ProcessLand()
	{
		var validOriginStates = new[] {"jump"};
		var currentState = animState.GetCurrentNode().ToString();
		if (!validOriginStates.Contains(currentState)) return;
		if(IsOnFloor()) animState.Travel("land");
	}

	private void ProcessSpriteFlip()
	{
		sprite.FlipH = Velocity.X switch
		{
			< 0 => true,
			> 0 => false,
			_ => sprite.FlipH
		};
	}
	
	private void ProcessBombThrow()
	{
		if (Input.IsActionJustPressed("bomb"))
		{
			drawLine = true;
			bombInstance = bombScene.Instantiate<Bomb>();
			bombSpawn.AddChild(bombInstance);
		}
		else if (Input.IsActionPressed("bomb"))
		{
			QueueRedraw();
		}
		else if (Input.IsActionJustReleased("bomb"))
		{
			drawLine = false;
			QueueRedraw();
			var direction = (GetLocalMousePosition() - ToLocal(GlobalPosition)).Normalized();
			bombInstance.Throw(direction);
		}
	}

	private void ApplyGravity(double delta)
	{
		if (!IsOnFloor()) Velocity += Vector2.Down * gravity * gravityScale * (float) delta;
	}

	private void Jump()
	{
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			Velocity += Vector2.Up * jumpVelocity;
		}
	}

	private void ApplyHorizontalSpeed()
	{
		var direction = Vector2.Zero;
		var velocity = Velocity;

		direction.X += Input.IsActionPressed("right") ? 1f : Input.IsActionPressed("left") ? -1f : 0f;
		velocity.X = direction != Vector2.Zero ? direction.X * speed : Mathf.MoveToward(velocity.X, 0, speed);

		Velocity = velocity;
	}

	private void GetHurt()
	{
		hp = Mathf.Clamp(hp - 1, 0, 3);
		AssignTexture();
	}

	private void Recover()
	{
		hp = Mathf.Clamp(hp + 1, 0, 3);
		AssignTexture();
	}

	private void AssignTexture()
	{
		sprite.Texture = hp switch
		{
			3 => spriteSets["healthy"],
			2 => spriteSets["hurt"],
			1 => spriteSets["hurting"],
			_ => spriteSets["healthy"]
		};
	}

	public override void Teleport(Vector2 destination)
	{
		Position = destination;
	}
}
