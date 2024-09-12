using Godot;

namespace SkyCatMeowTopia.Scripts;

public abstract class Utils {
    private Utils() { }

    public enum Direction {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    private static readonly Vector2I[] DirectionVector2 = new[] {
        Vector2I.Zero,
        Vector2I.Up,
        Vector2I.Down,
        Vector2I.Left,
        Vector2I.Right
    };

    public static Vector2I GetDirectionVector2(Direction direction) {
        return DirectionVector2[(int)direction];
    }

    public static double GetAnimationTotalLength(AnimatedSprite2D animatedSprite, string frameName) {
        double totalLength = 0;
        double animSpeed = 5.0 * animatedSprite.GetPlayingSpeed();
        int n = animatedSprite.SpriteFrames.GetFrameCount(frameName);
        for (int i = 0; i < n; i++) {
            totalLength += animatedSprite.SpriteFrames.GetFrameDuration(frameName, i) / animSpeed;
        }
        return totalLength;
    }
}