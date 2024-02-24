using Godot;
using SkyCatMeowtopia.Scripts;
using System;
using System.Collections.Generic;

public partial class MeowPlayer : CharacterBody2D {
    public enum State {
        None,
        Idle,
        Walk,
        Run,
        Axe,
        Shovel,
        Pickaxe
    }

    private static readonly Dictionary<State, string> AnimMap =
        new Dictionary<State, string>() {
            { State.Idle, "Idle" },
            { State.Walk, "Walk" },
            { State.Run, "Run" },
            { State.Axe, "Axe" },
            { State.Shovel, "Shovel" },
            { State.Pickaxe, "Pickaxe" }
        };

    [Export] private TileMap _tileMap = null;
    private static readonly int _GroundLayer = 1;
    private static readonly int _BuildingLayer = 2;

    private Utils.Direction _direction = Utils.Direction.Down;

    private AnimatedSprite2D _animatedSprite = null;
    private State _state = State.Idle;

    private static readonly int Speed = 100;
    private static readonly int RunSpeed = 160;

    public override void _Ready() {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");

        SetProcessInput(true);
    }

    public override void _PhysicsProcess(double dt) {
        UpdateVelocity();
        UpdateState();

        if (CanMove()) MoveAndSlide();

        string animName = AnimMap[_state];

        switch (_direction) {
            case Utils.Direction.Up:
                animName += "Up";
                break;
            case Utils.Direction.Down:
                animName += "Down";
                break;
            case Utils.Direction.Left:
                animName += "Left";
                break;
            case Utils.Direction.Right:
                animName += "Right";
                break;
            default:
                GD.PrintErr("Error : MeowPlayer Direction");
                break;
        }

        _animatedSprite.Play(animName);
    }

    private void UpdateVelocity() {
        Vector2 nextVelocity = Vector2.Zero;
        
        if (Input.IsActionPressed("Up")) nextVelocity.Y -= 1;
        if (Input.IsActionPressed("Down")) nextVelocity.Y += 1;
        if (Input.IsActionPressed("Left")) nextVelocity.X -= 1;
        if (Input.IsActionPressed("Right")) nextVelocity.X += 1;
        
        nextVelocity.X *= 2;
        
        Velocity = nextVelocity.Normalized();
        if (!Input.IsActionPressed("Run")) Velocity *= Speed;
        else Velocity *= RunSpeed;
        
        if (Velocity != Vector2.Zero) {
            if (Velocity.Y < 0) _direction = Utils.Direction.Up;
            if (Velocity.Y > 0) _direction = Utils.Direction.Down;
            if (Velocity.X < 0) _direction = Utils.Direction.Left;
            if (Velocity.X > 0) _direction = Utils.Direction.Right;
        }
    }

    private void UpdateState() {
        if (Input.IsActionPressed("Axe")) {
            _state = State.Axe;
        } else if (Input.IsActionPressed("Shovel")) {
            _state = State.Shovel;
        } else if (Input.IsActionPressed("Pickaxe")) {
            _state = State.Pickaxe;
        } else if (Velocity != Vector2.Zero) {
            _state = Input.IsActionPressed("Run") ? State.Run : State.Walk;
        } else {
            _state = State.Idle;
        }
    }

    private bool CanMove() {
        return _state == State.Walk ||
               _state == State.Run;
    }
}