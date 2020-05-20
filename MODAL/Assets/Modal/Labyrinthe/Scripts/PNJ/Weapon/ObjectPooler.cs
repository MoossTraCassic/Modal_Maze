using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.PNJ
{
    public class ObjectPooler<T> where T : UnityEngine.MonoBehaviour, IPooled<T>
    {
        public T[] instances;
 
        protected Queue<int> m_FreeIdx;

        public void Initialize(int count, T prefab)
        {
            instances = new T[count];
 
            m_FreeIdx = new Queue<int>(count);

            for (int i = 0; i < count; ++i)
            {
                instances[i] = Object.Instantiate(prefab);
                instances[i].gameObject.SetActive(false);
                instances[i].poolID = i;
                instances[i].pool = this;

                m_FreeIdx.Enqueue(i);
            }
        }

        public T GetNew()
        {
  
            int idx = m_FreeIdx.Dequeue();
            instances[idx].gameObject.SetActive(true);

            return instances[idx];
        }

        public void Free(T obj)
        {
 
            m_FreeIdx.Enqueue(obj.poolID);
        }
    }

    public interface IPooled<T> where T : MonoBehaviour, IPooled<T>
    {
        int poolID { get; set; }
        ObjectPooler<T> pool { get; set; }
    }

}