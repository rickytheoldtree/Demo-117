using Demo_117.Services;
using DG.Tweening;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.GamePlay
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour, ICanGetLocator<Entity>
    {
        private readonly float duration = 0.6f;
        private readonly Ease ease = Ease.InOutSine;

        private Camera cam;
        private IGameCameraService gameCameraService;

        private void Awake()
        {
            this.TryGetService(out gameCameraService);
            cam = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            this.RegisterEvent<CameraChangeEvent>(OnCameraChanged);
            var data = gameCameraService.GetCurrentCameraData();
            SetCamera(data); 
        }

        private void OnDisable()
        {
            this.UnRegisterEvent<CameraChangeEvent>(OnCameraChanged);
            DOTween.Kill(this);
        }

        private void OnCameraChanged(CameraChangeEvent e)
        {
            TweenToCamera(e.data, duration, ease);
        }

// 将输入FOV标准化为“垂直FOV”，Unity 的 Camera.fieldOfView 是垂直FOV
        private float ToVerticalFOV(float fovDeg, bool isHorizontal, float aspect)
        {
            fovDeg = Mathf.Clamp(fovDeg, 1f, 170f);
            if (!isHorizontal) return fovDeg; // 本来就是垂直FOV

            // 水平FOV -> 垂直FOV： v = 2 * atan( tan(h/2) / aspect )
            float hRad = fovDeg * Mathf.Deg2Rad;
            float vRad = 2f * Mathf.Atan(Mathf.Tan(hRad * 0.5f) / Mathf.Max(0.0001f, aspect));
            return Mathf.Clamp(vRad * Mathf.Rad2Deg, 1f, 170f);
        }

        private void SetCamera(CameraData data)
        {
            DOTween.Kill(this);

            transform.position = data.position;
            transform.rotation = data.rotation;

            cam.orthographic = false; // 统一透视
            float vFov = ToVerticalFOV(data.fieldOfView, data.fovHorizontal, cam.aspect);
            cam.fieldOfView = vFov;
        }

        private void TweenToCamera(CameraData data, float d, Ease e)
        {
            DOTween.Kill(this);

            var seq = DOTween.Sequence().SetId(this).SetUpdate(true).SetEase(e);

            // 位姿动画
            seq.Join(transform.DOMove(data.position, d));
            seq.Join(transform.DORotateQuaternion(data.rotation, d));

            // FOV动画（始终用垂直FOV补间）
            cam.orthographic = false;
            float targetVFov = ToVerticalFOV(data.fieldOfView, data.fovHorizontal, cam.aspect);
            seq.Join(DOTween.To(() => cam.fieldOfView, v => cam.fieldOfView = v, targetVFov, d));
        }
    }
}
