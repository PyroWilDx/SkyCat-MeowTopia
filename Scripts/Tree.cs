using Godot;
using SkyCatMeowtopia.Scripts;
using System;
using System.Collections.Generic;

public partial class Tree : StaticBody2D, IDamageable {
    private AnimatedSprite2D _animatedSprite;
    private Rect2 _colRect;
    private int _hp = 6;

    public override void _Ready() {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");

        CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShapeBlock");
        _colRect = new Rect2(Position - colShape.Shape.GetRect().Size / 2, colShape.Shape.GetRect().Size);
    }

    public override void _Process(double dt) {
        if (_hp <= 0 && !_animatedSprite.IsPlaying()) QueueFree();
    }

    public void _OnAreaBodyEntered(Node2D body) {
        if (body.Name == "MeowPlayer") {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_animatedSprite, "modulate",
                    new Color(1, 1, 1, 0.4f), 0.2f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Linear);
            tween.Play();
        }
    }

    public void _OnAreaBodyExited(Node2D body) {
        if (body.Name == "MeowPlayer") {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_animatedSprite, "modulate",
                    new Color(1, 1, 1, 1), 0.2f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Linear);
            tween.Play();
        }
    }

    private Rect2 GetCollisionRect() {
        return _colRect;
    }

    public void TakeDamage() {
        _hp--;
        if (_hp > 0) _animatedSprite.Play("Hurt");
        else _animatedSprite.Play("Death");
    }
}