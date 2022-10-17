using System.Text;
using TMPro;
using UnityEngine;
using Zero.Demo.Model.Data;

public class Name : MonoBehaviour, IDataHandler<NameData>
{
    public TextMeshPro Label;

    public unsafe void Process(ref NameData data)
    {
        fixed (byte* buffer = data.Name)
        {
            var name = Encoding.ASCII.GetString(buffer, 10);
            Label.text = name;
        }
    }

    private void OnEnable()
    {
        Label.text = string.Empty;
    }
}
