using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public abstract class GuiFrame
    {
        private bool destroyRoot = true;
        protected List<GuiFrame> attachedFrames = new List<GuiFrame>();

        public virtual string PrefabName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual GameObject Root
        {
            get;
            protected set;
        }

        public T AddFrame<T>(RectTransform parentTransform) where T : GuiFrame, new()
        {
            T newFrame = new T();
            newFrame.SetItemFrame(parentTransform);

            attachedFrames.Add(newFrame);
            return newFrame;
        }

        public void RemoveFrame(GuiFrame removedFrame)
        {
            attachedFrames.Remove(removedFrame);
            removedFrame.Dispose();
        }

        public GameObject SetItemFrame(Transform parent)
        {
            Root = FormManager.Instance.GetFrame(PrefabName);

            if (Root != null)
            {
                RectTransform rectTransform = Root.GetComponent<RectTransform>();
                rectTransform.SetParent(parent, false);
                rectTransform.localScale = Vector3.one;
                rectTransform.anchoredPosition = Vector3.zero;
                rectTransform.rotation = Quaternion.identity;

                InitFormControls();

                return Root;
            }

            return null;
        }

        protected virtual void InitFormControls()
        {

        }

        public virtual void Dispose()
        {
            for (int i = 0; i < attachedFrames.Count; i++)
            {
                attachedFrames[i].Dispose();
            }

            if (destroyRoot)
            {
                GameObject.Destroy(Root);
                Root = null;
            }
        }

        public GuiForm parentForm;
        private bool isEnabled;
        public virtual void Enable()
        {
            if (Root != null && !isEnabled)
            {
                isEnabled = true;
                if (!Root.activeSelf)
                    Root.SetActive(true);
            }
        }

        public virtual void Disable()
        {
            if (Root != null)
            {
                isEnabled = false;

                Root.SetActive(false);
            }
        }

        public bool Enabled
        {
            get
            {
                return Root.activeSelf;
            }
        }

        protected T GetView<T>()
        {
            return Root.GetComponent<T>();
        }
    }
}
