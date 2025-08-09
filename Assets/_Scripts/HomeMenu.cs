using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public void PlayNormal() { SceneManager.LoadScene("Game"); }
    public void PlayPaid() { SceneManager.LoadScene("DummyPayment"); }
}