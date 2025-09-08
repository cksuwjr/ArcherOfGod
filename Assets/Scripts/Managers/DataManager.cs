using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private List<SkillData> datas;

    [SerializeField] private List<AudioClip> audioClips;

    public SkillData GetSkillDataById(int id)
    {
        return datas[id];
    }

    public AudioClip GetAudioDataById(int id)
    {
        return audioClips[id];
    }
}
