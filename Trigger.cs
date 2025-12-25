using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Trigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _director.Play();
        }
    }
}
