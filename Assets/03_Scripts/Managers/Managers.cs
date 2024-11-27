using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Assets
{
    public class Managers : MonoBehaviour
    {
        static Managers s_instance;
        static Managers Instance { get { Init(); return s_instance; } }

        MapManager _mapMgr = new MapManager();
        ObjectManager _objectMgr = new ObjectManager();
        PoolManager _poolMgr = new PoolManager();
        ResourceManager _resourceMgr = new ResourceManager();
        SceneManagerEx _SceneMgrEx = new SceneManagerEx();
        NetworkManager _networkMgr = new NetworkManager();

        public static MapManager MapMgr { get { return Instance._mapMgr; } }
        public static ObjectManager ObjectMgr { get { return Instance._objectMgr; } }
        public static PoolManager PoolMgr { get { return Instance._poolMgr; } }
        public static ResourceManager ResourceMgr { get { return Instance._resourceMgr; } }
        public static SceneManagerEx SceneMgrEx { get { return Instance._SceneMgrEx; } }
        public static NetworkManager NetworkMgr { get { return Instance._networkMgr; } }

        void Start()
        {
            Init();
        }

        void Update()
        {
            _networkMgr.Update();
        }

        static void Init()
        {
            if (s_instance == null)
            {
                GameObject obj = GameObject.Find("@Managers");
                if (obj == null)
                {
                    obj = new GameObject { name = "@Managers" };
                }
                DontDestroyOnLoad(obj);

                s_instance = obj.AddComponent<Managers>();

                // Instantce 초기화
                s_instance._poolMgr.Init();
                s_instance._networkMgr.Init();
            }
        }

        public static void Clear()
        {
            SceneMgrEx.Clear();
            PoolMgr.Clear();
            ObjectMgr.Clear();
        }
    }
}
