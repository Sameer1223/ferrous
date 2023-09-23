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

        //此脚本永不消毁，并且每次进入初始场景时进行判断，若存在重复的则销毁
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
