using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Temp : MonoBehaviour
{
    List<List<GameObject>> gameObjects = new List<List<GameObject>>();


    public class Object
    {
       public Material material1;
       public Material material2;
       public Renderer renderer;
        public Object (Material material1, Renderer renderer)
        {
            this.material1 = material1;
            this.renderer = renderer;
        }

    }
    List<Object> objects;
    void Start()
    {

        gameObjects.Add(new List<GameObject>());
        gameObjects[0].Add(gameObject);
        GameObject temp2 = gameObjects[0][1];


        List<Renderer> temp = gameObject.GetComponentsInChildren<Renderer>().ToList();

        foreach (Renderer r in temp)
        {
            objects.Add(new Object(r.material, r));
        }

        //rendererList = new List<Renderer>();
        //rendererList = temp.GetComponentsInChildren<Renderer>().ToList();
        //rendererList.ForEach(renderer => materials.Add(renderer.material));
    }

}
