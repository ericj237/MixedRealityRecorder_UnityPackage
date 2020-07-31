using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusWaveAction : MonoBehaviour, IAction
{
    #region Inspector configurable variables
    public float frequency = 1f;
    public float amplitude = 1f;
    #endregion

    #region Private variable declarations
    private TileScript ts;
    private float x;
    #endregion

    #region Unity methods (Start, Update, ...)
    void Awake()
    {
        // Get a reference to the TileScript to avoid frequent GetComponent calls
        ts = transform.GetComponent<TileScript>();
    }
    #endregion

    void Start()
    {
        x = ts.x;
        frequency = Mathf.Lerp(0, 1, frequency);
    }

    public float GetPercentage()
    {
        // Sinusfunction: y=a*sin(b(x+c))+d TODO: alter the function so u can adjust amplitude, frequency, y-axis intercept
        float result = amplitude*Mathf.Sin(x*frequency);
        result = Mathf.InverseLerp(-1, 1, result);
        x += Time.deltaTime;
        return result;
    }
}
