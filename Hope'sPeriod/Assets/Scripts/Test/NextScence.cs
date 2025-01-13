using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScence : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            SceneManager.LoadScene("Battle");
        }   
    }
}
