using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;


public class SaveSystem : MonoBehaviour
{
    [Header("Components")]
    DataHolder dataHolder = new DataHolder();
    public Options options;
    public PlayerController player;
    [Header("States")]
    public bool setPlayerPos = true;

    private void Start()
    {
        Load();
        SetData();
    }

    public void SavePreparations()
    {
        DataCollector();
        SaveGame();
        print(Application.persistentDataPath + "/" + "SaveData" + ".Xml");
    }

    void SaveGame()
    {

        var serializer = new XmlSerializer(typeof(DataHolder));
        var stream = new FileStream(Application.persistentDataPath + "/" + "SaveData" + ".Xml", FileMode.Create);
        serializer.Serialize(stream, dataHolder);
        stream.Close();
    }

    public void Load()
    {
        var serializer = new XmlSerializer(typeof(DataHolder));
        var stream = new FileStream(Application.persistentDataPath + "/" + "SaveData" + ".Xml", FileMode.Open);
        dataHolder = serializer.Deserialize(stream) as DataHolder;
        stream.Close();

    }

    void DataCollector()
    {
        dataHolder.quality = options.qualityDropdown.value;
        dataHolder.res = options.resolutionDropdown.value;
        dataHolder.music = options.musicSlider.value;
        dataHolder.soundEffects = options.soundEffectsSlider.value;
        dataHolder.fullscreen = options.fullscreenToggle.isOn;

        dataHolder.playerXPos = player.transform.position.x;
        dataHolder.playerYPos = player.transform.position.y;

    }

    void SetData()
    {
        if (dataHolder != null)
        {
            if(options != null)
            {
                options.SetQuality(dataHolder.quality);
                options.SetResolution(dataHolder.res);
                options.SetMusic(dataHolder.music);
                options.SetSoundEffects(dataHolder.soundEffects);
                options.SetFullscreen(dataHolder.fullscreen);
            }

            if(player != null && setPlayerPos)
            {
                player.transform.position = new Vector3(dataHolder.playerXPos, dataHolder.playerYPos, player.transform.position.z);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SavePreparations();
    }
}
