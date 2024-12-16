using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class EventSystemManager : MonoBehaviour
{


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        SocketManagergame.OnListner_OUP += OnRecevied_OUP;
        SocketManagergame.OnListner_MP += OnRecevied_MP;
       
       
    }

    private void OnDisable()
    {

        SocketManagergame.OnListner_OUP -= OnRecevied_OUP;
        SocketManagergame.OnListner_MP -= OnRecevied_MP;
        

    }
    public void OnRecevied_OUP(JSONNode data)
    {
        GameObject prefabs = Resources.Load<GameObject>("ProfilePanel");
        Instantiate(prefabs, GameObject.Find("Canvas").transform,false);

        prefabs.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).transform.Find("ProfileNameTxt").GetComponent<Text>().text = data["pn"];
    }

    public void OnRecevied_MP(JSONNode data)
    {
        



    }


}
