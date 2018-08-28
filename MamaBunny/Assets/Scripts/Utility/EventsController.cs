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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // Set singleton
        DontDestroyOnLoad(gameObject); // Ensures this does not destroy on load
        EffectCanvas.Instance.TitleText("Press [H] for info. about game");
        Instance = this;
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
}
