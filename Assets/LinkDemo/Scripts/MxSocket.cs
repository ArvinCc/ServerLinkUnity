using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.UI;
using System.Threading;
using Mx.Server;
using System.IO;
using Mx.Json;
    
public class MxSocket : MonoBehaviour
{

    public Text textName;
    public Text textPath;
    public Text progressText;
    public Image progressImage;


    public string path = "ppp\\pp\\p\\测试游戏ppp.unitypackage"
;
    void Start()
    {
        Init();

        
        Debug.Log("||" + Directory.Exists(Path.GetDirectoryName("E:/Arvin/TdseLog.txt")));

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DownLoadFile("测试游戏.unitypackage", "E:");
           // DownLoadFile("大叔.txt", "E:");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            User o = new User();
            o.name="么么";
            o.password = "12345";
            o.phoneNumber = "42399999";
            SignIn(o);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            User o = new User();
            o.name = "么么1";
            o.password = "123456";
            o.phoneNumber = "423888888888";
            SignIn(o);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GetProducData();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            GetVipTime();
        }
    }

    void OnDisable()
    {
        Close();
    }


    public void Init()
    {
        try
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect("123.207.52.85", 3300);
            //client.Connect("127.0.0.1",8080);
            Debug.Log("连接服务器成功\r\n");
        }
        catch
        {
            Debug.Log("连接服务器失败\r\n");
        }
    }

    public void Down()
    {
        DownLoadFile(textName.text, textPath.text);
    }

    public void Pause()
    {
        if (dl != null)
        {

            dl.Pause();
        }
    }

    public void Continue()
    {
        if (dl != null)
        {
            dl.Resume();
        }
    }


    private Socket client;


    public void SignIn(User user)
    {
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.SignIn(user.name, user.password, user.phoneNumber))));
    }


    public void SignUp(User user)
    {
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.SignUp(user.name, user.password, user.phoneNumber))));
    }


    public void GetVipData() 
    {
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.GetVipData("A"))));
    }

    public void GetOrderData()
    {
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.GetOrderData("二狗","A"))));
    }
    public void GetProducData()
    {
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.GetProducData(""))));//获得所有项目信息
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.GetProducData("地铁安全"))));//获得地铁安全信息
    }
    public void GetVipTime()
    {
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.GetUserVipTime("二狗", "second"))));//获得剩余时间秒数
        StartCoroutine(Send(Encoding.UTF8.GetBytes(UserData.Instance.GetUserVipTime("二狗", "all"))));//获得到期时间
    }
    IEnumerator Send(byte[] content)
    {
        Message msg = new Message(client, content);

        msg.Start();

        while (!msg.GetIsDone())
        {

            Debug.Log("发送中");
            //  "正在发送中";

            yield return new WaitForSeconds(0);
        }

        yield return new WaitForEndOfFrame();

        //获得服务器返回的信息
        Debug.Log(msg.GetContent());
        //JSONArray jsonObjectsss = new JSONArray(msg.GetContent());

        //object json = new JSONTokener(msg.GetContent());

        //JSONObject jsonObjectsss = new JSONObject(msg.GetContent());

        //Debug.Log("！！！JSONObject当前:" + jsonObjectsss.getString("callback_url"));

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="depositPath"></param>
    public void DownLoadFile(string fileName, string depositPath)
    {
        if (isLoad)
            return;

        StartCoroutine(Load(fileName, depositPath));
    }


    private DownLoad dl;

    private bool isLoad;

    IEnumerator Load(string fileName, string depositPath)
    {
        progressImage.fillAmount = 0;

        isLoad = true;

        dl = new DownLoad(client, depositPath, fileName);

        dl.Start();

        while (!dl.GetIsDone())
        {
            float progress = (float)dl.GetCurrentSize() / (float)dl.GetFileSize();

            progressText.text = (int)(progress * 100) + "%";

            progressImage.fillAmount = progress;

            Debug.Log("下载中~~~~~~" + dl.GetCurrentSize() + "KB/" + dl.GetFileSize() + "KB" + "||fileName" + dl.GetFileName()+"||Name"+dl.GetName()+"||Path"+dl.GetPath());

            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("下载完成~~~~~~" + dl.GetCurrentSize() + "/" + dl.GetFileSize());

        isLoad = false;

        progressText.text = "100%";

        progressImage.fillAmount = 1;

        dl = null;

        //Debug.Log("内容是：" + File.ReadAllText(depositPath + "\\" + fileName, Encoding.UTF8));
    }

    public void Close()
    {
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}

