using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject gridMaker;

    public int turnsLeft;

    public GameObject text;

    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (turnsLeft == 6)
        {
            text.GetComponent<Text>().text = turnsLeft + "";
        }
        if (turnsLeft == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void MoveRight()
    {
        x--;
        turnsLeft--;
        text.GetComponent<Text>().text = turnsLeft + "";
    }

    void MoveLeft()
    {
        x++;
        turnsLeft--;
        text.GetComponent<Text>().text = turnsLeft + "";
    }

    void MoveUp()
    {
        y--;
        turnsLeft--;
        text.GetComponent<Text>().text = turnsLeft + "";
    }

    void MoveDown()
    {
        y++;
        turnsLeft--;
        text.GetComponent<Text>().text = turnsLeft + "";
    }
}
