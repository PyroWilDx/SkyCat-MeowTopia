using Godot;
using System.Collections.Generic;

namespace SkyCatMeowtopia.Scripts;

public class QuadTree<T> {
    private readonly Rect2 _r;
    
    private readonly bool _isLeaf;
    private readonly QuadTree<T> _upLeft;
    private readonly QuadTree<T> _upRight;
    private readonly QuadTree<T> _downLeft;
    private readonly QuadTree<T> _downRight;

    private readonly List<T> _eList;
    
    public QuadTree(int d, float x, float y, float w, float h) {
        _r = new Rect2(x, y, w, h);
        
        _isLeaf = d == 0;
        if (_isLeaf) {
            _upLeft = null;
            _upRight = null;
            _downLeft = null;
            _downRight = null;
            _eList = new List<T>();
        } else {
            int newDepth = d - 1;
            float halfW = w / 2;
            float halfH = h / 2;
            float midX = x + halfW;
            float midY = y + halfH;
            _upLeft = new QuadTree<T>(newDepth, x, y, halfW, halfH);
            _upRight = new QuadTree<T>(newDepth, x + midX, y, halfW, halfH);
            _downLeft = new QuadTree<T>(newDepth, x, y + midY, halfW, halfH);
            _downRight = new QuadTree<T>(newDepth, x + midX, y + midY, halfW, halfH);
            _eList = null;
        }
    }

    private bool HasPoint(Vector2 p) {
        return _r.HasPoint(p);
    }

    public void AddElement(T e, Vector2 atPosition) {
        if (_isLeaf) {
            _eList.Add(e);
        } else {
            if (_upLeft.HasPoint(atPosition)) _upLeft.AddElement(e, atPosition);
            else if (_upRight.HasPoint(atPosition)) _upRight.AddElement(e, atPosition);
            else if (_downLeft.HasPoint(atPosition)) _downLeft.AddElement(e, atPosition);
            else _downRight.AddElement(e, atPosition);
        }
    }

    public void RemoveElement(T e, Vector2 atPosition) {
        if (_isLeaf) {
            _eList.Remove(e);
        } else {
            if (_upLeft.HasPoint(atPosition)) _upLeft.RemoveElement(e, atPosition);
            else if (_upRight.HasPoint(atPosition)) _upRight.RemoveElement(e, atPosition);
            else if (_downLeft.HasPoint(atPosition)) _downLeft.RemoveElement(e, atPosition);
            else _downRight.RemoveElement(e, atPosition);
        }
    }

    public List<T> GetElements(Vector2 atPosition) {
        if (_isLeaf) {
            return _eList;
        }
        if (_upLeft.HasPoint(atPosition)) return _upLeft.GetElements(atPosition);
        if (_upRight.HasPoint(atPosition)) return _upRight.GetElements(atPosition);
        if (_downLeft.HasPoint(atPosition)) return _downLeft.GetElements(atPosition);
        return _downRight.GetElements(atPosition);
    }
    
}
