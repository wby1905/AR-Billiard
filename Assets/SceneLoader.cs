using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>().LoadContent("ARPoolGame", LoadSceneMode.Single);

    }

}
