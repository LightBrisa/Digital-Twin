using UnityEditor;
using UnityEngine;
using System.Collections;

public class ContrastEvaluation : MonoBehaviour
{
    public class Relation
    {
        public string object1;
        public string object2;
        public float area;
        public Relation(string object1, string object2, float area)
        {
            this.object1 = object1;
            this.object2 = object2;
            this.area = area;
        }
    }

    static public ArrayList relationsInRoom1 = new ArrayList();
    static public ArrayList relationsInRoom2 = new ArrayList();
    static public ArrayList relationsInRoom3 = new ArrayList();
    static public ArrayList relationsInRoom4 = new ArrayList();


    void Start()
    {
        relationsInRoom3.Add(new Relation("floor", "Furniture2", 10f));
        relationsInRoom3.Add(new Relation("floor", "Table2", 1f));
        relationsInRoom3.Add(new Relation("floor", "Couch3", 5f));
        relationsInRoom3.Add(new Relation("floor", "Couch2", 5f));
        relationsInRoom3.Add(new Relation("floor", "Couch1", 15f));
        relationsInRoom3.Add(new Relation("floor", "Furniture1", 10f));
        relationsInRoom3.Add(new Relation("floor", "Chair", 1f));
        relationsInRoom3.Add(new Relation("floor", "Chair1", 1f));
        relationsInRoom3.Add(new Relation("floor", "Chair2", 1f));
        relationsInRoom3.Add(new Relation("floor", "Chair3", 1f));
        relationsInRoom3.Add(new Relation("floor", "Chair4", 1f));
        relationsInRoom3.Add(new Relation("floor", "Chair5", 1f));
        relationsInRoom3.Add(new Relation("floor", "Table1", 1.5f));
        relationsInRoom3.Add(new Relation("ceiling", "wall1", 2f));
        relationsInRoom3.Add(new Relation("ceiling", "wall2", 2f));
        relationsInRoom3.Add(new Relation("ceiling", "wall3", 2f));
        relationsInRoom3.Add(new Relation("ceiling", "wall4", 2f));

        relationsInRoom3.Add(new Relation("wall1", "wall2", 3f));
        relationsInRoom3.Add(new Relation("wall2", "wall3", 3f));
        relationsInRoom3.Add(new Relation("wall3", "wall4", 3f));
        relationsInRoom3.Add(new Relation("wall4", "wall1", 3f));
    }

    static public float Contrast(string room)
    {
        float energy = 0;
        foreach (Relation relation in relationsInRoom3)
        {
            GameObject object1 = GameObject.Find(relation.object1);
            GameObject object2 = GameObject.Find(relation.object2);
            Color color1 = object1.GetComponent<MeshRenderer>().material.color;
            Color color2 = object2.GetComponent<MeshRenderer>().material.color;
            float Lm = ColorSystem.RGBToLab(color1).L;
            float Ln = ColorSystem.RGBToLab(color2).L;
            energy += (1 - Mathf.Pow(((Lm - Ln) / 100), 2)) * relation.area;
        }
        return energy;
    }
}