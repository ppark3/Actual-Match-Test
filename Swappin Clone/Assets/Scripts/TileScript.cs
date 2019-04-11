using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int type;
    public Color[] colors;
    private bool inSlide = false;

    public Vector3 startPosition;
    public Vector3 destPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (inSlide)
        {
            if(GridMaker.slideLerp < 0)
            {
                transform.localPosition = destPosition;
                inSlide = false;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPosition, destPosition, GridMaker.slideLerp);
            }
        }
    }

    public void SetSprite(int rand)
    {
        type = rand;
        GetComponent<SpriteRenderer>().color = colors[type];
    }

    public bool isMatch(GameObject gameObject1, GameObject gameObject2)
    {
        TileScript ts1 = gameObject1.GetComponent<TileScript>();
        TileScript ts2 = gameObject2.GetComponent<TileScript>();
        return ts1 != null && ts2 != null && type == ts1.type && type == ts2.type;
    }


    public void SetupSlide(Vector2 newDestPost)
    {
        inSlide = true;
        startPosition = transform.localPosition;
        destPosition = newDestPost;
    }
}
