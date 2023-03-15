using Godot;

public partial class Player : CharacterBody2D
{
  [Export] private float speed = 300.0f;
  [Export] private float jumpVelocity = 700.0f;
  [Export] private float gravityScale = 2f;
  [Export] private PackedScene bomb;
  private Color lineColor = new Color(1, 1, 1);

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
      bombInstance.Freeze = false;
      bombInstance.Reparent(GetParent());

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
