using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public INavigationDAO navigationDAO;
    // Start is called before the first frame update
    void Start()
    {
        navigationDAO=GetComponent<INavigationDAO>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
