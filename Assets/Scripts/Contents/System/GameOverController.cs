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

    private PlayerStats playerStats;
    private CharactorStats strongpointStats;



    private void Start()
    {
        var player = GameObject.FindWithTag(Tags.Player);
        var playerStats = player.GetComponent<PlayerStats>();
        playerStats.deathEvent.AddListener(OnDeathPlayer);
        this.playerStats = playerStats;

        onPlayerResurrectionEvent.AddListener(player.GetComponent<PlayerFSM>().OnResurrection);
        uiGameOverView.SetResurrectionAction(OnResurrectionEvent);
        var strongpointStats = GameObject.FindWithTag(Tags.PointStructure).GetComponentInChildren<CharactorStats>();

        this.strongpointStats = strongpointStats;   
        onResurrectionEventEvent.AddListener(strongpointStats.GetComponentInChildren<DestroyPlayerStrongpointEvent>().OnResurrection);
        strongpointStats.deathEvent.AddListener(OnDestroyStrongpoint);
    }



    public void OnDeathPlayer()
    {
        uiGameOverView.SetGameOverText("플레이어가 사망하였습니다.");
        uiGameOverView.gameObject.SetActive(true);
        isDeathPlayer = true;

        strongpointStats.OnChangeCanHit(false);
        // TODO :: 스트링 테이블 연동 예정
    }

    public void OnDestroyStrongpoint()
    {
        uiGameOverView.SetGameOverText("거점이 파괴되었습니다.");
        uiGameOverView.gameObject.SetActive(true);
        isDestroyStrongpoint = true;

        playerStats.OnChangeCanHit(false);
    }

    public void OnResurrectionEvent()
    {
        if(isDeathPlayer)
        {
            strongpointStats.OnChangeCanHit(true);
            onPlayerResurrectionEvent?.Invoke();
        }
        else if (isDestroyStrongpoint)
        {
            playerStats.OnChangeCanHit(true);
            onResurrectionEventEvent?.Invoke();
        }
    }
}
