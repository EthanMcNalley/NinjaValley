using System.Collections;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public Material shader;
    public float duration = 1f;
    public float shaderPower = 2.41f;
    [SerializeField] private float elapsedTime = 0f;
    private bool isShaderEnabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shader != null)
        {
     
            Debug.Log(shaderPower);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShaderEnabled = !isShaderEnabled;
            toggleShader(shader);
        }
            
    }

    void toggleShader(Material shader)
    {
        if (isShaderEnabled==true)
        {
            StartCoroutine(enableEffect(shader));
        }
        else
        {
            StartCoroutine(disableEffect(shader));
        }
    }

    IEnumerator enableEffect(Material shader)
    {
        elapsedTime = 0f;
        shader.SetFloat("_MaskPower", 10);
        while (elapsedTime <duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float blend = Mathf.Lerp(10, shaderPower, t);
            shader.SetFloat("_MaskPower", blend);
           

            yield return null;
        }
   
        shader.SetFloat("_MaskPower", shaderPower);


    }

    IEnumerator disableEffect(Material shader)
    {

        elapsedTime = 0f;
        shader.SetFloat("_MaskPower", shaderPower);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float blend = Mathf.Lerp(shaderPower, 10, t);
            shader.SetFloat("_MaskPower", blend);
          

            yield return null;
        }
 
        shader.SetFloat("_MaskPower", 10);
        

    }

}
