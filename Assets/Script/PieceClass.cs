using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PieceClass
{
    public string nom;
    public Vector3 position;
    public Vector3 eulerAngle;
    public string description;
    public int niveau;
    public bool dependant;
    public bool socle;
    public float rotFrame;
    public AttchableSide attchableSide;
    public Vector2 velocity;
    public float vie;
    

    public PieceClass() { }

    public PieceClass(Vector3 position,Vector3 eulerAngle, string nom,  string description, int niveau,bool dependant,bool socle,float rotFrame,AttchableSide attchableSide,float vie)
    {
        this.nom = nom;
        this.eulerAngle = eulerAngle;
        this.position = position;
        this.description = description;
        this.niveau = niveau;
        this.attchableSide = attchableSide;
        this.dependant = dependant;
        this.socle = socle;
        this.rotFrame = rotFrame;
        this.vie = vie;
    }
    //public PieceClass(Vector3 position, Vector3 eulerAngle, string nom, string description, int niveau, bool attachable, bool dependant)
    //{
    //    this.nom = nom;
    //    this.eulerAngle = eulerAngle;
    //    this.position = position;
    //    this.description = description;
    //    this.niveau = niveau;
    //    this.attachable = attachable;
    //    this.dependant = dependant;
    //}


}

[System.Serializable]
public class Assemblage
{
    public List<PieceClass> assemblage;
    public string nom;
    public bool debloque;

    public Assemblage() { assemblage = new List<PieceClass>(); }

    public void Add(PieceClass p)
    {
        this.assemblage.Add(p);
        
    }
    //public Add(Vector3 position, Vector3 eulerAngle, string nom, GameObject obj, string description, bool deck)
    //{
    //    assemblage.Add(new PieceClass(position,eulerAngle,nom,obj,description,deck));
    //    return assemblage;
    //}

}
[System.Serializable]
public class PieceObj //pour PIeceObjList
{
    public string name;
    public GameObject obj;

    public  PieceObj() { }
}
[System.Serializable]
public class PieceObjList //pour Liste de toutes les pieces
{
    public List<PieceObj> Liste;

    public PieceObjList() { Liste = new List<PieceObj>(); }

    public GameObject Find(string name)
    {
        GameObject obj = null;

        foreach (PieceObj p in Liste)
        {
            if(p.name == name)
            {
               obj = p.obj;
            }
        }

        return obj;
    }

}
[System.Serializable]
public class VaisseauClass
{
    public string Deck;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Assemblage pieces;
    public Vector2 velocity;
    public VaisseauClass() { pieces = new Assemblage(); }
}
[System.Serializable]
public class DeckList
{
    public List<Assemblage> Liste;
    
    public DeckList() { Liste = new List<Assemblage>(); }

    public Assemblage Find(string name)
    {
        Assemblage obj = null;

        foreach (Assemblage p in Liste)
        {
            if (p.nom == name)
            {
                obj = p;
            }
        }

        return obj;
    }

}
[System.Serializable]
public class Grandeur
{
    public float fuel;
    public float fuelMax;
    public float health;
    public float healthMax;
    public int coin;

    public Grandeur() { }
    public Grandeur(float fuel, float fuelMax, float health, float healthMax, int coin)
    {
        this.fuel = fuel;
        this.fuelMax = fuelMax;
        this.health = health;
        this.healthMax = healthMax;
        this.coin = coin;

    }

}
[System.Serializable]
public class ObjetList
{
    public List<Objet> Objets;
    
    public Objet Find(string name)
    {
        Objet obj = new Objet();
        obj.MaxStack = 0;
        foreach (Objet p in Objets)
        {
            if (p.name == name)
            {
                obj = p;
            }
        }

        return obj;
    }
}
[System.Serializable]
public class Objet
{
    public string name;
    public GameObject obj;
    public int MaxStack;
}


[System.Serializable]
public class ItemClass
{
    public List<string> Objets;
    public List<int> stacks; 
    public List<PieceClass> Pieces;

    public ItemClass() { Pieces = new List<PieceClass>();  Objets = new List<string>(); }

    public bool PutInventory(string nameObj)
    {
        int i = 0;
        bool rangee = false;
        foreach(string name in this.Objets)
        {
            if (!rangee)
            {
                if (name == nameObj && stacks[i] < GameObject.Find("Liste").GetComponent<Liste>().ObjetList.Find(nameObj).MaxStack) { rangee = true; stacks[i]++; }
            }
                i++;
        }
        if (!rangee && this.Count() < this.maxItem())
        {
            stacks.Add(0);
            stacks[i]++;
            this.Objets.Add(nameObj);

            rangee = true;
        }
        return rangee;
    }
    public bool PutInventory(PieceClass p)
    {
        int i = 0;
        bool rangee = false;
        
        if (this.Count() < this.maxItem())
        {
            this.Pieces.Add(p);
            rangee = true;
        }
        return rangee;
    }
    public void Add(PieceClass Pieces) { this.Pieces.Add(Pieces); }
    bool sup = false;
    public bool Contains(string colis)
    {
        bool a = false;
        for (int i = 0; i < Objets.Count; i++)
        {
            if (colis == Objets[i] && stacks[i]>0)
            {
                a = true;
            }
        }



                return a;
    }
    public void Remove(string colis)
    {
        for (int i = 0; i < Objets.Count; i++)
        {
            if (colis == Objets[i] && !sup)
            {
                
                sup = true;
                stacks[i]--;
                if (stacks[i] == 0)
                {
                    stacks.RemoveAt(i);
                    Objets.RemoveAt(i);
                }
            }

        }

    }

    public void Remove(PieceClass Pieces) { for (int i = 0; i < this.Pieces.Count; i++) { if (Pieces == this.Pieces[i]) { this.Pieces.RemoveAt(i); i = this.Pieces.Count; } } }
    public void RemovePieceAt(int i) { this.Pieces.RemoveAt(i); }

    public int Count()
    {
        int c = Objets.Count + Pieces.Count;
        return c; 
    }
    public int maxItem()
    {
        int c = 0;
        if (SceneManager.GetActiveScene().buildIndex <= 1)
        {
            if (GameObject.Find("Vaisseau") != null)
            {
                foreach (Transform piece in GameObject.Find("Vaisseau").transform)
                {
                    if (piece.GetComponent<Piece>() != null) { c = c + piece.GetComponent<Piece>().stockageListe[piece.GetComponent<Piece>().niveau]; }
                    else if (piece.GetComponent<BuildPiece>() != null) { c = c + piece.GetComponent<BuildPiece>().objPrefab.GetComponent<Piece>().stockageListe[piece.GetComponent<BuildPiece>().niveau]; }
                }
                foreach (Transform piece in GameObject.Find("Vaisseau").transform.GetChild(0))
                {
                    if (piece.GetComponent<Piece>() != null) { c = c + piece.GetComponent<Piece>().stockageListe[piece.GetComponent<Piece>().niveau]; }
                    else if (piece.GetComponent<BuildPiece>() != null) { c = c + piece.GetComponent<BuildPiece>().objPrefab.GetComponent<Piece>().stockageListe[piece.GetComponent<BuildPiece>().niveau]; }
                }
            }
        }
        else { c = PlayerPrefs.GetInt("MaxItem"); }


        return c;
    }



}
//["livraison","objet","depart","arrivée","recompense"]
// ou ["attaque","objet","vaisseau","arrivée","recompense"]
[System.Serializable]
public class metier
{
    public string type;
    public string obj;
    public string depart;
    public string arrive;
    public int recompense;
    public bool recuperer;
    public metier() { }
    public metier(string type,string obj,string depart,string arrive, int recompense)
    {
        this.type = type;
        this.obj = obj;
        this.depart = depart;
        this.arrive = arrive;
        this.recompense = recompense;
        recuperer = false;
    }
    public metier RandomMetier()
    {
        metier m = new metier();
        string[] livraison = { "Super Calculator", "la mère de Louis", "Supersonic Reactor" };
        string[] attaque = { "Black Box", "Module x-t8cv" };
        m.type = "livraison";
        m.obj = livraison[Random.Range(0, livraison.Length)];
        List<GameObject> l = Camera.main.GetComponent<Principal>().Stations;
        GameObject a = l[Random.Range(0, l.Count)];
        GameObject b = l[Random.Range(0, l.Count)];
        while (b == a) { b = l[Random.Range(0, l.Count)]; }
        m.depart = a.name; m.arrive = b.name;
        m.recompense = Random.Range(0, 300);
        m.recuperer = false;
        return m;
    }

}
[System.Serializable]
public class StationClass
{
    public string name;
    public Vector3 pos;
    public int prefab;
    //public string interiorType;
    public List<metier> jobs;
    public int index;
    public StationClass() { jobs = new List<metier>(); }
    public StationClass(string name, Vector3 pos, int prefab, List<metier> jobs,int index)
    {
        this.name = name;
        this.pos = pos;
        this.prefab = prefab;
        
        //this.interiorType = interiorType;
        this.jobs = jobs;
        this.index = index;

    }

}
[System.Serializable]
public class ListsaveObj
{
    //public List<StationClass> stations;
    public string currentSpot;
    public List<FlecheClass> fleches;
    public ItemClass Items;
    public ListsaveObj() {
        //stations = new List<StationClass>();
        fleches = new List<FlecheClass>(); }


}

[System.Serializable]
public class AttchableSide
{
    public bool haut;
    
    public bool droite;
    public bool bas;
    public bool gauche;

    public List<bool> getList()
    {
        List<bool> a = new List<bool> { haut, droite, bas, gauche, haut, droite, bas, gauche, haut, droite, bas, gauche, haut, droite, bas, gauche, haut, droite, bas, gauche };

        return a;
    }
}
[System.Serializable]
public class FlecheClass
{
    public Color color;
    public string targetName;
    public bool showWayPoint;
    public FlecheClass(Color color,string targetName, bool showWayPoint)
    {
        this.color = color;
        this.targetName = targetName;
        this.showWayPoint = showWayPoint;
    }
}
[System.Serializable]
public class StringObjClass
{
    public string name;
    public float chance;

}

[System.Serializable]
public class RandomListe
{
    public List<StringObjClass> objects;
    public string RandomString()
    {
        float addition = 0;
        foreach(StringObjClass st in objects)
        {
            addition = addition + st.chance;
        }
        float hasard = Random.Range(0,addition);
        float avance = objects[0].chance;
        int index = 0;
        while(avance <= addition)
        {
            if(hasard <= avance) { return objects[index].name; }
            else { index++; avance = avance + objects[index].chance; }
        }
        return "";
    }
    public float RandomFloat()
    {
        float addition = 0;
        foreach (StringObjClass st in objects)
        {
            addition = addition + st.chance;
        }
        float hasard = Random.Range(0, addition);
        float avance = objects[0].chance;
        int index = 0;
        while (avance <= addition)
        {
            if (hasard <= avance) { return float.Parse( objects[index].name); }
            else { index++; avance = avance + objects[index].chance; }
        }
        return -1;
    }
}

[System.Serializable]
public class ColorList
{
    public List<ColorClass> colors;
    public ColorClass RandomColor()
    {
        return colors[Random.Range(0,colors.Count)];
    }
    public ColorList() { colors = new List<ColorClass>(); }
}

[System.Serializable]
public class ColorClass
{
    public string name;
    public Color color;
    public bool Debloque;
}