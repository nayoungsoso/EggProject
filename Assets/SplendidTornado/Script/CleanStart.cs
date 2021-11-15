using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanStart : MonoBehaviour
{

    public GameObject ParticleObj; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Clean());
    }

    IEnumerator Clean()
    {

        yield return new WaitForSeconds(0.01f);
        ParticleObj.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        ParticleObj.SetActive(true);
    }
    


}
