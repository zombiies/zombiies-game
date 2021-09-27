using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class GameController : Singleton<GameController>
{
    #region Avaiable
    [Header("PLAYER ELEMENTS")]
    [SerializeField] Text p_txtAmountItem = default;
    [SerializeField] Text p_txtAmountZombie = default;
    [SerializeField] Text p_txtAmountCrytal = default;
    [SerializeField] IGCardUI[] p_cardUI = new IGCardUI[3];
    [SerializeField] CardInformation cardZombie1 = default;
    [SerializeField] CardInformation cardZombie2 = default;
    [SerializeField] CardInformation cardEquipment1 = default;
    [SerializeField] CardInformation cardEquipment2 = default;
    [SerializeField] CardInformationUI cardZombieUI1 = default;
    [SerializeField] CardInformationUI cardZombieUI2 = default;
    [SerializeField] CardInformationUI cardEquipmentUI1 = default;
    [SerializeField] CardInformationUI cardEquipmentUI2 = default;
    private ZombieModel[] p_onHand;
    [Header("ENEMY ELEMENTS")]
    [SerializeField] Text e_txtAmountItem = default;
    [SerializeField] Text e_txtAmountZombie = default;
    [SerializeField] Text e_txtAmountCrytal = default;
    [SerializeField] IGCardUI[] e_cardUI = new IGCardUI[3];
    [Header("POPUP")]
    public GameObject objVictoryPopup = default;
    public GameObject objLosePopup = default;
    [SerializeField] CardInformation cardWin = default;
    [SerializeField] CardInformationUI cardWinUI = default;
    [SerializeField] Button btnHome = default;
    [SerializeField] Text txttime;
    [SerializeField] Button btnConfirm;
    private Transform tranCardHeld = default;
    private Vector3 _oldPosCard = default;
    private Vector3 _oldRotCard = default;
    private bool isHeld = false;
    private string _tokenId = "";
    private int _currentTurn = -1;
    #endregion
    void Awake()
    {
        btnConfirm.onClick.AddListener(OnConfirmButtonClicked);
        btnHome.onClick.AddListener(OnHomeButtonClicked);
        UpdateGame(DataManager.Instance.matchStarted);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHeld)
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("onHand"));
            if (rayHit.collider && rayHit.collider.CompareTag("PCardHand"))
            {
                _tokenId = rayHit.collider.gameObject.GetComponent<CardInformation>().tokenid;
                isHeld = true;
                tranCardHeld = rayHit.collider.transform;
                _oldPosCard = tranCardHeld.localPosition;
                _oldRotCard = tranCardHeld.localEulerAngles;
                tranCardHeld.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        if (Input.GetMouseButton(0) && isHeld)
        {
            Vector3 mousePos = Input.mousePosition;
            tranCardHeld.localPosition = new Vector3(mousePos.x - 700, mousePos.y - 400, 0);
        }
        if (Input.GetMouseButtonUp(0) && isHeld)
        {
            isHeld = false;
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("onBoard"));
            if (rayHit.collider && rayHit.collider.CompareTag("PCardBoard"))
            {
                int position = int.Parse(rayHit.collider.name);
                GraphQLManager.Instance.PutCardToBoard(position, _tokenId, () =>
                 {
                     tranCardHeld.gameObject.SetActive(false);
                     tranCardHeld.localEulerAngles = _oldRotCard;
                     tranCardHeld.localPosition = _oldPosCard;
                 },
                () =>
                {
                    tranCardHeld.localEulerAngles = _oldRotCard;
                    tranCardHeld.localPosition = _oldPosCard;
                }
                );
            }
            if (!rayHit.collider || !rayHit.collider.CompareTag("PCardBoard"))
            {
                tranCardHeld.localEulerAngles = _oldRotCard;
                tranCardHeld.localPosition = _oldPosCard;
            }
        }
    }
    private void SetUpText(PlayerStatusModel playerStatus)
    {
        if (playerStatus.playerId.Equals(DataManager.Instance.userInfo.id))
        {
            p_txtAmountItem.text = playerStatus.GetAmountCardByType(TypeCard.EQUIPMENT).ToString();
            p_txtAmountZombie.text = playerStatus.GetAmountCardByType(TypeCard.MONSTER).ToString();
            p_txtAmountCrytal.text = playerStatus.crystal.ToString();
            return;
        }
        e_txtAmountItem.text = playerStatus.GetAmountCardByType(TypeCard.EQUIPMENT).ToString();
        e_txtAmountZombie.text = playerStatus.GetAmountCardByType(TypeCard.MONSTER).ToString();
        e_txtAmountCrytal.text = playerStatus.crystal.ToString();
    }
    public void UpdateGame(MatchModel matchModel)
    {
        foreach (var item in matchModel.playerStatuses)
        {
            SetTextTime(item);
            SetUpText(item);
            for (int i = 0; i < (item.onBoard?.Length ?? 0); i++)
            {
                SetUpCardOnBoard(item.onBoard[i], item.playerId);
            }
            SetUpCardOnHand(item.onHand, item.playerId);
        }
    }
    public void SetUpCardOnBoard(BoardCardModel boardCard, string playerID)
    {
        if (playerID.Equals(DataManager.Instance.userInfo.id))
            SetCardByPosition(boardCard, p_cardUI);
        else
            SetCardByPosition(boardCard, e_cardUI);
    }
    private void SetCardByPosition(BoardCardModel boardCard, IGCardUI[] iGCardUIs)
    {
        switch (boardCard.position)
        {
            case 1:
                iGCardUIs[0].SetUpCard(boardCard);
                break;
            case 2:
                iGCardUIs[1].SetUpCard(boardCard);
                break;
            case 3:
                iGCardUIs[2].SetUpCard(boardCard);
                break;
        }
    }
    public void SetUpCardOnHand(ZombieModel[] handCard, string playerID)
    {
        if (!playerID.Equals(DataManager.Instance.userInfo.id)) return;
        List<ZombieModel> zombies = handCard.ToList().FindAll(x => x.type == TypeCard.MONSTER);
        List<ZombieModel> equipments = handCard.ToList().FindAll(x => x.type == TypeCard.EQUIPMENT);
        if (zombies.Count >= 1 && zombies[0] != null)
        {
            cardZombie1.SetData(zombies[0]);
            cardZombieUI1.SetUpCard();
            cardZombie1.gameObject.SetActive(true);
        }
        if (zombies.Count >= 2 && zombies[1] != null)
        {
            cardZombie2.SetData(zombies[1]);
            cardZombieUI2.SetUpCard();
            cardZombie2.gameObject.SetActive(true);
        }
        if (equipments.Count >= 1 && equipments[0] != null)
        {
            cardEquipment1.SetData(equipments[0]);
            cardEquipmentUI1.SetUpCard();
            cardEquipment1.gameObject.SetActive(true);
        }
        if (equipments.Count >= 2 && equipments[1] != null)
        {
            cardEquipment2.SetData(equipments[1]);
            cardEquipmentUI2.SetUpCard();
            cardEquipment2.gameObject.SetActive(true);
        }
    }

    public void SetTextTime(PlayerStatusModel match)
    {
        if (match.playerId.Equals(DataManager.Instance.userInfo.id) && match.inTurn && !match.confirmTurn)
        {
            if (_currentTurn != 0)
            {
                _currentTurn = 0;
                btnConfirm.gameObject.SetActive(true);
                btnConfirm.interactable = true;
                txttime.color = new Color32(255, 255, 255, 255);
                _time = 29;
                ResetLoading();
            }
        }
        if (match.playerId.Equals(DataManager.Instance.userInfo.id) && match.inTurn && match.confirmTurn)
        {
            btnConfirm.interactable = false;
            txttime.color = new Color32(121, 121, 121, 121);
            isStop = true;
        }
        if (match.playerId.Equals(DataManager.Instance.userInfo.id) && !match.inTurn)
        {
            if (_currentTurn != 1)
            {
                _currentTurn = 1;
                btnConfirm.gameObject.SetActive(false);
            }
        }

    }
    public void HandleAttack(string playerId, AttackEventModel[] attacks)
    {
        if (DataManager.Instance.userInfo.id.Equals(playerId))
            StartCoroutine(IEPlayerAttack(attacks, p_cardUI, e_cardUI));
        else
            StartCoroutine(IEPlayerAttack(attacks, e_cardUI, p_cardUI));
    }
    private IEnumerator IEPlayerAttack(AttackEventModel[] attacks, IGCardUI[] _p_cardUI, IGCardUI[] _e_cardUI)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            IGCardUI playerCard = GetCardUI(_p_cardUI, attacks[i].tokenId);
            IGCardUI targetCard = GetCardUI(_e_cardUI, attacks[i].toPosition);
            playerCard.PlayAnimAttack();
            targetCard.PlayAnimHit(attacks[i].skill.type);
            yield return new WaitForSeconds(1f);
            if (attacks[i].destroy)
                targetCard.gameObject.SetActive(false);
            foreach (var item in attacks[i].targetOnBoard)
            {
                SetCardByPosition(item, _e_cardUI);
            }
            yield return new WaitForSeconds(0.2f);
            if (i == attacks.Length - 1)
                GraphQLManager.Instance.EndTurn();
        }
    }
    private void OnConfirmButtonClicked()
    {
        GraphQLManager.Instance.ConfirmTurn();
    }
    private bool isStop = false;
    private Tween tween;
    private int _time = 29;
    private void ResetLoading()
    {
        isStop = false;
        tween = DOTween.To(() => _time, x => _time = x, 0, 29f)
            .OnUpdate(() =>
            {
                txttime.text = _time.ToString();
                if (isStop)
                    tween.Kill();
            })
            .OnComplete(() =>
            {
                if (hasCardOnBoard())
                    GraphQLManager.Instance.ConfirmTurn();
            });
    }
    private IGCardUI GetCardUI(IGCardUI[] iGCardUIs, string _tokenId)
    {
        for (int i = 0; i < iGCardUIs.Length; i++)
        {
            if (iGCardUIs[i].tokenId.Equals(_tokenId) && iGCardUIs[i].gameObject.activeSelf)
                return iGCardUIs[i];
        }
        return null;
    }
    private IGCardUI GetCardUI(IGCardUI[] iGCardUIs, int _position)
    {
        for (int i = 0; i < iGCardUIs.Length; i++)
        {
            if (iGCardUIs[i].positionCard == _position && iGCardUIs[i].gameObject.activeSelf)
                return iGCardUIs[i];
        }
        return null;
    }
    public bool hasCardOnBoard()
    {
        for (int i = 0; i < p_cardUI.Length; i++)
        {
            if (p_cardUI[i].gameObject.activeSelf)
                return true;
        }
        return false;
    }
    public void BackHome()
    {
        SceneController.Instance.OpenScene(SCENE_NAME.Home);
    }
    private void OnHomeButtonClicked()
    {
        GraphQLManager.Instance.Surrender();
        BackHome();
    }
    public void SetRewardCard(ZombieModel card)
    {
        cardWin.gameObject.SetActive(true);
        cardWin.SetData(card);
        cardWinUI.SetUpCard();
    }
    private void OnApplicationQuit()
    {
        GraphQLManager.Instance.Surrender();
    }
}
