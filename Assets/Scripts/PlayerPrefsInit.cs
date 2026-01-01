using UnityEngine;

public class PlayerPrefsInit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetString("Game Mode", "Normal");
        PlayerPrefs.SetString("Opponent Type", "Bot");
        PlayerPrefs.Save();
        if (!PlayerPrefs.HasKey("MusicVolume")){ 
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }
        if (!PlayerPrefs.HasKey("SFXVolume")){
            PlayerPrefs.SetFloat("SFXVolume", 0.2f);
        }
        if (!PlayerPrefs.HasKey("BGMVolume")){
            PlayerPrefs.SetFloat("BGMVolume", 0.2f);
        }
        if (!PlayerPrefs.HasKey("FlipTacToe")){
            PlayerPrefs.SetInt("FlipTacToe", 0);
        }
        if (!PlayerPrefs.HasKey("TicTacToe"))
        {
            PlayerPrefs.SetInt("TicTacToe", 0);
        }
        if (!PlayerPrefs.HasKey("MineTacToe"))
        {
            PlayerPrefs.SetInt("MineTacToe", 0);
        }
        if(!PlayerPrefs.HasKey("BGM Volume"))
        {
            PlayerPrefs.SetFloat("BGM Volume", 0.5f);
        }
        if(!PlayerPrefs.HasKey("SFX Volume"))
        {
            PlayerPrefs.SetFloat("SFX Volume", 0.5f);
        }
        if(!PlayerPrefs.HasKey("Mute"))
        {
            PlayerPrefs.SetFloat("Mute", 1f);
        }
    }

}
