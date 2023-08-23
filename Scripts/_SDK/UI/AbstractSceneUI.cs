using Assets._SDK.Game;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _SDK.UI
{
    public abstract class AbstractSceneUI : MonoBehaviour
    {
        protected Dictionary<string, AbstractPanel> _panels;
        void Start()
        {
           
            OnStart();
        }
        protected abstract void OnStart();
        protected void ShowPanel(string panelName)
        {
            GetOrAddPanel(panelName).gameObject.SetActive(true);
        }

        protected void HidePanel(string panelName)
        {
            GetOrAddPanel(panelName).gameObject.SetActive(false);
        }

        protected AbstractPanel GetOrAddPanel(string panelName)
        {
			_panels ??= new Dictionary<string, AbstractPanel>();
			if (!_panels.ContainsKey(panelName))
            {
                var panel = gameObject?.transform.Find(panelName).gameObject.GetComponent<AbstractPanel>();
                
                _panels.Add(panelName, panel);
            }
            return _panels[panelName];
        }

    }
}