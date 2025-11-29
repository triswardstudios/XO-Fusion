using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public Button sprite;
    public Sprite[] sprites = new Sprite[2];
    private void Start()
    {
        GameObject audio = GameObject.Find("Audio Source");
        if (audio.GetComponent<AudioSource>().mute == false)
        {
            sprite.GetComponent<Image>().sprite = sprites[1];
        }
        else
        {
            sprite.GetComponent<Image>().sprite = sprites[0];
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SoundImageCheck()
    {
        if (sprite.GetComponent<Image>().sprite == sprites[0])
        {
            sprite.GetComponent<Image>().sprite = sprites[1];
            GameObject audio = GameObject.Find("Audio Source");
            audio.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            GameObject audio = GameObject.Find("Audio Source");
            sprite.GetComponent<Image>().sprite = sprites[0];
            audio.GetComponent<AudioSource>().mute = true;
        }
    }
}