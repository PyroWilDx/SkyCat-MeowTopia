using Godot;
using SkyCatMeowtopia.Scripts;
using System;
using System.Collections.Generic;

public partial class Tree : StaticBody2D {
    private static QuadTree<Tree> _treeQT = null;

    public static void HitTree(Rect2 hitRect) {
        List<Tree> eList = _treeQT.GetElements(hitRect.GetCenter());
        foreach (Tree tree in eList) {
            if (tree.GetCollisionRect().Intersects(hitRect)) {
                tree.Hurt();
            }
        }
    }

    private AnimatedSprite2D _animatedSprite;
    private Rect2 _colRect;
    private int _hp = 6;

    public override void _Ready() {
        if (_treeQT == null) {
            _treeQT = new QuadTree<Tree>(4, -1024, -1024, 2048, 2048);
        }

        _treeQT.AddElement(this, Position);

        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");

        CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShape");
        _colRect = new Rect2(Position - colShape.Shape.GetRect().Size / 2, colShape.Shape.GetRect().Size);
    }

    public override void _Process(double dt) {
        if (_hp <= 0 && !_animatedSprite.IsPlaying()) QueueFree();
    }

    public override void _ExitTree() {
        _treeQT.RemoveElement(this, Position);
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

    private void Hurt() {
        _hp--;
        if (_hp > 0) _animatedSprite.Play("Hurt");
        else _animatedSprite.Play("Death");
    }
}