using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    public static GameOver instance;
    int id;
    Text t;
    void Start () {
        instance = this;
        gameObject.SetActive(false);
        t = GetComponent<Text>();
    }

	void Update () {
        if (Input.GetButtonDown(id + "Shoot"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerMovement.num = 1;
        }
    }

    public void PlayerWin(int p)
    {
        gameObject.SetActive(true);
        id = p;
        t.text = t.text.Replace("{}", "" + id);
    }
}
