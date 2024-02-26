using Godot;
using SkyCatMeowtopia.Scripts;
using System;

public partial class MeowAttack : Area2D {
    public static MeowAttack Instanciate(Node2D parent, Vector2 position,
            float radius, float height, float rotation) {
        PackedScene packedScene = (PackedScene) ResourceLoader.Load("res://Scenes/Entity/MeowAttack.tscn");
        Node spawnedAttack = packedScene.Instantiate();
        
        MeowAttack meowAttack = (MeowAttack) spawnedAttack;
        parent.AddChild(meowAttack);
        meowAttack.Position = position;
        meowAttack._atkArea.Radius = radius;
        meowAttack._atkArea.Height = height;
        meowAttack.Rotation = rotation;

        return meowAttack;
    }

    private CapsuleShape2D _atkArea;
    private double _timeSinceCreation;
    private double _maxDuration;

    public override void _Ready() {
        CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShape");
        _atkArea = (CapsuleShape2D) colShape.Shape;
        _timeSinceCreation = 0;
        _maxDuration = 0;
    }

    public override void _Process(double dt) {
        _timeSinceCreation += dt;
        if (_timeSinceCreation > _maxDuration) QueueFree();
    }

    public void _OnAreaBodyEntered(Node2D body) {
        if (body is IDamageable) {
            ((IDamageable) body).TakeDamage();
        }
    }

    public void SetMaxDuration(double value) {
        _maxDuration = value;
    }
}