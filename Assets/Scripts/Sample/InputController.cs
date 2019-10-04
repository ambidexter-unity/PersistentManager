using System.Collections;
using System.Collections.Generic;
using Common.PersistentManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Sample
{
    public class InputController : MonoBehaviour
    {

#pragma warning disable 649
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private InputField _input;
        [SerializeField] private Text _output;

        [Inject] private readonly IPersistentManager _persistentManager;
#pragma warning restore 649

        private void Start()
        {
            _persistentManager.Initialize();
            
            _saveButton.onClick.AddListener(Save);
            
            _loadButton.onClick.AddListener(Load);
        }

        private void OnDestroy()
        {
            _saveButton.onClick.RemoveAllListeners();
            _loadButton.onClick.RemoveAllListeners();
        }

        private void Save()
        {
            PersistentObject ob = new PersistentObject();
            ob.Data = _input.text;
            _input.text = "";
            _persistentManager.Persist(ob);

        }
        
        private void Load()
        {
            PersistentObject ob = new PersistentObject();
            _persistentManager.Restore(ob);
            _output.text = ob.Data;
        }
        
    }
}