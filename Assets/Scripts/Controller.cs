using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Otrill newOtril = new Otrill();
        newOtril.DisplayStats();

        Morya newMorya = new Morya();
        newMorya.DisplayStats();

        Udara newUdara = new Udara();
        newUdara.DisplayStats();
    }
}
