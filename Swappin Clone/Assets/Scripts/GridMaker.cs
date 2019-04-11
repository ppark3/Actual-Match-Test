using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMaker : MonoBehaviour
{
    public const int WIDTH = 5;
    public const int HEIGHT = 7;

    float xOffset = WIDTH / 2f - 0.5f;
    float yOffset = HEIGHT / 2f - 0.5f;

    public GameObject[,] tiles;
    public GameObject tilePrefab;
    public GameObject playerPrefab;
    public GameObject player;
    GameObject gridHolder;

    public int score;
    public Text scoreText;

    public bool stop;

    public static float slideLerp = -1;
    public float lerpSpeed = 0.25f;

    public ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[WIDTH, HEIGHT];

        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(-1f, -0.5f, 0);

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (x == 2 && y == 3)
                {
                    player = Instantiate(playerPrefab);
                    player.transform.parent = gridHolder.transform;
                    player.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                    tiles[x, y] = player;
                    player.GetComponent<PlayerScript>().x = x;
                    player.GetComponent<PlayerScript>().y = y;
                    player.GetComponent<PlayerScript>().turnsLeft = 6;
                }
                else
                {
                    GameObject newTile = Instantiate(tilePrefab);
                    newTile.transform.parent = gridHolder.transform;
                    newTile.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                    tiles[x, y] = newTile;

                    TileScript tileScript = newTile.GetComponent<TileScript>();
                    tileScript.SetSprite(Random.Range(0, tileScript.colors.Length));
                }
            }
        }

        while (HasMatch())
        {
            HasMatchBeginning();
        }

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D))) && player.GetComponent<PlayerScript>().x > 0
                && player.GetComponent<PlayerScript>().turnsLeft > 0 && !stop)
        {
            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            GameObject temp = tiles[x - 1, y];
            tiles[x - 1, y] = player;
            tiles[x, y] = temp;

            player.transform.localPosition = new Vector2(WIDTH - (x - 1) - xOffset, HEIGHT - y - yOffset);
            tiles[x,y].transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

            player.SendMessage("MoveRight");
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A))) && player.GetComponent<PlayerScript>().x < 4
                && player.GetComponent<PlayerScript>().turnsLeft > 0 && !stop)
        {
            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            GameObject temp = tiles[x + 1, y];
            tiles[x + 1, y] = player;
            tiles[x, y] = temp;

            player.transform.localPosition = new Vector2(WIDTH - (x + 1) - xOffset, HEIGHT - y - yOffset);
            tiles[x, y].transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

            player.SendMessage("MoveLeft");
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.S))) && player.GetComponent<PlayerScript>().y < 6
                && player.GetComponent<PlayerScript>().turnsLeft > 0 && !stop)
        {
            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            GameObject temp = tiles[x, y + 1];
            tiles[x, y + 1] = player;
            tiles[x, y] = temp;

            player.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - (y + 1) - yOffset);
            tiles[x, y].transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

            player.SendMessage("MoveDown");
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.W))) && player.GetComponent<PlayerScript>().y > 0
                && player.GetComponent<PlayerScript>().turnsLeft > 0 && !stop)
        {
            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            GameObject temp = tiles[x, y - 1];
            tiles[x, y - 1] = player;
            tiles[x, y] = temp;

            player.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - (y - 1) - yOffset);
            tiles[x, y].transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

            player.SendMessage("MoveUp");
        }

        if (slideLerp < 0 && !Repopulate() && HasMatch())
        {
            RemoveMatches();
            player.GetComponent<PlayerScript>().turnsLeft = 6;
        }
        else if (slideLerp >= 0)
        {
            stop = true;
            slideLerp += Time.deltaTime / lerpSpeed;
            if (slideLerp >= 1)
            {
                slideLerp = -1;
            }
        }

        if (slideLerp < 0 && !HasMatch())
        {
            stop = false;
        }
        scoreText.GetComponent<Text>().text = "SCORE:" + score;
    }

    public void HasMatchBeginning()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();

                if (tileScript != null)
                {
                    if (x < WIDTH - 2 && tileScript.isMatch(tiles[x + 1, y], tiles[x + 2, y]))
                    {
                        tileScript.SetSprite(Random.Range(0, tileScript.colors.Length));
                    }
                    if (y < HEIGHT - 2 && tileScript.isMatch(tiles[x, y + 1], tiles[x, y + 2]))
                    {
                        tileScript.SetSprite(Random.Range(0, tileScript.colors.Length));
                    }
                }
            }
        }
    }

    public TileScript HasMatch()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();

                if(tileScript != null)
                {
                    if (x < WIDTH - 2 && tileScript.isMatch(tiles[x + 1, y], tiles[x + 2, y]))
                    {
                        return tileScript;
                    }
                    if (y < HEIGHT - 2 && tileScript.isMatch(tiles[x, y + 1], tiles[x, y + 2]))
                    {
                        return tileScript;
                    }
                }
            }
        }
        return null;
    }

    public void RemoveMatches()
    {
        int scorePoints = 0;
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                if (tileScript != null)
                {
                    if (x < WIDTH - 2 && tileScript.isMatch(tiles[x + 1, y], tiles[x + 2, y]))
                    {
                        Instantiate(particles, tiles[x,y].transform.position, Quaternion.identity);
                        Instantiate(particles, tiles[x + 1, y].transform.position, Quaternion.identity);
                        Instantiate(particles, tiles[x + 2, y].transform.position, Quaternion.identity);

                        scorePoints += 3;

                        Destroy(tiles[x, y]);
                        Destroy(tiles[x + 1, y]);
                        Destroy(tiles[x + 2, y]);
                    }
                    if (y < HEIGHT - 2 && tileScript.isMatch(tiles[x, y + 1], tiles[x, y + 2]))
                    {
                        Instantiate(particles, tiles[x, y].transform.position, Quaternion.identity);
                        Instantiate(particles, tiles[x, y + 1].transform.position, Quaternion.identity);
                        Instantiate(particles, tiles[x, y + 2].transform.position, Quaternion.identity);

                        scorePoints += 3;

                        Destroy(tiles[x, y]);
                        Destroy(tiles[x, y + 1]);
                        Destroy(tiles[x, y + 2]);
                    }
                }
            }
        }
        score += scorePoints;
    }

    public bool Repopulate()
    {
        bool repop = false;

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if(tiles[x,y] == null)
                {
                    repop = true;

                    if (y == 0)
                    {
                        tiles[x, y] = Instantiate(tilePrefab);
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        tileScript.SetSprite(Random.Range(0, tileScript.colors.Length));
                        tiles[x,y].transform.parent = gridHolder.transform;
                        tiles[x,y].transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);
                    }
                    else
                    {
                        slideLerp = 0;
                        tiles[x, y] = tiles[x, y - 1];
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        if (tileScript != null)
                        {
                            tileScript.SetupSlide(new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset));
                        }
                        PlayerScript playerScript = tiles[x, y].GetComponent<PlayerScript>();
                        if (playerScript != null)
                        {
                            playerScript.x = x;
                            playerScript.y = y;
                        }

                        tiles[x, y - 1] = null;
                    }
                }
            }
        }
        return repop;
    }
}
