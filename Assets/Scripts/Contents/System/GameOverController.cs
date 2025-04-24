using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    private UIGameOverView uiGameOverView;

    private bool isDeathPlayer = false;
    private bool isDestroyStrongpoint = false;

    public UnityEvent onPlayerResurrectionEvent;
    public UnityEvent onResurrectionEventEvent;



    private void Start()
    {
        var player = GameObject.FindWithTag(Tags.Player);
        var playerStats = player.GetComponent<PlayerStats>();
        playerStats.deathEvent.AddListener(OnDeathPlayer);

        onPlayerResurrectionEvent.AddListener(player.GetComponent<PlayerFSM>().OnResurrection);
        uiGameOverView.SetResurrectionAction(OnResurrectionEvent);
        var strongpointStats = GameObject.FindWithTag(Tags.PointStructure).GetComponentInChildren<CharactorStats>();

        onResurrectionEventEvent.AddListener(strongpointStats.GetComponentInChildren<DestroyPlayerStrongpointEvent>().OnResurrection);
        strongpointStats.deathEvent.AddListener(OnDestroyStrongpoint);
    }



    public void OnDeathPlayer()
    {
        uiGameOverView.SetGameOverText("�÷��̾ ����Ͽ����ϴ�.");
        uiGameOverView.gameObject.SetActive(true);
        isDeathPlayer = true;
        // TODO :: ��Ʈ�� ���̺� ���� ����
    }

    public void OnDestroyStrongpoint()
    {
        uiGameOverView.SetGameOverText("������ �ı��Ǿ����ϴ�.");
        uiGameOverView.gameObject.SetActive(true);
        isDestroyStrongpoint = true;
    }

    public void OnResurrectionEvent()
    {
        if(isDeathPlayer)
        {
            onPlayerResurrectionEvent?.Invoke();
        }
        else if (isDestroyStrongpoint)
        {
            onResurrectionEventEvent?.Invoke();
        }
    }
}
