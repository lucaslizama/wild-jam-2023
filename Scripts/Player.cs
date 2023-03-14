using Godot;

public partial class Player : CharacterBody2D
{
  [Export] private float speed = 300.0f;
  [Export] private float jumpVelocity = 700.0f;
  [Export] private float gravityScale = 2f;

  private float gravity;

  public override void _Ready()
  {
    gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
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
