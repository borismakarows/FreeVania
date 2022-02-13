using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class cameraMovement : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject player;
    [SerializeField] CinemachineVirtualCamera[] virtualCameras = new CinemachineVirtualCamera[3];
    CinemachineFramingTransposer[] cameraFrames = new CinemachineFramingTransposer[3];
    float[] m_ScreenYAtStart = new float[3];
    Vector2 moveInputt;

    [Header("Attributes")]
    [SerializeField] float lookDown = 0.3f;
    [SerializeField] float lookUp = 0.7f;
    [SerializeField] float lookDownRate = 0.1f;
    [SerializeField] float lookUpRate = 0.1f;


    void Awake() 
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    
    void Start()
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            cameraFrames[i] = virtualCameras[i].GetCinemachineComponent<CinemachineFramingTransposer>();
            m_ScreenYAtStart[i] = cameraFrames[i].m_ScreenY;
        }
    }

    void Update()
    {
        // look();
    }


    void look()
    {
        // moveInputt = playerMovement.getMoveInput();
        
        
        
        if (moveInputt.y < 0)
        {
            while (moveInputt.y < 0)
            {
                for (int i = 0; i < cameraFrames.Length; i++)
                {
                    while (cameraFrames[i].m_ScreenY > lookDown)
                    {
                        for (int j = 0; j < cameraFrames.Length; j++)
                        {
                            cameraFrames[j].m_ScreenY -= lookDownRate;
                        }
                    }
                }
                
            }
        }

        else if (moveInputt.y > 0)
        {
            while (moveInputt.y > 0)
            {
                for (int i = 0; i < cameraFrames.Length; i++)
                {
                    while (cameraFrames[i].m_ScreenY < lookUp)
                    {
                        for (int j = 0; j < cameraFrames.Length; j++)
                        {
                            cameraFrames[j].m_ScreenY += lookUpRate;
                        }
                    }
                }
            }
        }
        
        else
        {
            return;
        }
            
    }

}
