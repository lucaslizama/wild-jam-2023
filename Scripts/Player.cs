using Godot;

public partial class Player : CharacterBody2D
{
  [Export] private float speed = 300.0f;
  [Export] private float jumpVelocity = 700.0f;
  [Export] private float gravityScale = 2f;
  [Export] private Color lineColor;

  private float gravity;
  private bool drawLine = false;

  public override void _Ready()
  {
    gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
  }

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
    }
    else if (Input.IsActionPressed("bomb"))
    {
      QueueRedraw();
    }
    else if (Input.IsActionJustReleased("bomb"))
    {
      drawLine = false;
      QueueRedraw();
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    ApplyGravity(delta);
    Jump();
    ApplyHorizontalSpeed();
    MoveAndSlide();
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
}
