using UnityEngine;
using Photon.Pun;

public class PlayerStats : MonoBehaviourPun, IPunObservable
{
    public static PlayerStats Instance { get; private set; }

    [Tooltip("The shared score")]
    public int CurrentScore = 0;
    public int GalaxiesJumped = 0;

    void Awake()
    {
        // ensure singleton, only one stats object per client
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ensure this object has a PhotonView set to Observed Components → this script
        }
        else Destroy(gameObject);
    }

    // Add Score
    public void AddGalaxiesJumped(int amount)
    {
        GalaxiesJumped += amount;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // we own this, send our current score
            stream.SendNext(CurrentScore);
        }
        else
        {
            // remote client sent us their score––overwrite ours
            CurrentScore = (int)stream.ReceiveNext();
        }
    }
}