using System;
using Godot;

public partial class Player : TeleportableCharacterBody2D
{
  [Export] private float speed = 300.0f;
  [Export] private float jumpVelocity = 700.0f;
  [Export] private float gravityScale = 2f;
  [Export] private PackedScene bomb;
  private Color lineColor = new Color(1, 1, 1);

  private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
  private bool drawLine = false;

  public override void _Draw()
  {
    if (drawLine)
    {
      var origin = ToLocal(GlobalPosition);
      var target = GetLocalMousePosition();
      DrawDashedLine(origin, (target - origin).Normalized() * 250f, lineColor, 10f, 20f, false);
    }
  }

  public override void _Process(double delta)
  {
    if (Input.IsActionJustPressed("bomb"))
    {
      drawLine = true;

      var bombInstance = bomb.Instantiate<Bomb>();

      GetNode<Node2D>("BombSpawn").AddChild(bombInstance);
    }
    else if (Input.IsActionPressed("bomb"))
    {
      QueueRedraw();
    }
    else if (Input.IsActionJustReleased("bomb"))
    {
      drawLine = false;
      QueueRedraw();

      var bombInstance = GetNode<Bomb>("BombSpawn/Bomb");
      bombInstance.Position += Vector2.Up * 10;
      bombInstance.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale = new Vector2(0.8f, 0.8f);
      bombInstance.GetNode<CollisionShape2D>("CollisionShape2D").Position = new Vector2(12.8f, 12.8f);
      bombInstance.GetNode<CollisionShape2D>("CollisionShape2D").Scale = new Vector2(4, 4);

      var direction = (GetLocalMousePosition() - ToLocal(GlobalPosition)).Normalized();
      bombInstance.Throw(direction);
    }
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
    if (Velocity.X != 0)
    {
      GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("run");
      if (Velocity.X < 0)
      {
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
      }
      else if (Velocity.X >= 0)
      {
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
      }
    }
    else
    {
      GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
    }
  }

  public void ApplyGravity(double delta)
  {
    if (!IsOnFloor()) Velocity += Vector2.Down * gravity * gravityScale * (float)delta;
  }

  public void Jump()
  {
    if (Input.IsActionJustPressed("jump") && IsOnFloor())
    {
      Velocity += Vector2.Up * jumpVelocity;
    }
  }

  public void ApplyHorizontalSpeed()
  {
    var direction = Vector2.Zero;
    var velocity = Velocity;

    direction.X += Input.IsActionPressed("right") ? 1f : Input.IsActionPressed("left") ? -1f : 0f;
    velocity.X = direction != Vector2.Zero ? direction.X * speed : Mathf.MoveToward(velocity.X, 0, speed);

    Velocity = velocity;
  }

  public override void Teleport(Vector2 destination)
  {
    Position = destination;
  }
}
