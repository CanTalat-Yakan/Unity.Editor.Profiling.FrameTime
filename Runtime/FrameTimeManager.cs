using UnityEngine;
using UnityEngine.Rendering;

namespace Unity.Essentials
{
    public class FrameTimeManager : MonoBehaviour
    {
        [SerializeField] private FrameTimeMonitor _monitor;
        [SerializeField] private FrameTimeGraphRenderer _graphRenderer;
        [SerializeField] private FrameTimeGUI _gui;

        [Space]
        [Header("Display Settings")]
        [SerializeField] private bool _displayStatistics = true;
        [Space]
        [SerializeField] private Vector2 _graphSizePercentage = new(50, 10);
        [SerializeField] private Vector2 _bottomLeftPivotPercentage = new(0, 10);

        [Space]
        [SerializeField] private KeyCode _modifierKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode _toggleKey = KeyCode.F3;

        public bool DisplayStatistics;
        public int TargetRefreshRate;
        public Vector2 GraphSize;
        public Vector2 BottomLeftPivot;

#if UNITY_EDITOR
        public void OnValidate()
        {
            GraphSize = _graphSizePercentage / 100f;
            BottomLeftPivot = _bottomLeftPivotPercentage / 100f;
        }
#endif

        public void Update()
        {
            DisplayStatistics = _displayStatistics;

            if (Input.GetKeyDown(_toggleKey))
                if (_displayStatistics || Input.GetKey(_modifierKey))
                    _displayStatistics = !_displayStatistics;


            // Handle refresh rate changes
            if (TargetRefreshRate != (int)Screen.currentResolution.refreshRateRatio.value)
                TargetRefreshRate = (int)Screen.currentResolution.refreshRateRatio.value;
        }

        public void Awake()
        {
            if (_monitor == null)
                _monitor = GetComponent<FrameTimeMonitor>();

            if (_graphRenderer == null)
                _graphRenderer = GetComponent<FrameTimeGraphRenderer>();

            if (_gui == null)
                _gui = GetComponent<FrameTimeGUI>();

            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        public void OnDestroy() =>
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera.cameraType != CameraType.Game)
                return;

            _monitor.OnEndCameraRendering();
            _graphRenderer.DrawFrameTimeGraph();
        }
    }
}