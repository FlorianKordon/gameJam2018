using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NihilismDelusion : Delusion
{
    public GameObject playerCharacter;

    private Material _baseMat;
    private Shader _baseShader;
    private Shader _delusionShader;

    private void Start()
    {
        _baseMat = playerCharacter.GetComponent<Renderer>().material;
        _baseShader = Shader.Find("");
        _delusionShader = Shader.Find("");
    }

    public override void DelusionForecast()
    {
        //Debug.Log("Vibration Forecast");
        Handheld.Vibrate();
    }

    public override void DelusionContent()
    {
        //_rend =
    }

    public override void DelusionCloseDown()
    {
        throw new System.NotImplementedException();
    }
}
