using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.Services
{
    // 该接口用于切换游戏摄像机
    public interface IGameCameraService : IService
    {
        CameraData GetCurrentCameraData();
        void NextPos();
        void PrevPos();
    }
    
    public struct CameraChangeEvent
    {
        public CameraData data;
    }

    public struct CameraData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float fieldOfView;
        public bool fovHorizontal; // 是否水平FOV
        public bool reverseY; // 是否反转Y轴
    }
    
    public class GameCameraService : AbstractService, IGameCameraService
    {
        private CameraData[] cameraPositions;

        private int currentIndex;
        // 当前摄像机位置索引
        
        public override void Init()
        {
            //可以使用配置表来获取摄像机位置数据
            //也可以加载ScriptableObject来获取摄像机位置数据
            //这里为了简单起见，直接在代码中定义摄像机位置数据
            cameraPositions = new CameraData[]
            {
                new CameraData
                {
                    position = new Vector3(0, 0, -10),
                    rotation = Quaternion.Euler(0, 0, 0),
                    fieldOfView = 60,
                    reverseY = true
                },
                new CameraData
                {
                    position = new Vector3(60, 0, 20),
                    rotation = Quaternion.Euler(2, -90, 0),
                    fieldOfView = 60,
                    fovHorizontal = true,
                    reverseY = true
                },
                new CameraData
                {
                    position = new Vector3(0, 100, 17),
                    rotation = Quaternion.Euler(90, 0, 0),
                    fieldOfView = 40f,
                }
            };
        }


        public CameraData GetCurrentCameraData()
        {
            return cameraPositions[currentIndex];
        }

        public void NextPos()
        {
            // 切换到下一个摄像机位置
            currentIndex = (currentIndex + 1) % cameraPositions.Length;
            // 发送摄像机位置变化事件
            this.SendEvent(new CameraChangeEvent
            {
                data = cameraPositions[currentIndex]
            });
        }

        public void PrevPos()
        {
            // 切换到上一个摄像机位置
            currentIndex = (currentIndex - 1 + cameraPositions.Length) % cameraPositions.Length;
            // 发送摄像机位置变化事件
            this.SendEvent(new CameraChangeEvent
            {
                data = cameraPositions[currentIndex]
            });
        }
    }
}