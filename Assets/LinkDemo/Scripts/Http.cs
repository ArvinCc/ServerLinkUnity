using UnityEngine;
using System.Collections;
using System.Text;
using Mx.Server;

public class Http : MonoBehaviour
{
    private const string url = "http://127.0.0.1:8000";


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SendHttpGet();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            SendHttpPost();
        }
    }


    public void SendHttpGet()
    {
        StartCoroutine(HttpGet());
    }

    public void SendHttpPost()
    {
        StartCoroutine(HttpPost());
    }

    IEnumerator HttpGet()
    {

        WWW getData = new WWW(url + "?user=2&&password=00");
        yield return getData;
        if (getData.error != null)
        {
            Debug.Log(getData.error);
        }
        else
        {
            Debug.Log(getData.text);
        }  

        yield return null;
    }


    IEnumerator HttpPost()
    {
        WWW postData = new WWW(url, Encoding.UTF8.GetBytes(UserData.Instance.GetOrderData("二狗的小孩", "B")));
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Debug.Log(postData.text);
        }  

        yield return null;
    }

}
