using Assets.Scripts.Gui;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class FormManager : IGameManager
    {
        private static FormManager instance = null;

        public static FormManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormManager();
                }
                return instance;
            }
        }

        private Transform guiCanvasTransform;
        private Dictionary<string, GameObject> formsCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> framesCache = new Dictionary<string, GameObject>();

        LocationForm locationForm = null;

        public override void Start()
        {
            base.Start();

            // Загружаем все ресурсы для LocationForm
            guiCanvasTransform = GameObject.Find("GuiCanvas").transform;
            ShowStartForm();
        }

        public void ShowStartForm()
        {
            locationForm = new LocationForm();
            locationForm.Init(GetForm("LocationForm", guiCanvasTransform));
            locationForm.Show();
        }

        public GameObject GetForm(string formName, Transform parent = null)
        {
            if (!formsCache.ContainsKey(formName))
            {
                string path = "Gui/Forms/" + formName;
                GameObject formPrefab = Resources.Load<GameObject>(path);
                formsCache.Add(formName, formPrefab);
            }

            GameObject uiObj = UnityEngine.Object.Instantiate(formsCache[formName], parent) as GameObject;
            uiObj.SetActive(false);

            return uiObj;
        }

        public GameObject GetFrame(string framePath, Transform parent = null)
        {
            if (!framesCache.ContainsKey(framePath))
            {
                string path = "Gui/Frames/" + framePath;
                GameObject framePrefab = Resources.Load<GameObject>(path);
                framesCache.Add(framePath, framePrefab);
            }

            GameObject uiObj = UnityEngine.Object.Instantiate(framesCache[framePath], Vector3.zero, Quaternion.identity) as GameObject;
            uiObj.SetActive(false);

            return uiObj;
        }

        public override void Update()
        {
            base.Update();

            if (locationForm != null)
            {
                locationForm.Update();
            }
        }
    }
}