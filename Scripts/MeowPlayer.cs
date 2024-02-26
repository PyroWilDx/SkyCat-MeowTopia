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

    private static readonly Dictionary<State, string> AnimMap = new Dictionary<State, string>() {
        { State.Idle, "Idle" },
        { State.Walk, "Walk" },
        { State.Run, "Run" },
        { State.Axe, "Axe" },
        { State.Shovel, "Shovel" },
        { State.Pickaxe, "Pickaxe" }
    };

    private Utils.Direction _currDirection = Utils.Direction.Down;

    private AnimatedSprite2D _animatedSprite = null;
    private int _lastAnimationFrame = 0;
    private bool _justChangedFrame = false;
    private State _currState = State.Idle;

    private const int Speed = 100;
    private const int RunSpeed = 160;

    public override void _Ready() {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");

        SetProcessInput(true);
    }

    public override void _PhysicsProcess(double dt) {
        UpdateVelocity();
        UpdateState();

        if (CanMove()) MoveAndSlide();

        UpdateAnimation();

        DoAction();
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
            if (Velocity.Y < 0) _currDirection = Utils.Direction.Up;
            if (Velocity.Y > 0) _currDirection = Utils.Direction.Down;
            if (Velocity.X < 0) _currDirection = Utils.Direction.Left;
            if (Velocity.X > 0) _currDirection = Utils.Direction.Right;
        }
    }

    private void UpdateState() {
        if (Input.IsActionPressed("Axe")) {
            _currState = State.Axe;
        } else if (Input.IsActionPressed("Shovel")) {
            _currState = State.Shovel;
        } else if (Input.IsActionPressed("Pickaxe")) {
            _currState = State.Pickaxe;
        } else if (Velocity != Vector2.Zero) {
            _currState = Input.IsActionPressed("Run") ? State.Run : State.Walk;
        } else {
            _currState = State.Idle;
        }
    }

    private bool CanMove() {
        return _currState is State.Walk or State.Run;
    }

    private void UpdateAnimation() {
        _justChangedFrame = false;

        string animName = AnimMap[_currState];

        switch (_currDirection) {
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

        if (_animatedSprite.Animation != animName) _justChangedFrame = true;
        else if (_lastAnimationFrame != _animatedSprite.Frame) _justChangedFrame = true;

        _lastAnimationFrame = _animatedSprite.Frame;

        _animatedSprite.Play(animName);
    }

    private void DoAction() {
        switch (_currState) {
            case State.Axe:
                Axe();
                break;
        }
    }

    private bool IsFrameNew(int frameNumber) {
        return _justChangedFrame && _animatedSprite.Frame == frameNumber;
    }

    private void Axe() {
        if (IsFrameNew(6)) {
            Rect2 hitRect = new Rect2();
            switch (_currDirection) {
                case Utils.Direction.Up:
                    hitRect = new Rect2(Position.X, Position.Y - 14, 10, 18);
                    break;
                case Utils.Direction.Down:
                    hitRect = new Rect2(Position.X - 10, Position.Y - 6, 10, 18);
                    break;
                case Utils.Direction.Left:
                    hitRect = new Rect2(Position.X - 18, Position.Y - 2, 18, 10);
                    break;
                case Utils.Direction.Right:
                    hitRect = new Rect2(Position.X, Position.Y - 2, 18, 10);
                    break;
                default:
                    GD.PrintErr("Error : MeowPlayer Axe HitRect");
                    break;
            }

            Tree.HitTree(hitRect);
        }
    }
}