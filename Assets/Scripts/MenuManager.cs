using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Text titleText;
    public Transform canvasTF;
    public GameObject redSquare;
    public Transform boardIcon;
    public void OnMenuClick(int code)
    {
        if (code == 0)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (code == 1)
        {
            SceneManager.LoadScene("OptionsScene");
        }
        else if (code == 2)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else if (code == 3)
        {
            for (int i = 0; i < canvasTF.childCount; i++)
            {
                Transform tf = canvasTF.GetChild(i);
                tf.gameObject.SetActive(false);

            }
            boardIcon.gameObject.SetActive(true);
            for (int i = 0; i < boardIcon.childCount; i++)
            {
                Transform tf = boardIcon.GetChild(i);
                tf.gameObject.SetActive(false);

            }
            redSquare.SetActive(true);
        }
        else if (code == 4)
        {
            titleText.text = "15 Puzzle";
            boardIcon.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 45));
        }
    }
}
