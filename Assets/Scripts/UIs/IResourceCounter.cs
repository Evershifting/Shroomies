using TMPro;

public interface IResourceCounter
{
    TextMeshProUGUI ResourceAmount { get; set; }

    void Init(string resource, int value);
}