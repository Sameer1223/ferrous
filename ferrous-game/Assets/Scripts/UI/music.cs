using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class music : MonoBehaviour
{
    static music _instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static music instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<music>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    void Awake()
    {

        //�˽ű��������٣�����ÿ�ν����ʼ����ʱ�����жϣ��������ظ���������
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }

    }
    


    // Update is called once per frame
    void Update()
    {
        
    }
}
