using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public InputField diffInput;
    public GameObject errorButton;
    public Text cipherDisplayText;
    public Text errorText;
    public int value = 150;
    public int sub = 0;

    private void Start()
    {
        sub = PlayerPrefs.GetInt("sub", 0);
        if (sub >= GameManager.subs.Length) { sub = 0; SetCipher(0); }
        value = PlayerPrefs.GetInt("diff", 150);
        diffInput.text = value.ToString();
        cipherDisplayText.text = SubExamble(sub);

        errorButton.SetActive(false);
    }
    public void SetDifficulty()
    {
        int val = 0;
        bool ft = false;
        string svalue = diffInput.text;
        if (int.TryParse(svalue, out val))
        {
            if (val == -42 || val == 4242)
            {
                ft = true;
                if (val < 0) val = 150;
                ShowError("<b>What do you get when you multiply six by nine?</b>");
                errorButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                GameManager.subs = new string[][]{
                    new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" },
                    new string[] { "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" },
                    new string[] { "α", "β", "γ", "δ", "ε", "ϛ", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ" },
                    new string[] { "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Χ", "Ο" },
                    new string[] { "Α", "Β", "Γ", "Δ", "Ε", "Ϛ", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Χ" },
                    new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" },
                    new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" },
                    new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                    new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" },
                    new string[] { "①", "②", "③", "④", "⑤", "⑥", "⑦", "⑧", "⑨", "⑩", "⑪", "⑫", "⑬", "⑭", "⑮" },
                    new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" },
                    new string[] { "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix", "x", "xi", "xii", "xiii", "xiv", "xv" },
                    new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen" },
                    new string[] { "Y", "O", "U", " ", "W", "I", "N", "!", "V", "I", "C", "T", "O", "R", "Y", },
                    new string[] { "Y", "O", "U", " ", "W", "I", "N", "!", "V", "I", "C", "T", "O", "R", "Y", } };

                sub = 0;
                SetCipher(sub);

            }
            if (val < 0) ShowError("Type a positive number.");
            else if (val > 9999) ShowError("Type a smaller number.");
            else
            {
                if (!ft)
                    HideError();
                if (val == 0) val = 1;
                value = val;
                PlayerPrefs.SetInt("diff", value);
                PlayerPrefs.Save();
            }

        }
        else ShowError("Type a number, then click <b>SET</b>.");
        // Show new val
        diffInput.text = value.ToString();
    }

    public void NextCipher()
    {
        HideError();
        sub = (sub + 1) % (GameManager.subs.Length - 1);
        SetCipher(sub);
    }
    public void SetCipher(int sub)
    {

        cipherDisplayText.text = SubExamble(sub);
        PlayerPrefs.SetInt("sub", sub);
        PlayerPrefs.Save();
    }

    public void ShowError(string message)
    {
        errorText.text = message;
        errorButton.SetActive(true);
    }

    public void HideError()
    {
        errorButton.SetActive(false);
    }

    public string SubExamble(int sub)
    {
        string[] cipher = GameManager.subs[sub];
        return string.Format("{0}, {1}, ..., {2}", cipher[0], cipher[1], cipher[14]);
    }

    public void Return()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
