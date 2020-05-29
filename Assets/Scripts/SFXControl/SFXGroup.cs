using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXGroup : MonoBehaviour
{
 
    /// <summary>
    /// Designers put resources into this list
    /// </summary>
    [SerializeField]
    private List<SFXGroupItem> _SFXGroupItems;

    /// <summary>
    /// dump data into a dictionary,then delete _SFXGroup List
    /// </summary>
    private Dictionary<string, AudioSource> _SFXs;

    private void Awake()
    {
        _SFXs = new Dictionary<string, AudioSource>();
        foreach (var s in _SFXGroupItems)
        {
            if (s != null)
            {
                _SFXs.Add(s.name, s.audioSource);
            }
        }
        _SFXGroupItems.Clear();
    }

    /// <summary>
    /// make sure only call funtions on it
    /// </summary>
    /// <returns></returns>
    public AudioSource GetAudioSource()
    {
        return _SFXs[name];
    }

    public void PlaySFX(string name)
    {
        _SFXs[name].Play();
    }
    public void StopSFX(string name)
    {
        _SFXs[name].Stop();
    }
 
}
