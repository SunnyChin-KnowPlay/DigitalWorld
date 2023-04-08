﻿using System.Collections.Generic;
using UnityEngine;

namespace DreamEngine.Core
{
    /// <summary>
    /// GameObject的池子
    /// </summary>
    public class GameObjectPool
    {
        #region Params
        private readonly Queue<GameObject> pool;
        private readonly GameObject prefab;
        private readonly Transform root;
        #endregion

        #region Logic
        public GameObjectPool(Transform trans, GameObject prefab)
        {
            this.root = trans;
            this.prefab = prefab;
            this.pool = new Queue<GameObject>();
        }

        public GameObject Allocate()
        {
            if (pool.Count > 0)
                return pool.Dequeue();

            GameObject go = Object.Instantiate(prefab);
            return go;
        }

        public void Recycle(GameObject go)
        {
            if (go == null) return;
            go.transform.SetParent(root);
            go.SetActive(false);
            pool.Enqueue(go);
        }

        public void Clear()
        {
            foreach (var item in pool)
                Object.Destroy(item);
            pool.Clear();
        }
        #endregion
    }
}