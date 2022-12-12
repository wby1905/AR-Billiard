using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private IMixedRealitySceneSystem _sceneSystem;
    // Start is called before the first frame update
    void Start()
    {
        _sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        _sceneSystem.Initialize();

    }

    void Update()
    {
        if (!_sceneSystem.IsContentLoaded("ARPoolGame"))
        {
            _sceneSystem.LoadContent("ARPoolGame", LoadSceneMode.Single);
        }
    }

}
