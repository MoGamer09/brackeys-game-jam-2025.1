using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IndicatorManager : MonoBehaviour
{
    public float max = 0.7f;
    public float min = 0.1f;
    public float speed = 2f;
    public float borderWidth = 0.2f;
    public Color color = new Color(1,0.85f,00);
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create a new MaterialPropertyBlock
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        // Set a random color in the MaterialPropertyBlock
        propertyBlock.SetColor("_Color", color);
        propertyBlock.SetFloat("_Max", max);
        propertyBlock.SetFloat("_Min", min);
        propertyBlock.SetFloat("_borderWidth", borderWidth);
        propertyBlock.SetFloat("_speed", speed);
        

        // Apply the MaterialPropertyBlock to the GameObject
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(propertyBlock);
    }

    public void ShowIndicator()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void HideIndicator()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
