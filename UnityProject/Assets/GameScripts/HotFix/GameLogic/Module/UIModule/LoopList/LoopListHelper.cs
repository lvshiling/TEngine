using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameLogic
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class LoopListHelper : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public GameObject item;
        private int _totalCount = -1;
        public int totalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                Display();
            }
        }
        public UnityAction<Transform, int> itemRenderer;
        // Implement your own Cache Pool here. The following is just for example.
        Stack<Transform> pool = new Stack<Transform>();
        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(item);
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            // Use `DestroyImmediate` here if you don't need Pool
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            itemRenderer?.Invoke(transform, idx);
        }
        LoopScrollRect ls;
        void Display()
        {
            if (ls == null)
            {
                ls = GetComponent<LoopScrollRect>();
                ls.prefabSource = this;
                ls.dataSource = this;
            }
            if (ls != null)
            {
                ls.totalCount = totalCount;
                ls.RefillCells();
            }
        }
    }
}