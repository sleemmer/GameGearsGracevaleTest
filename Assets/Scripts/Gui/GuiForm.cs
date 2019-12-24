using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public abstract class GuiForm
    {
        private GameObject Root
        {
            get;
            set;
        }

        public bool IsOpen
        {
            get;
            private set;
        }

        protected List<GuiFrame> attachedFrames = new List<GuiFrame>();

        public virtual void Init(GameObject formRoot)
        {
            Root = formRoot;
            IsOpen = false;
        }

        public virtual void Dispose()
        {
            Root = null;
        }

        public virtual void Show()
        {
            if (Root != null)
            {
                IsOpen = true;
                Root.SetActive(true);
            }
        }

        public T AddFrame<T>(RectTransform parentTransform) where T : GuiFrame, new()
        {
            T newFrame = new T();
            newFrame.parentForm = this;

            newFrame.SetItemFrame(parentTransform);

            attachedFrames.Add(newFrame);

            return newFrame;
        }

        public void RemoveFrame(GuiFrame removedFrame)
        {
            attachedFrames.Remove(removedFrame);
            removedFrame.Dispose();
        }

        public virtual void Hide()
        {
            if (Root != null)
            {
                IsOpen = false;
                Root.SetActive(false);
            }
        }

        public virtual void Disable()
        {
            if (Root != null)
            {
                Root.SetActive(false);
            }
        }

        public virtual void Enable()
        {
            if (Root != null)
            {
                Root.SetActive(IsOpen);
            }
        }

        protected T GetView<T>()
        {
            return Root.GetComponent<T>();
        }
    }
}