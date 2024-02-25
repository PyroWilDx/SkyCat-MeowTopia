using Godot;
using SkyCatMeowtopia.Scripts;
using System;
using System.Collections.Generic;

public partial class TileMap : Godot.TileMap {
    private static TileMap _tileMap = null;

    public static TileMap GetInstance() {
        return _tileMap;
    }

    public override void _Ready() {
        _tileMap = this;
    }
    
}