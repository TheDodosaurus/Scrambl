using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static string[][] subs = {
    new string[]{ "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" },
    new string[]{ "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" },
    new string[]{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" },
    new string[]{ "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV"},
    new string[]{ "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen"},
    new string[]{ "Y", "O", "U", " ", "W", "I", "N", "!", "V", "I", "C", "T", "O", "R", "Y", } };
    public GameObject tilePrefab;
    public RectTransform tilesParent;
    public int diff = 150;
    public bool active = false;
    public int sub = 1;

    private int[] board;
    private TileScript[] tiles;
    private bool hasWonSoFar = false;
    private IEnumerator scramble;

    private void Start()
    {
        gm = this;

        diff = PlayerPrefs.GetInt("diff", 150);
        sub = PlayerPrefs.GetInt("sub", 0);
        if (sub >= subs.Length) sub = 0;

        active = false;
        hasWonSoFar = false;
        board = new int[16];
        tiles = new TileScript[15];
        for (int i = 0; i < 15; i++)
        {
            board[i] = i;
            GameObject tgo = Instantiate(tilePrefab, tilesParent.transform);
            tiles[i] = tgo.GetComponent<TileScript>();
            tiles[i].SetNumber(i, sub);
            tiles[i].rt.anchoredPosition = tiles[i].target = tiles[i].origin = IndexToPos(i);
        }
        board[15] = -1;

        /*int n = 16;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            int t = board[n];
            board[n] = board[k];
            board[k] = t;
        }*/

        scramble = BuildBoard();
        StartCoroutine(scramble);
    }
    IEnumerator BuildBoard()
    {
        // Assumes empty tile is in last spot
        yield return new WaitForSeconds(.5f);
        int i16 = 15;
        List<int[]> options;
        int[] option;
        int n, x, y, newi;
        int last = -1;
        for (int step = 0; step < diff; step++)
        {
            options = new List<int[]>();
            n = 0;
            x = i16 % 4;
            y = i16 / 4;
            if (x >= 1 && last != 1) { options.Add(new int[] { i16 - 1, 0 }); n++; }
            if (x <= 2 && last != 0) { options.Add(new int[] { i16 + 1, 1 }); n++; }
            if (y >= 1 && last != 3) { options.Add(new int[] { i16 - 4, 2 }); n++; }
            if (y <= 2 && last != 2) { options.Add(new int[] { i16 + 4, 3 }); n++; }
            option = options[Random.Range(0, n)];
            newi = option[0];
            last = option[1];
            tiles[board[newi]].SmoothTo(IndexToPos(i16));
            yield return new WaitForSeconds(.03f);
            board[i16] = board[newi];
            board[newi] = -1;
            i16 = newi;

        }
        active = true;
    }

    public Vector2 IndexToPos(int x)
    {
        return new Vector2((x % 4) * 80 + 35, 275 - (x / 4) * 80);
    }
    public int PosToIndex(Vector2 pos)
    {

        if (pos.x % 80f <= 70f && pos.y % 80f <= 70f && pos.x / 80f >= 0 && pos.x / 80f < 4 && pos.y / 80f >= 0 && pos.y / 80f < 4)
        {
            return Mathf.FloorToInt(pos.x / 80f) + 4 * (3 - Mathf.FloorToInt(pos.y / 80f));
        }
        return -1;
    }

    private void Update()
    {
        if (active && Input.touchCount >= 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            Vector2 point;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(tilesParent, Input.touches[0].position, null, out point))
            {
                int x = PosToIndex(point);
                if (x != -1)
                {
                    int gd = GapDist(x);
                    if (gd != 0)
                    {
                        tiles[board[x]].SmoothTo(IndexToPos(x + gd));
                        board[x + gd] = board[x];
                        board[x] = -1;

                        if (HasWon()) Win();
                    }
                }
            }
        }
    }

    private int GapDist(int i)
    {
        int x = i % 4;
        int y = i / 4;
        if (x >= 1 && board[i - 1] == -1) return -1;
        if (x <= 2 && board[i + 1] == -1) return +1;
        if (y >= 1 && board[i - 4] == -1) return -4;
        if (y <= 2 && board[i + 4] == -1) return +4;
        return 0;
    }

    private bool HasWon()
    {
        if (hasWonSoFar) return false;
        for (int i = 0; i < 15; i++)
        {
            if (board[i] != i) return false;
        }
        return board[15] == -1;
    }

    private void Win()
    {
        hasWonSoFar = true;
        for (int i = 0; i < 15; i++)
        {
            tiles[i].SetNumber(i, GameManager.subs.Length - 1);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void Again()
    {
        StopCoroutine(scramble);
        for (int i = 0; i < 15; i++)
        {
            Destroy(tiles[i].gameObject);
        }
        Start();
    }
}
