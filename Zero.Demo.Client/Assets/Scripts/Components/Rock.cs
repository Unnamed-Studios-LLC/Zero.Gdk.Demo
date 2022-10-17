using UnityEngine;
using Zero.Demo.Model.Data;

public class Rock : MonoBehaviour, IDataHandler<RockData>
{
    public SpriteRenderer SpriteRenderer;

    public void Process(ref RockData data)
    {
        SpriteRenderer.sprite = RockLibrary.GetRock(data.Index);
    }
}
