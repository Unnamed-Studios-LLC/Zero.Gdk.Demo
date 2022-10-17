using UnityEngine;
using Zero.Demo.Model.Data;

public class Character : MonoBehaviour, IDataHandler<CharacterData>, IDataHandler<ShipData>
{
    private int _toIndex;
    private float _pauseDuration;
    private ShipDefinition _shipDefinition;

    public Transform CharacterTransform;
    public SpriteRenderer Hat;
    public SpriteRenderer Head;
    public SpriteRenderer Legs;
    public float MoveSpeed = 0.5f;

    public void Process(ref CharacterData data)
    {
        Hat.sprite = CharacterLibrary.GetHat(data.HatIndex);
        Head.color = CharacterLibrary.GetHeadColor(data.HeadIndex);
        Legs.color = CharacterLibrary.GetLegsColor(data.LegsIndex);
    }

    public void Process(ref ShipData data)
    {
        _shipDefinition = ShipLibrary.Get(data.ShipType);
        if ((_shipDefinition.CharacterCoordinates?.Length ?? 0) > 0)
        {
            var start = _shipDefinition.GetCoordinates(Random.Range(0, _shipDefinition.CharacterCoordinates.Length));
            SetPosition(start);
            NewTarget();
        }
    }

    private void NewTarget()
    {
        _pauseDuration = Random.Range(2f, 5f);

        var from = (Vector2)CharacterTransform.localPosition;
        do
        {
            _toIndex = Random.Range(0, _shipDefinition.CharacterCoordinates.Length);
        } while (_shipDefinition.GetCoordinates(_toIndex) == from);
    }

    private void Update()
    {
        if (_pauseDuration > 0)
        {
            _pauseDuration -= Time.deltaTime;
        }
        else if ((_shipDefinition.CharacterCoordinates?.Length ?? 0) > 0)
        {
            var from = (Vector2)CharacterTransform.localPosition;
            var to = _shipDefinition.GetCoordinates(_toIndex);

            var vector = to - from;
            var moveVector = MoveSpeed * Time.deltaTime * vector.normalized;
            Vector2 targetCoordinates;
            if (from == to ||
                moveVector.sqrMagnitude > vector.sqrMagnitude)
            {
                targetCoordinates = to;

                NewTarget();
            }
            else
            {
                targetCoordinates = from + moveVector;
            }

            SetPosition(targetCoordinates);
        }
    }

    private void SetPosition(Vector2 position)
    {
        CharacterTransform.localPosition = new Vector3(position.x, position.y, -0.1f);
    }
}
