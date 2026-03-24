using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public GameObject main,settings;
    public Slider soundEffects, musics;
    cam mycam;
    private void Start()
    {
        mycam = FindAnyObjectByType<cam>();
        soundEffects.value = myMath.LoadFloat("Sound-Effects-Value", 0.5f);
        musics.value = myMath.LoadFloat("Music-Value",0.5f);
    }
    public void play()
    {
        
        main.SetActive(false);
        settings.SetActive(false);
        GetComponent<Animator>().SetTrigger("fall");
        myMath.waitAndStart(2, () => 
        {
            myMath.SaveFloat("camRotX", mycam.transform.localEulerAngles.x);
            myMath.SaveFloat("camRotY", mycam.transform.localEulerAngles.y);
            SceneManager.LoadScene("Game"); 
        }
        );
        
        Save();
    }
    public void quit()
    {
        main.SetActive(false);
        settings.SetActive(false);
        GetComponent<Animator>().SetTrigger("fall");
        myMath.waitAndStart(2, () =>
        {
            Application.Quit();
        }
        );
        Save();
    }

    public void settingsOpen(bool onOf)
    {
        main.SetActive(!onOf);
        settings.SetActive(onOf);
        Save();
    }
    public void Save()
    {
        myMath.SaveFloat("Music-Value", musics.value);
        myMath.SaveFloat("Sound-Effects-Value", soundEffects.value);
    }

}

