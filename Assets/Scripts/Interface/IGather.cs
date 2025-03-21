using UnityEngine.Events;

public interface IGather
{
    void OnInteraction();
    UnityEvent<int> OnEndInteractionEvent { set; get; }

    int TileID { get; set; }
}
