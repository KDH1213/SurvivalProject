using UnityEngine.Events;

public interface IGather
{
    void OnInteraction();
    UnityEvent<int> OnEndInteractionEvent { get; }

    int TileID { get; set; }
}
