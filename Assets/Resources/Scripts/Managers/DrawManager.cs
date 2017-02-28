using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos {
    public static class DrawManager {
        //Game Objects
        public static GameObject mainCamera;
        public static GameObject drawGameObject;
        public static GameObject meshesGameObject;
        //Camera
        private static Vector2 cameraPos = new Vector2(0, 0);
        private static Vector2 targetCameraPos = new Vector2(0, 0);
        private static float cameraOrthoSize;
        private static float targetSize = 1f;
        private static float cameraEase = 0.2f;
        private static bool zoomToMouse = true;
        //GameObjects
        private static float poolGenMSLimit = 2;
        private static int minPooledGameObjects = 50;
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> gameObjectsPool = new List<GameObject>();
        private static MeshFilter quadGOFilter;
        //    Other
        private static Shader transparentShader = Shader.Find("Transparent/Cutout/Diffuse");

        public static void Init() {
            //Setup camera and game objects
            drawGameObject = new GameObject("DrawGameObject");
            meshesGameObject = new GameObject("Meshes");
            mainCamera = Camera.main.gameObject;
            drawGameObject.transform.parent = mainCamera.transform;
            meshesGameObject.transform.parent = drawGameObject.transform;
            //
            gameObjects = new List<GameObject>();
            gameObjectsPool = new List<GameObject>();
            //
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quadGOFilter = quad.GetComponent<MeshFilter>();
            quad.SetActive(false);
        }

        public static void Draw() {
            GenGameObjectsPool();
            UpdateCameraPos();
        }

        #region Camera
        private static void UpdateCameraPos() {
            //TargetSize = GameManager.currentGame.currentZoomScale;
            cameraOrthoSize = Mathf.SmoothStep(Camera.main.orthographicSize, targetSize, 0.25f);
            if (zoomToMouse && cameraOrthoSize - targetSize > 0.01f) {
                Vector3 MousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 MousePos = new Vector2(MousePos3.x, MousePos3.y);
                float OrthoMultiplier = (2f / Camera.main.orthographicSize * (Camera.main.orthographicSize - cameraOrthoSize));
                Vector2 MouseOffset = (MousePos - targetCameraPos) * OrthoMultiplier;
                targetCameraPos += MouseOffset;
            }
            cameraPos += (targetCameraPos - cameraPos) * cameraEase;
            //
            Camera.main.transform.position = new Vector3(cameraPos.x, cameraPos.y, -100);
            Camera.main.orthographicSize = cameraOrthoSize;

        }
        public static void MoveCameraBy(Vector2 amount) {
            targetCameraPos = targetCameraPos + amount;
        }
        public static void MoveCameraTo(Vector2 pos, float zoom = 0) {
            targetCameraPos = pos;
            if (zoom != 0) {
                //GameManager.currentGame.currentZoomScale = zoom;
            }
        }
        #endregion


        #region GameObjects
        private static void GenGameObjectsPool() {
            float startTime = Time.realtimeSinceStartup;
            while (gameObjectsPool.Count < minPooledGameObjects && Time.realtimeSinceStartup - startTime < poolGenMSLimit) {
                gameObjectsPool.Add(MakeSprite());
            }
        }
        private static GameObject MakeSprite() {
            GameObject gameObject = new GameObject();
            gameObject.transform.parent = meshesGameObject.transform;
            gameObject.name = "";
            gameObject.SetActive(false);
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            //collider = gameObject.AddComponent<BoxCollider> ();
            //collider.center = new Vector3 (0.5f, -0.5f, 0);
            //collider.size = new Vector3 (1f, 1f, 0.1f);
            meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.material.shader = transparentShader;
            meshFilter.mesh = quadGOFilter.mesh;
            return gameObject;

        }

        public static void RenderEntity(Entity entity) {
            if (entity.gameObject == null) {
                AssignGameObjectToEntity(entity);
            }
            UpdateGameObject(entity);
        }
        private static void UpdateGameObject(Entity entity) {
            //
            GameObject gameObject = entity.gameObject;
            gameObject.SetActive(entity.visible);
            gameObject.name = entity.name;
            entity.meshRenderer.sharedMaterial= entity.material;
            if (entity.visible) {
                Transform transform = gameObject.transform;
                transform.position = entity.position;
                transform.localScale = entity.scale;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, entity.rotation, transform.eulerAngles.z);
            }
            //
        }
        private static void AssignGameObjectToEntity(Entity entity) {
            GameObject gameObject = GetPooledGameObject();
            entity.gameObject = gameObject;
            entity.meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        private static GameObject GetPooledGameObject() {
            if (gameObjectsPool.Count == 0) {
                GenGameObjectsPool();
            }
            GameObject go = gameObjectsPool[0];
            gameObjects.Add(go);
            gameObjectsPool.RemoveAt(0);
            return go;
        }
        #endregion
    }
}