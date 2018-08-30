using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventsController : MonoBehaviour {

    public static EventsController Instance
    { get; private set; }

    public delegate void PlayerLifeChange();
    public delegate void BoatPieceObtained();

    public event PlayerLifeChange OnPlayerLifeChange;
    public event BoatPieceObtained OnBoatPieceObtained;

    public Transform m_moneyParticles;
    public Transform m_angryMoneyParticles;

    [Header("Start game stuff")]
    public GunTable m_gunTable;
    public PlayerControl m_player;
    public Transform m_startGamePosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // Set singleton
        DontDestroyOnLoad(gameObject); // Ensures this does not destroy on load
        Instance = this;
    }

    private void Start()
    {
        EffectCanvas.Instance.TitleText("Press [H] for info. about game");

        // Set position start of game
        if (!m_startGamePosition)
            return;

        m_player.transform.position = m_startGamePosition.position;
        m_player.transform.rotation = m_startGamePosition.rotation;

        // Set gun start of game
        m_player.ActiveGun(false);
        m_gunTable.PlaceGun();
    }

    public void TriggerPlayerLifeChange()
    {
        if (OnPlayerLifeChange != null)
            OnPlayerLifeChange();
    }

    public void TriggerBoatPieceObtained()
    {
        if (OnBoatPieceObtained != null)
            OnBoatPieceObtained();
    }

    public void SummonMoney(Vector3 _position)
    {
        Instantiate(m_moneyParticles, _position, Quaternion.identity);
    }

    public void SummonAngryMoney(Vector3 _position)
    {
        Instantiate(m_angryMoneyParticles, _position, Quaternion.identity);
    }
}
