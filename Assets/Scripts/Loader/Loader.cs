using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader <T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance = null;

    public static T Instance { get => _instance; }

    public void Awake() {
        print("name " + FindObjectOfType<T>().name);
        if (_instance == null) {
            _instance = FindObjectOfType<T>();
        }

        if (_instance != FindObjectOfType<T>()) {
            Destroy(FindObjectOfType<T>());
        }

        DontDestroyOnLoad(FindObjectOfType<T>());
    }
}
