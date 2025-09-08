using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/New Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public float cooldown;
    public string description;

    public GameObject skillVFX;       // ÆÄÆ¼Å¬ Prefab
    public AudioClip skillSFX;

}
