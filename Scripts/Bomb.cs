using Godot;
using WildJam2023.Scripts.Extensions;

namespace WildJam2023.Scripts;

public partial class Bomb : Teleportable.TeleportableRigidbody2D
{
    [Export] private float throwVelocity;

    // Nodes
    private AnimatedSprite2D sprite;
    private CollisionShape2D collider;

    // Properties
    public AnimatedSprite2D Sprite => sprite;
    public CollisionShape2D Collider => collider;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collider = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void Throw(Vector2 direction)
    {
        Position += Vector2.Up * 10;
        Sprite.Scale = new Vector2(0.8f, 0.8f);
        Collider.Position = new Vector2(12.8f, 12.8f);
        Collider.Scale = new Vector2(4, 4);
        Freeze = false;
        Reparent(this.GetRootNode());
        ApplyImpulse(direction * throwVelocity);
    }

    private async void OnBodyEntered(Node2D body)
    {
        if (body is Portal) return;

        SetDeferred(RigidBody2D.PropertyName.Freeze, true);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("explosion");
        await ToSignal(GetNode<AnimatedSprite2D>("AnimatedSprite2D"), AnimatedSprite2D.SignalName.AnimationFinished);
        QueueFree();
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (WillTeleport && TeleportTarget != Vector2.Zero)
        {
            var transform = state.Transform;
            transform.Origin = TeleportTarget;
            state.Transform = transform;
            WillTeleport = false;
            TeleportTarget = Vector2.Zero;
        }
    }

    public override void Teleport(Vector2 destination)
    {
        TeleportTarget = destination;
        WillTeleport = true;
    }
}