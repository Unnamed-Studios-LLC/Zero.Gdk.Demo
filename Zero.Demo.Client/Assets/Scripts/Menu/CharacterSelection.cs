using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private int _hatIndex;
    private int _headIndex;
    private int _legsIndex;
    private int _flagIndex;
    private int _flagColorIndex;

    public Image HatImage;
    public Image HeadImage;
    public Image LegsImage;
    public Image FlagImage;
    public Image FlagColorImage;
    public RectTransform FlagColorSelection;
    public RectTransform[] FlagColorSelections;

    public int HatIndex
    {
        get => _hatIndex;
        set => SetHat(value);
    }

    public int HeadIndex
    {
        get => _headIndex;
        set => SetHead(value);
    }

    public int LegsIndex
    {
        get => _legsIndex;
        set => SetLegs(value);
    }

    public int FlagIndex
    {
        get => _flagIndex;
        set => SetFlag(value);
    }

    public int FlagColorIndex
    {
        get => _flagColorIndex;
        set => SetFlagColor(value);
    }

    public void DecrementHat() => HatIndex = Mod(HatIndex - 1, CharacterLibrary.HatCount);
    public void DecrementHead() => HeadIndex = Mod(HeadIndex - 1, CharacterLibrary.HeadCount);
    public void DecrementLegs() => LegsIndex = Mod(LegsIndex - 1, CharacterLibrary.LegsCount);
    public void DecrementFlag() => FlagIndex = Mod(FlagIndex - 1, CharacterLibrary.FlagCount);

    public void IncrementHat() => HatIndex = Mod(HatIndex + 1, CharacterLibrary.HatCount);
    public void IncrementHead() => HeadIndex = Mod(HeadIndex + 1, CharacterLibrary.HeadCount);
    public void IncrementLegs() => LegsIndex = Mod(LegsIndex + 1, CharacterLibrary.LegsCount);
    public void IncrementFlag() => FlagIndex = Mod(FlagIndex + 1, CharacterLibrary.FlagCount);

    public void Save()
    {
        PlayerPrefs.Save();
    }

    public void SetFlagColor(int value)
    {
        _flagColorIndex = value;
        PlayerPrefs.SetInt("flag-color", value);
        FlagColorImage.color = CharacterLibrary.GetFlagColor(value);
        FlagColorSelection.position = FlagColorSelections[Mod(value, CharacterLibrary.FlagColorCount)].position;
    }

    private static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    private void Start()
    {
        HatIndex = PlayerPrefs.GetInt("hat", 0);
        HeadIndex = PlayerPrefs.GetInt("head", 0);
        LegsIndex = PlayerPrefs.GetInt("legs", 0);
        FlagIndex = PlayerPrefs.GetInt("flag", 0);
        FlagColorIndex = PlayerPrefs.GetInt("flag-color", 0);
    }

    private void LateUpdate()
    {
        UpdateHatSize();
    }

    private void SetHat(int value)
    {
        _hatIndex = value;
        PlayerPrefs.SetInt("hat", value);
        HatImage.sprite = CharacterLibrary.GetHat(value);
    }

    private void SetHead(int value)
    {
        _headIndex = value;
        PlayerPrefs.SetInt("head", value);
        HeadImage.color = CharacterLibrary.GetHeadColor(value);
    }

    private void SetLegs(int value)
    {
        _legsIndex = value;
        PlayerPrefs.SetInt("legs", value);
        LegsImage.color = CharacterLibrary.GetLegsColor(value);
    }

    private void SetFlag(int value)
    {
        _flagIndex = value;
        PlayerPrefs.SetInt("flag", value);
        FlagImage.sprite = CharacterLibrary.GetFlag(value);
    }

    private void UpdateHatSize()
    {
        var sprite = HatImage.sprite;
        if (sprite == null)
        {
            return;
        }

        var headSize = HeadImage.rectTransform.rect.size;
        var pixelScale = headSize.x / 5f;
        HatImage.rectTransform.sizeDelta = sprite.textureRect.size * pixelScale;
        HatImage.rectTransform.anchoredPosition = (new Vector2(sprite.textureRect.width / 2f, 0) - sprite.pivot) * pixelScale;
    }
}
