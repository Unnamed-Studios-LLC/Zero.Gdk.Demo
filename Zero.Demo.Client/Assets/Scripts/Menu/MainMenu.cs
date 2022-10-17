using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnnamedStudios.Common.Model;
using Zero.Demo.Model.Api;

public class MainMenu : MonoBehaviour
{
    public CharacterSelection CharacterSelection;
    public TMP_InputField NameInput;
    public TMP_InputField CodeInput;
    public TMP_InputField WorldKeyInput;
    public TextMeshProUGUI Error;

    private ServiceResponse<CreateConnectionResponse> _response;

    public static string JoinKey { get; set; } = string.Empty;

    public void Play()
    {
        if (string.IsNullOrWhiteSpace(NameInput.text))
        {
            Error.text = "Please enter a name";
            return;
        }
        Error.text = string.Empty;

        PlayerPrefs.SetString("name", NameInput.text);
        PlayerPrefs.Save();

        var request = new CreateConnectionRequest
        {
            Name = NameInput.text,
            WorldKey = WorldKeyInput.text.ToUpper(),
            Hat = CharacterSelection.HatIndex,
            Head = CharacterSelection.HeadIndex,
            Legs = CharacterSelection.LegsIndex,
            Flag = CharacterSelection.FlagIndex,
            FlagColor = CharacterSelection.FlagColorIndex,
            ClientVersion = Application.version
        };
        var playTask = Task.Run(() => PlayAsync(request));
        Loading.Show(playTask);
    }

    private void Awake()
    {
        NameInput.text = PlayerPrefs.GetString("name", string.Empty);
        WorldKeyInput.text = JoinKey;
        JoinKey = string.Empty;
    }

    private void LateUpdate()
    {
        if (_response != null)
        {
            if (!_response.Successful)
            {
                Debug.LogError("Failed to create connection");
                Error.text = _response.StatusCode switch
                {
                    400 => "Please use a different name",
                    404 => "Incorrect or invalid W#",
                    _ => "Unable to connect, please try again later",
                };
            }
            else
            {
                DemoGameService.Response = _response.Object;
                SceneManager.LoadScene("GameScene");
            }
            _response = null;
        }
    }

    private async Task PlayAsync(CreateConnectionRequest request)
    {
        _response = await Api.Client.ConnectionCreateAsync(request);
    }

    private void Start()
    {
        DiscordController.Instance.SetMainMenu();
    }
}
