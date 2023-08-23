using UnityEngine;

public class MyUtils
{
    //hideously slow as it iterates all objects, so don't overuse!
    public static GameObject FindInChildrenIncludingInactive(GameObject go, string name)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).gameObject.name == name) return go.transform.GetChild(i).gameObject;
            GameObject found = FindInChildrenIncludingInactive(go.transform.GetChild(i).gameObject, name);
            if (found != null) return found;
        }

        return null;  //couldn't find crap
    }
}
