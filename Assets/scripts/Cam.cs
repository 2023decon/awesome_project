using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Cam : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public CinemachineCameraOffset CameraOffset;
    public CinemachineFollowZoom FollowZoom;
    private CinemachineBasicMultiChannelPerlin noise;

    [SerializeField] private Text encountingText;
    [SerializeField] private Image encountingImage;

    public Vector3 defRot;
    public Volume airboneVol;

    private string state = "default";
    private float timeline = 0;

    private static Cam instance;
    public static Cam Instance {
        get {
            if (instance == null) return null;
            else return instance;
        }
    }

    void Start()
    {
        instance = this;

        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CameraOffset = GetComponent<CinemachineCameraOffset>();
        FollowZoom = GetComponent<CinemachineFollowZoom>();
        noise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
        if (state != "default") timeline += Time.deltaTime;

        if (state == "inAirbone") {
            float tick = timeline * 50f;
            Vector2 offset = new Vector2(Player.Instance.transform.position.x - transform.position.x + Player.Instance.direction * 1.5f, Player.Instance.transform.position.y - transform.position.y);

            CameraOffset.m_Offset = new Vector2(offset.x / 10 * tick, offset.y / 10 * tick);
            airboneVol.weight = 0.1f * tick;
            VirtualCamera.m_Lens.Dutch = -0.7f * tick;
            VirtualCamera.m_Lens.OrthographicSize = 8 - 0.4f * tick;

            if (tick > 10) {
                VirtualCamera.m_Lens.Dutch = -7;
                VirtualCamera.m_Lens.OrthographicSize = 4;

                state = "default";
            }
        } else if (state == "outAirbone") {
            float tick = timeline * 50f;
            Vector2 offset = new Vector2(Player.Instance.transform.position.x - transform.position.x + Player.Instance.direction * 1.5f, Player.Instance.transform.position.y - transform.position.y);

            CameraOffset.m_Offset = new Vector2(offset.x - (offset.x / 10 * tick), offset.y - (offset.y / 10 * tick));
            airboneVol.weight = 1 - (0.1f * tick);
            VirtualCamera.m_Lens.Dutch = 7 - 0.7f * tick;
            VirtualCamera.m_Lens.OrthographicSize = 4 + 0.4f * tick;

            if (tick > 10) {
                VirtualCamera.m_Lens.Dutch = 0;
                VirtualCamera.m_Lens.OrthographicSize = 8;
                
                state = "default";
            }
        }
    }

    public void InAirbone() {
        state = "inAirbone";
        timeline = 0;
    }

    public void OutAirbone() {
        state = "outAirbone";
        timeline = 0;
    }

    public void Shake(int strength, float dur) {
        StartCoroutine(_shake(strength, dur));
    }

    IEnumerator _shake(int strength, float dur)
    {
        noise.m_AmplitudeGain = strength;

        yield return new WaitForSeconds(dur);

        noise.m_AmplitudeGain = 0;
    }
}
