using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCanvas : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Makes this GameObject persistent across scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code here, if necessary
    }

    // Update is called once per frame
    void Update()
    {
        // Regular update code here, if necessary
    }
}
