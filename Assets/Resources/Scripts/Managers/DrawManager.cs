using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class DrawManager
	{
        //Game Objects
        public static GameObject mainCamera;
        public static GameObject drawGameObject;
        public static GameObject meshesGameObject;
        //Camera
        private static Vector2 cameraPos = new Vector2(0, 0);
        private static Vector2 targetCameraPos = new Vector2(0, 0);
        private static Vector2 displaySize;
        private static float cameraOrthoSize;
        private static float targetSize = 1f;
        private static float cameraEase = 0.2f;
        private static bool zoomToMouse = true;
        //Meshes
        private static List<MeshDisplay> meshDisplays = new List<MeshDisplay>(); // Displays to draw
        private static List<MeshDisplay> pooledDisplays = new List<MeshDisplay>(); // Empty displays for use
        private static float poolGenMSLimit = 2;
        private static int minPooledDisplays = 50;       
        //     

		public static void Init ()
		{
            //Setup camera and game objects
			drawGameObject = new GameObject ("DrawGameObject");						
            meshesGameObject = new GameObject("Meshes");
            mainCamera = Camera.main.gameObject;
            drawGameObject.transform.parent = mainCamera.transform;
            meshesGameObject.transform.parent = drawGameObject.transform;
            //Make new display lists  
            pooledDisplays = new List<MeshDisplay>();
            meshDisplays = new List<MeshDisplay>();
        }

		public static void Draw ()
		{
			UpdateCameraPos ();
            GenDisplayPool();
            ApplyMeshUpdates ();
		}

		#region Camera
		private static void UpdateCameraPos ()
		{
			//TargetSize = GameManager.currentGame.currentZoomScale;
			cameraOrthoSize = Mathf.SmoothStep (Camera.main.orthographicSize, targetSize, 0.25f);
			if (zoomToMouse && cameraOrthoSize - targetSize > 0.01f) {
				Vector3 MousePos3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 MousePos = new Vector2 (MousePos3.x, MousePos3.y);
				float OrthoMultiplier = (2f / Camera.main.orthographicSize * (Camera.main.orthographicSize - cameraOrthoSize));	
				Vector2 MouseOffset = (MousePos - targetCameraPos) * OrthoMultiplier;
				targetCameraPos += MouseOffset;
			} 
			cameraPos += (targetCameraPos - cameraPos) * cameraEase;
			//
			Camera.main.transform.position = new Vector3 (cameraPos.x, cameraPos.y, -100);
			Camera.main.orthographicSize = cameraOrthoSize;

		}
		public static void MoveCameraBy (Vector2 amount)
		{
			targetCameraPos = targetCameraPos + amount;
		}
		public static void MoveCameraTo (Vector2 pos, float zoom = 0)
		{
			targetCameraPos = pos;
			if (zoom != 0) {
				//GameManager.currentGame.currentZoomScale = zoom;
			}
		}
        #endregion
        #region Meshes       
        private static void GenDisplayPool() {
            float startTime = Time.realtimeSinceStartup;
            while (pooledDisplays.Count < minPooledDisplays && Time.realtimeSinceStartup - startTime < poolGenMSLimit) {
                pooledDisplays.Add(new MeshDisplay(new Vector2(1, 1), TextureManager.DefaultAtlas));
            }
        }

        private static void ApplyMeshUpdates() {
            for (int i = 0; i < meshDisplays.Count; i++) {
                meshDisplays[i].ApplyUpdate();
            }
        }
        public static void UpdateEntity(Entity entity, Vector2 overrideVector = default(Vector2)) {           
            if (entity.meshDisplay == null) {
                AssignMeshDisplayToEntity(entity);
            }
            UpdateMeshDisplay(entity,overrideVector);
        }
        private static void UpdateMeshDisplay(Entity entity, Vector2 overrideVector = default(Vector2)) {
            Graphic graphic = entity.mainGraphic;
            MeshDisplay meshDisplay = entity.meshDisplay;
            //
            meshDisplay.setVisibility(entity.visible);
            if (entity.visible) {
                meshDisplay.UpdatePosition();
                if (overrideVector == default(Vector2)) {
                    meshDisplay.UpdateUV(0, 0, graphic.atlasVector);
                } else {
                    meshDisplay.UpdateUV(0, 0, overrideVector);
                }
            }
            //
        }
        private static void AssignMeshDisplayToEntity(Entity entity) {
            MeshDisplay meshDisplay = GetPooledDisplay();
            entity.meshDisplay = meshDisplay;
            meshDisplay.entity = entity;
            entity.meshDisplay.UpdateSpriteAtlas(entity.mainGraphic.spriteAtlas);
            entity.meshDisplay.UpdateScale();
            
        }
        private static MeshDisplay GetPooledDisplay() {
            if (pooledDisplays.Count == 0) {
                GenDisplayPool();
            }
            MeshDisplay md = pooledDisplays[0];
            meshDisplays.Add(md);
            pooledDisplays.RemoveAt(0);
            return md;
        }
        private static void CleanMeshDisplay(MeshDisplay meshDisplay) {
            if (meshDisplay != null) {
                meshDisplay.entity = null;
                meshDisplay.setVisibility(false);
                pooledDisplays.Add(meshDisplay);
                meshDisplays.Remove(meshDisplay);
            }
        }
        #endregion
    }
}