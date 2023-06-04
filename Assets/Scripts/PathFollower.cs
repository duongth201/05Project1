using UnityEngine;

public class PathFollower : MonoBehaviour
{
    /**
     * Path Type :
     *  1: Ground in map for solider
     *  2: Air for plane (map2, map3)
     */
    public enum PathType { Ground, Air};


    // Property for Path Follower
    public PathType Type;
    public float Speed; 
    public float OffsetAmount;  

    private Vector2 PositionOffset;
    private Path path;
    private int segmentIndex;   // index in map

    private Vector2 A, B, C, D;
    private Vector2 v1, v2, v3;
    private float t;


}
