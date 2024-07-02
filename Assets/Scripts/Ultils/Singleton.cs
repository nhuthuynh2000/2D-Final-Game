using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T m_ins;

    public static T Ins
    {
        get
        {
            if (!m_ins)
            {
                m_ins = GameObject.FindAnyObjectByType<T>();
                if (!m_ins)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    m_ins = singleton.AddComponent<T>();
                }
            }
            return m_ins;
        }
    }

    public virtual void Awake()
    {
        MakeSingleton(true);
    }

    public virtual void Start()
    {

    }

    public void MakeSingleton(bool destroyOnLoad)
    {
        if (!m_ins)
        {
            m_ins = this as T;
            if (destroyOnLoad)
            {
                var root = transform.root;
                if (root != transform)
                {
                    DontDestroyOnLoad(root);
                }
                else
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
