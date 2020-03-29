using UnityEngine;
using System.Collections;

public class BackgroundMove : MonoBehaviour {
    private Transform bgCenter { get; set; }
    private Transform bgHoriz { get; set; }
    private Transform bgVert { get; set; }
    private Transform bgCorner { get; set; }
    private Bounds bounds;
    private float centerWidth;
    private float centerHeight;
    public void Set(string center, string horizontal, string vertical, string corner) {

        bgCenter = GameObject.Find(center).transform;
        bgHoriz = GameObject.Find(horizontal).transform;
        bgVert = GameObject.Find(vertical).transform;
        bgCorner = GameObject.Find(corner).transform;

        bounds = bgCenter.GetComponent<SpriteRenderer>().sprite.bounds;
        centerWidth = bounds.size.x * bgCenter.localScale.x;
        centerHeight = bounds.size.y * bgCenter.localScale.y;
    }
    public void Move(Vector2 location) {
        //Debug.Log ("location=" + location);
        if (location.x - bgCenter.position.x <= -centerWidth || location.x - bgCenter.position.x >= centerWidth) {
            bgCenter.position = new Vector3(bgHoriz.position.x, bgHoriz.position.y, -1);
        }
        if (location.x - bgCenter.position.x <= 0f) {
            bgHoriz.position = new Vector3(bgCenter.position.x - centerWidth, bgHoriz.position.y, -1);
            bgVert.position = new Vector3(bgHoriz.position.x + centerWidth, bgVert.position.y, -1);
            bgCorner.position = new Vector3(bgCenter.position.x - centerWidth, bgCorner.position.y, -1);
        } else {
            bgHoriz.position = new Vector3(bgCenter.position.x + centerWidth, bgHoriz.position.y, -1);
            bgVert.position = new Vector3(bgHoriz.position.x - centerWidth, bgVert.position.y, -1);
            bgCorner.position = new Vector3(bgCenter.position.x + centerWidth, bgCorner.position.y, -1);
        }

        if (location.y - bgCenter.position.y <= -centerHeight || location.y - bgCenter.position.y >= centerHeight) {
            bgCenter.position = new Vector3(bgVert.position.x, bgVert.position.y, -1);
        }
        if (location.y - bgCenter.position.y <= 0f) {
            bgVert.position = new Vector3(bgVert.position.x, bgCenter.position.y - centerHeight, -1);
            bgHoriz.position = new Vector3(bgHoriz.position.x, bgVert.position.y + centerHeight, -1);
            bgCorner.position = new Vector3(bgCorner.position.x, bgCenter.position.y - centerHeight, -1);
        } else {
            bgVert.position = new Vector3(bgVert.position.x, bgCenter.position.y + centerHeight, -1);
            bgHoriz.position = new Vector3(bgHoriz.position.x, bgVert.position.y - centerHeight, -1);
            bgCorner.position = new Vector3(bgCorner.position.x, bgCenter.position.y + centerHeight, -1);
        }
    }
}
