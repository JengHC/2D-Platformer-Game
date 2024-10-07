using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LifeDisplayer : MonoBehaviour
{

    public List<GameObject> LifeImages;

    public void SetLives(int life)
    {
        for( int i = 0; i<LifeImages.Count; i++)
        {
            if(i<life)
            {
                LifeImages[i].SetActive(true);
            }
            else 
            {
                LifeImages[i].SetActive(false);
            }
        }
    }
}
