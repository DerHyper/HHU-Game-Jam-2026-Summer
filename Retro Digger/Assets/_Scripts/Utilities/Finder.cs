using UnityEngine;

/// <summary>
/// Wrapper class for finding specific components.
/// </summary>
public static class Finder
{
    /// <summary>
    /// Tries to find a component. If there is none, creates one.
    /// </summary>
    /// <typeparam name="T">Type of the component</typeparam>
    /// <returns>The found component. Note: If multiple components are found, just one is returned.</returns>
    public static T FindOrCreateObjectOfType<T>() where T : Component
    {
        T foundObject = GameObject.FindObjectOfType<T>();
        if (foundObject == null)
        {
            GameObject go = GameObject.Instantiate(new GameObject());
            return go.AddComponent<T>();
        }
        return foundObject;
    }

    /// <inheritdoc cref="FindOrCreateObjectOfType()"/>
    /// <param name="target">The parent object, who's children will be searched thou at.</param>
    public static T FindOrCreateComponent<T>(GameObject target) where T : Component
    {
        if (!target.TryGetComponent(out T foundComponent))
        {
            return target.AddComponent<T>();
        }
        return foundComponent;
    }

    /// <summary>
    /// Tries to find a GameObject. If there is none, creates one.
    /// </summary>
    /// <param name="tag">
    /// If a GameObject with the given tag exists, this method returns it. Else a new GameObject is instantiated with the name <tag>Object.
    /// Note: Unity does not allow to access the tags directly so this method might throw an exception if an invalid tag is used.
    /// </param>
    /// <returns>The found GameObject. Note: If multiple GOs are found, just one is returned.</returns>
    public static GameObject FindOrCreateGameObjectWithTag(string tag)
    {
        GameObject foundGameObject = GameObject.FindGameObjectWithTag(tag);
        if (foundGameObject == null)
        {
            GameObject newGameObject = GameObject.Instantiate(new GameObject());
            newGameObject.name = tag + "Object";
            newGameObject.tag = tag;
            return newGameObject;
        }
        return foundGameObject;
    }

    /// <inheritdoc cref="FindOrCreateObjectOfType()"/>
    /// <param name="name">
    /// If a GameObject with the given name exists, this method returns it. Else a new GameObject is instantiated with this name.
    /// </param>
    public static GameObject FindOrCreateGameObjectWithName(string name)
    {
        GameObject foundGameObject = GameObject.Find(name);
        if (foundGameObject == null)
        {
            GameObject newGameObject = GameObject.Instantiate(new GameObject());
            newGameObject.name = name;
            return newGameObject;
        }
        return foundGameObject;
    }

    public static GameObject FindObjectWithNameInChildren(string name, GameObject parent)
    {
        Transform child = parent.transform.Find(name);
        if (child != null)
        {
            return child.gameObject;
        }
        return null;
    }

}