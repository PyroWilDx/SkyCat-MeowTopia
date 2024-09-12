using Godot;
using SkyCatMeowTopia.Scripts;
using System;
using System.Collections.Generic;

public partial class Tree : StaticBody2D, IDamageable {
    private AnimatedSprite2D _animatedSprite;
    private int _hp = 6;

    public override void _Ready() {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
    }

    public override void _Process(double dt) {
        if (!IsAlive()) {
            if (!_animatedSprite.IsPlaying()) QueueFree();
        }
    }

    public void _OnAreaBodyEntered(Node2D body) {
        if (body.Name == "MeowPlayer" && IsAlive()) {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_animatedSprite, "modulate",
                    new Color(1, 1, 1, 0.4f), 0.2f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Linear);
            tween.Play();
        }
    }

    public void _OnAreaBodyExited(Node2D body) {
        if (body.Name == "MeowPlayer" && IsAlive()) {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_animatedSprite, "modulate",
                    new Color(1, 1, 1, 1), 0.2f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Linear);
            tween.Play();
        }
    }

    public void TakeDamage() {
        if (!IsAlive()) return;
        
        _hp--;
        if (_hp > 0) {
            _animatedSprite.Play("Hurt");
        } else {
            _animatedSprite.Play("Death");
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_animatedSprite, "modulate",
                    new Color(1, 1, 1, 0),
                    Utils.GetAnimationTotalLength(_animatedSprite, "Death"))
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Linear);
            tween.Play();
        }
    }

    public bool IsAlive() {
        return _hp > 0;
    }
}