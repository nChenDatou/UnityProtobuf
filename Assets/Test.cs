using TestGoogleProtoBuff;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        personInfo info = new personInfo();
        info.Name = "ys";
        info.Age = 50;
        info.Money = 9999;
        info.Phone.Add(new personInfo.Types.PhoneNumber { Number = "12123", Type = PhoneType.Home });
        byte[] msg = ProtobufUtility.Serialize(info);
        Debug.Log(msg.Length);

        personInfo deinfo = ProtobufUtility.DeSerialize<personInfo>(msg);
        Debug.Log("姓名：" + deinfo.Name);
        Debug.Log("年龄：" + deinfo.Age);
        Debug.Log("资产：" + deinfo.Money);
        Debug.Log($"{deinfo.Phone[0].Type}的电话号：{deinfo.Phone[0].Number}");
    }
}

