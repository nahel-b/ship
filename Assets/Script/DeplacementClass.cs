using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeplacementClass
{
    public List<Lieu> Lieux;

    public DeplacementClass()
    {
        Lieux = new List<Lieu>();
        StationsName = new List<string>();
    }


    public Lieu Find(string name)
    {
        Lieu a = new Lieu();
        foreach(Lieu l in Lieux)
        {
            if(l.name == name) { a = l; }

        }

        return a;
    }

    public List<string> StationsName;
}
[System.Serializable]

public class Lieu
{
    public string name;
    public Vector3 apparitionPos;
}

[System.Serializable]

public class PersoSave
{
    public List<PersoClass> Perso;
    public PersoClass Find(string name)
    {
        PersoClass a = new PersoClass();
        foreach (PersoClass perso in Perso)
        {
            if (perso.name == name) { a = perso; }

        }

        return a;
    }
}
[System.Serializable]

public class PersoClass
{
    public string name;
    public int Phase;
    public PersoClass()
    {
        name = "null";
        Phase = 0;
    }
}
