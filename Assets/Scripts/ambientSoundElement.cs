using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class helicopterBehavior : MonoBehaviour
{
    public string audioName;

    // Start is called before the first frame update
    async void Start()
    {
        audioManager.instance.PlayAudio(audioName, false, transform.position, transform, false);
    }
}
