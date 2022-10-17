using UnityEngine;
using Zero.Demo.Model.Data;

public class Wood : MonoBehaviour, IDataHandler<WoodData>
{
    public SpriteRenderer SpriteRenderer;

    public void Process(ref WoodData data)
    {
        SpriteRenderer.sprite = WoodLibrary.GetWood(data.Index);
    }
}